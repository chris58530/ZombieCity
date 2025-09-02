using Zenject;

public class BattleZombieCounterViewMediator : IMediator
{
    private BattleZombieCounterView view;

    public override void Register(IView view)
    {
        this.view = view as BattleZombieCounterView;

    }
    private void OnZombieCountUpdated(int remaining, int dead, int total)
    {
        if (view != null)
        {
            view.UpdateDisplay(remaining, dead, total);
        }
    }
}
