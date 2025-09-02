using Zenject;

public class BattleZombieCounterViewMediator : IMediator
{
    private BattleZombieCounterView view;

    public override void Register(IView view)
    {
        this.view = view as BattleZombieCounterView;
        
        // 初始化時顯示 UI
        this.view.ShowAndFadeIn();
        
        // 註冊監聽殭屍數量更新事件
        RegisterZombieCountEvents();
    }

    public override void DeRegister(IView view)
    {
        // 取消註冊事件監聽
        UnregisterZombieCountEvents();
        this.view = null;
    }

    private void RegisterZombieCountEvents()
    {
        // 找到 BattleZombieSpawnerView 並註冊事件
        var spawnerView = UnityEngine.Object.FindObjectOfType<BattleZombieSpawnerView>();
        if (spawnerView != null)
        {
            spawnerView.OnZombieCountUpdated += OnZombieCountUpdated;
        }
    }

    private void UnregisterZombieCountEvents()
    {
        // 取消註冊事件
        var spawnerView = UnityEngine.Object.FindObjectOfType<BattleZombieSpawnerView>();
        if (spawnerView != null)
        {
            spawnerView.OnZombieCountUpdated -= OnZombieCountUpdated;
        }
    }

    private void OnZombieCountUpdated(int remaining, int dead, int total)
    {
        if (view != null)
        {
            view.UpdateDisplay(remaining, dead, total);
        }
    }
}
