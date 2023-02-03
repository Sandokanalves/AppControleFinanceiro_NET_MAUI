
using ControleFinaceiro.Models;
using ControleFinaceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;
using ControleFinaceiro.Service;

namespace ControleFinaceiro.Views;

public partial class TransactionAdd : ContentPage
{
    TransactionSevice firebase = new TransactionSevice();
    public TransactionAdd()
	{
		InitializeComponent();
       
	}

    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (IsValidData() == false)
            return;
        
        SaveTransactionInDatabase();

        Navigation.PushModalAsync(new TransactionList());
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private async void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValue.Text)
        };

       await firebase.Add(transaction);
        
       
    }

    private bool IsValidData()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if(string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        }
        double result;
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid = false;
        }


        if(valid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }
        return valid;
    }
}