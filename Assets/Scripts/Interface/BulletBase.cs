using System;
using UnityEngine;

public class BulletBase : MonoBehaviour,IPoolable
{
    public IHittable hittable;
    public Action onHitCallBack;
    public void OnDespawned()
    {
    }

    public void OnSpawned()
    {
    }
    private void OTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHittable hittable))
        {
        }
    }
}
