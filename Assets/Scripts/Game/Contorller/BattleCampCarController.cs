using UnityEngine;
using DG.Tweening;

public class BattleCampCarController : MonoBehaviour, IHittable
{
    public float hp;
    public float moveSpeed = 5f; // 移動速度
    public float maxXPosition = 8f; // 最大X軸移動範圍
    
    private bool isDragging = false;
    private Vector2 touchStartPos;
    private float initialXPos;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log("Touch phase: " + touch.phase);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    initialXPos = transform.position.x;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        float deltaX = (touch.position.x - touchStartPos.x) / Screen.width * maxXPosition;
                        float newX = Mathf.Clamp(initialXPos + deltaX, -maxXPosition, maxXPosition);
                        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }

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