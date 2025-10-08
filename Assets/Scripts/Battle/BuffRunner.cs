using DG.Tweening;
using UnityEngine;

public class BuffRunner : MonoBehaviour, IHittable
{
    public GunData gunData;

    public float hp;
    public float speed;
    private void OnEnable()
    {
        transform.position = new Vector2(UnityEngine.Random.Range(-2f, 2f), transform.position.y);
        transform.DOMoveY(-3, speed).SetEase(Ease.Linear);
    }

    public void GetDamaged(int damage)
    {
        //算次數不算傷害
        hp -= 1;
        if (hp <= 0)
        {
            //死亡邏輯
            Destroy(gameObject);
        }
    }

    public Vector2 GetFixedPosition()
    {
        return transform.position;
    }

    public void ResetView()
    {
        DOTween.Kill(GetHashCode());
    }
}
