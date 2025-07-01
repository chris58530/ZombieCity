public class SelectLevelViewMediator : IMediator
{
    private SelectLevelView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as SelectLevelView;
    }
    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
    [Listener(CampCarEvent.ON_SELECT_LEVEL_SHOW)]
    public void ShowSelectLevel()
    {
        view.ShowSelectLevel();
    }
    public void SelectLevelClicked(int levelId)
    {
        //add levelId
        //add spawnSo to proxy
        //todo
        listener.BroadCast(SelectLevelEvent.ON_SELECT_LEVEL_CLICKED);
    }
}
public class LevelProxy : IProxy
{
   
}