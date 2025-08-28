using Zenject;

public class SelectBattleSurvivorViewMediator : IMediator
{
    [Inject] private SelectBattleSurvivorProxy selectPlayerProxy;
    private SelectBattleSurvivorView view;

    public override void Register(IView view)
    {
        this.view = view as SelectBattleSurvivorView;
    }

    [Listener(SelectPlayerEvent.ON_SHOW_SELECT_PLAYER)]
    public void ShowSelectPlayer()
    {
        view.Show();
    }

    public void HideSelectPlayer()
    {
        //取消選取當前關卡 退出縮放 並且ResetView

    }
    public bool HasSelectedPlayers(int playerId)
    {
        return selectPlayerProxy.selectedPlayers != null && selectPlayerProxy.selectedPlayers.Length > 0 && System.Array.IndexOf(selectPlayerProxy.selectedPlayers, playerId) >= 0;
    }

    public void SelectPlayer(int playerId)
    {
        selectPlayerProxy.AddSelectPlayer(playerId);
    }
    public void RemoveSelectPlayer(int playerId)
    {
        selectPlayerProxy.RemoveSelectPlayer(playerId);
    }
    public void RemoveAllSelectPlayers()
    {
        selectPlayerProxy.RemoveAllSelectPlayers();
    }
    public int[] GetSelectedPlayers()
    {
        return selectPlayerProxy.selectedPlayers;
    }
}
