﻿using ControleFinaceiro.Views;

namespace ControleFinaceiro;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage( new TransactionList());
       
    }
}
