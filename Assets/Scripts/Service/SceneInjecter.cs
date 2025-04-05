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
    }


}
