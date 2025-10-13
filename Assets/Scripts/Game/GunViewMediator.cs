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
        if (!view.isSetUp)
            view.SetUpGun(gunProxy.gunDataSetting);

        view.StartShooting();
    }

    [Listener(GunEvent.ON_GUN_STOP_SHOOT)]
    public void OnStopShoot()
    {
        view.StopShooting();
    }

    [Listener(BattleSkillEvent.ON_SELECT_ADD)]
    public void OnSelectAdd()
    {
        view.skill_Add = true;
    }

    [Listener(BattleSkillEvent.ON_SELECT_PENETRATE)]
    public void OnSelectPenetrate()
    {
        view.skill_Penetrate = true;
    }

    [Listener(BattleSkillEvent.ON_SELECT_FIRE_RATE)]
    public void OnSelectFireRate()
    {
        view.skill_FireRate = true;
    }

}
