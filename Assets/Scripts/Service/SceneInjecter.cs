using UnityEngine;
using Zenject;

public class SceneInjecter : MonoInstaller
{
    // This method is called when the scene is loaded
    public override void InstallBindings()
    {
        Container.Bind<Listener>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<JsonDataProxy>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<FloorProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FloorViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameCameraProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameCameraViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ZombieSpawnerProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ZombieSpawnerViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ClickHitProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ClickHitViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<IllustrateBookViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SurvivorProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SurvivorViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PassiveHitProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PassiveHitMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ResourceInfoMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ResourceInfoProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TrasitionBackGroundMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TrasitionBackGroundProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DrawCardMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DrawCardProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DropItemProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DropItemViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SelectLevelViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BattleCampCarViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BattleCampCarProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BattleProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelComfirmViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BattleBackGroundViewMediator>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BattleSkillViewMediator>().AsSingle().NonLazy();
        
    }

}
