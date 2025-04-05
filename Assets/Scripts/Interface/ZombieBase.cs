using UnityEngine;

public class ZombieBase : MonoBehaviour, IPoolable
{
    public int id;
    public AnimationView animationView;



    public void OnSpawned()
    {
        animationView.Show();
    }

    public void OnDespawned()
    {
        animationView.Hide();

    }
}
