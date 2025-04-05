using UnityEngine;
using Zenject;

public class InjectService : Singleton<InjectService>
{
    [Inject] private DiContainer diContainer;
    public void Inject<T>(T instance)
    {
        diContainer.Inject(instance);
    }
}
