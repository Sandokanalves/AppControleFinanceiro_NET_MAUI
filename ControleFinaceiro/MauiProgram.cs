
using ControleFinaceiro.Views;
using ControleFinaceiro.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;



namespace ControleFinaceiro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
           
            .RegisterViews();
       
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
    //public static MauiAppBuilder RegisterDatabaseAndRepositories(this MauiAppBuilder mauiAppBuilder)
    //{

        
    //    return serviceCollection;
    //}

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<TransactionAdd>();
        mauiAppBuilder.Services.AddTransient<TransactionEdit>();
        mauiAppBuilder.Services.AddTransient<TransactionList>();
        
        return mauiAppBuilder;
    }
}
