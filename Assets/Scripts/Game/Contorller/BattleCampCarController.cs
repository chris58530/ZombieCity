using UnityEngine;
using DG.Tweening;

public class BattleCampCarController : MonoBehaviour
{
    public float hp;
    public void MoveToMiddle(float moveSpeed)
    {
        transform.position = new Vector3(transform.position.x, -7f, transform.position.z);
        transform.DOMoveY(-2.5f, moveSpeed).SetEase(Ease.InOutQuad);
    }

}