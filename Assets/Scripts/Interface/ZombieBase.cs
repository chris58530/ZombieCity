using UnityEngine;

public class ZombieBase : MonoBehaviour, IPoolable
{
    public int id;
    public AnimationView animationView;


    public ZombieBase GetZombie()
    {
        return this;
    }
    public void Hit()
    {

    }
    public void OnSpawned()
    {
        animationView.Show();
    }

    public void OnDespawned()
    {
        animationView.Hide();

    }
}
