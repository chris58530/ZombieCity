using Zenject;

public class GunViewMediator : IMediator
{
    [Inject] private GunProxy gunProxy;
    private GunView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as GunView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
        this.view = null;
    }
    [Listener(GunEvent.ON_GUN_START_SHOOT)]
    public void OnStartShoot()
    {
        GunDataSetting setting = gunProxy.gunDataSetting;
        view.SetUpGun(setting);
    }
}
