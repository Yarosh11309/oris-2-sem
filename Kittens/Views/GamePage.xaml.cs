
﻿using System.Net.Sockets;
using System.Text.Json;
using Kittens.ViewModel;
using KittensLibrary;
using Kittens.Services;
using CommunityToolkit.Maui.Views;

namespace Kittens.Views;

public partial class GamePage : ContentPage
{
    private Action<string,Player> Action;

    public Player Player { get; set; }

    GameViewModel _gameViewModel;


    public GamePage(GameViewModel gameViewModel)
	{
		InitializeComponent();
        table.IsVisible = false;
        BindingContext = gameViewModel;
        _gameViewModel = gameViewModel;
        _gameViewModel.DisableFalseUI += DisableFalse;
        _gameViewModel.DisableTrueUI += DisableTrue;
        _gameViewModel.LoseUi += Lose;
        _gameViewModel.WinUI += Win;
        _gameViewModel.ThirdClientUI += ThirdClient;
    }

    private void OnOkClickedAsync(object sender, EventArgs e)
    {   
        _gameViewModel.ConnectToGameCommand(NicknameEntr.Text, EmailEntr.Text);

        login_form.IsVisible = false;
        table.IsVisible = true;  
    }

    public  void Lose()
    {
         DisplayAlert("Конец игры!", "Эх, вы проиграли", "(((");
    }

    public void Win()
    {
         DisplayAlert("Конец игры!", "Поздравляю, вы выиграли", ")))");
    }

    public void DisableFalse()
    {
        gameField.IsEnabled = false;
    }

    public void DisableTrue()
    {
        gameField.IsEnabled = true;
    }

    public void ThirdClient()
    {
        DisplayAlert("Вы третий лишний!", "Закройте это окно", "ок");
    }
}