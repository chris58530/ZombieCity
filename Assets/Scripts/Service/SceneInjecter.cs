using System.ComponentModel;
using UnityEngine;
using Zenject;

public class SceneInjecter : MonoInstaller
{
    // This method is called when the scene is loaded
    public override void InstallBindings()
    {
        Container.Bind<Listener>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FloorProxy>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FloorViewMedaitor>().AsSingle().NonLazy();
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
    }


}
