using ControleFinaceiro.Models;
using ControleFinaceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using ControleFinaceiro.Service;

namespace ControleFinaceiro.Views;

public partial class TransactionList : ContentPage
{
    /*
		* Publisher - Subscribers -> 
		* TransactionAdd -> Publisher > Cadastro (Mensagem > Transaction).
		* OK - Subscribers -> TransactionList (Receba o Transaction).
	*/
    TransactionSevice firebase = new TransactionSevice();
    public TransactionList()
	{
		

		InitializeComponent();

        Reload();

        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
		{
			Reload();
        });
	}

	private async void Reload()
	{
		var items = await firebase.GetAll();
        CollectionViewTransactions.ItemsSource = items;

		double income = items.Where(a => a.Type == TransactionType.Income).Sum(a => a.Value);
		double expense = items.Where(a => a.Type == TransactionType.Expense).Sum(a => a.Value);
		double balance = income - expense;

		LabelIncome.Text = income.ToString("C");
		LabelExpense.Text = expense.ToString("C");
		LabelBalance.Text = balance.ToString("C");
    }

	private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
	{
		
		var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
	}

    private void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
		var grid = (Grid)sender;
		var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
		Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
		transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_ToDelete(object sender, TappedEventArgs e)
    {
		await AnimationBorder((Border)sender, true);
		bool result = await App.Current.MainPage.DisplayAlert("Excluir!","Tem certeza que deseja excluir?","Sim", "N�o");

		if (result)
		{
			Transaction transaction = (Transaction)e.Parameter;
		await firebase.Delete(transaction);

			Reload();
		}
		else
		{
			await AnimationBorder((Border)sender, false);
		}
    }

	private Color _borderOriginalBackgroundColor;
	private String _labelOriginalText;
	private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
	{
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
		{
            _borderOriginalBackgroundColor = border.BackgroundColor;
			_labelOriginalText = label.Text;

            await border.RotateYTo(90, 500);

			border.BackgroundColor = Colors.Red;
			
			label.TextColor = Colors.White;
			label.Text = "X";

            await border.RotateYTo(180, 500);
        }
		else
		{
			await border.RotateYTo(90, 500);

			border.BackgroundColor = _borderOriginalBackgroundColor;
			label.TextColor = Colors.Black;
			label.Text = _labelOriginalText;

            await border.RotateYTo(0, 500);
        }
	}
}