using UnityEngine;
using DG.Tweening;

public class BattleCampCarController : MonoBehaviour, IHittable
{
    public float hp;

    public Vector2 GetFixedPosition()
    {
        return transform.position;
    }

    public void Hit()
    {
        hp -= 1;
    }

    public void MoveToMiddle(float moveSpeed)
    {
        transform.position = new Vector3(transform.position.x, 6.5f, transform.position.z);
        transform.DOMoveY(-4.5f, moveSpeed).SetEase(Ease.InOutQuad);
    }

}
public interface IHittable
{
    void Hit();
    Vector2 GetFixedPosition();
}