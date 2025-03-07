using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Kittens.Services;
using KittensLibrary;
using System.Text.Json;
using Protocol.Converter;
using Protocol;
using Protocol.Packets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Core.Extensions;


namespace Kittens.ViewModel;


public partial class GameViewModel: BaseViewModel
{

    public event Action<Card, Card, Card> SeeTheFutureUI;
    public event Action ExplodingKittenUI;
    public event Action DefuseUI;
    public event Action AttackUI;
    public event Action SkipUI;
    public event Action NopeUI;
    public event Action ShuffleUI;
    public event Action StealUI;
    public event Action DisableFalseUI;
    public event Action DisableTrueUI;
    public event Action LoseUi;
    public event Action WinUI;
    public event Action ThirdClientUI;

    private Client _client;
    private Player _player;

    private Card _draggedCard;

    private Card _backCard = new Card("back", CardType.Back, "card_back.png");
    private int _otherCardsCount;

    [ObservableProperty]
    ObservableCollection<Card> playerCards;

    [ObservableProperty]
    ObservableCollection<Card> otherPlayerCards;

    [ObservableProperty]
    Card lastResetCard;


    public GameViewModel()
    {
        Title = "Game";
        Status = "";
        _client = new Client(OnPacketRecieve);
        PlayerCards = new ObservableCollection<Card>();
        OtherPlayerCards = new ObservableCollection<Card>();
    }

    private void Update()
    {
        if (_player.State == State.Play)
        {
            Title =  $"{_player.Nickname} ходит";
            Shell.Current.Dispatcher.Dispatch(DisableTrueUI);
        }
        else if (_player.State == State.Wait)
        {
            Title = $"{_player.Nickname} ожидает";
            Shell.Current.Dispatcher.Dispatch(DisableFalseUI);
        }
        else if (_player.State == State.Win)
        {
            Shell.Current.Dispatcher.Dispatch(WinUI);
        }
        else if (_player.State == State.Lose)
        {
            Shell.Current.Dispatcher.Dispatch(LoseUi);
        }
            PlayerCards.Clear();
        OtherPlayerCards.Clear();

        foreach (var playerCard in _player.Cards.Select(card => Cards.typeCards[card]))
        {
            PlayerCards.Add(playerCard);
        }

        for (int i = 0; i < _otherCardsCount; i++)
        {
            OtherPlayerCards.Add(_backCard);
        }
           
    }

    [RelayCommand]
    public void PlayerCard_Tapped(Card card)
    {
        _client.QueuePacketSend(PacketConverter.Serialize(PacketType.ActionCard, new PacketActionCard() { ActionCard = card.Name }).ToPacket());
    }

    [RelayCommand]
    public void BackCard_Tapped()
    {
        _client.QueuePacketSend(PacketConverter.Serialize(PacketType.TakeCard, new PacketTakeCard() { Test = 0 }).ToPacket());
    }

    public void ConnectToGameCommand(string userName, string email)
    {
        _player = new Player(userName,email);
        Status = "Ожидание подключения";
        _client.Connect("127.0.0.1", 4910);
        
        _client.SendHandshakePacket(userName, email);
        while (_player.State != State.Play && _player.State != State.Wait) { }
    }

    private void OnPacketRecieve(byte[] packet)
    {
        var parsed = Packet.Parse(packet);

        if (parsed != null)
        {
            ProcessIncomingPacket(parsed);
        }
    }

    private void ProcessIncomingPacket(Packet packet)
    {
        var type = PacketTypeManager.GetTypeFromPacket(packet);

        switch (type)
        {
            case PacketType.Handshake:
                ProcessHandshake(packet);
                break;
            case PacketType.FailConnect:
                ProcessFailConnect(packet);
                break;
            case PacketType.StartGame:
                ProcessStartGame(packet);
                break;
            case PacketType.SeeTheFuture:
                ProcessSeeTheFuture(packet);
                break;
            case PacketType.PlayerState:
                ProcessPlayerState(packet);
                break;
            case PacketType.EndGame:
                ProcessEndGame(packet);
                break;
            case PacketType.Unknown:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ProcessPlayerState(Packet packet)
    {
        var playerState = PacketConverter.Deserialize<PacketPlayerState>(packet);
        _player.Cards = playerState.Cards;
        _player.State = playerState.PlayerState;
        _otherCardsCount = playerState.OtherPlayerCardsCount;
        LastResetCard = Cards.typeCards[playerState.LastResetCard];
        Update();
    }
    private void ProcessEndGame(Packet packet)
    {
        var playerState = PacketConverter.Deserialize<PacketEndGame>(packet);
        _player.State = playerState.State;
        Update();
    }

    private void ProcessSeeTheFuture(Packet packet)
    {
        var threeCards = PacketConverter.Deserialize<PacketSeeTheFuture>(packet);
    }
    private void ProcessHandshake(Packet packet)
    {
        Status = "Успешное подключение, ожидаем еще одного игрока";    
    }

    private void ProcessFailConnect(Packet packet)
    {
        var failConnect = PacketConverter.Deserialize<PacketFailConnect>(packet);
        Shell.Current.Dispatcher.Dispatch(ThirdClientUI);

    }

    private void ProcessStartGame(Packet packet)
    {
        var startGame = PacketConverter.Deserialize<PacketStartGame>(packet);
        _player.Cards = startGame.Player.Cards;
        _player.State = startGame.Player.State;
        _otherCardsCount = startGame.OtherPlayerCardsCount;
        Update();
    }
}