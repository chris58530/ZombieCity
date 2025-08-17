using UnityEngine;
using DG.Tweening;
using System;

public class BattleCampCarController : MonoBehaviour, IHittable
{
    public GameObject root;
    public float hp;
    private float moveSpeed = .5f; // 基礎移動速度
    private const float MOVE_LIMIT = 2f; // X軸移動限制
    private const float MINIMUM_DRAG_DISTANCE = 10f; // 最小拖曳距離，避免輕微觸碰就移動

    private bool isDragging = false;
    private Vector2 lastInputPos;
    private Vector2 currentInputPos;

    private void Update()
    {
        HandleTouchInput();
    }
    public void ResetView()
    {
        root.SetActive(false);
    }

    private void HandleTouchInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            lastInputPos = currentInputPos = Input.mousePosition;
            isDragging = true;
        }
        // 拖曳中
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentInputPos = Input.mousePosition;
            if (ShouldMove())
            {
                MoveObject();
            }
            lastInputPos = currentInputPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastInputPos = currentInputPos = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        currentInputPos = touch.position;
                        if (ShouldMove())
                        {
                            MoveObject();
                        }
                        lastInputPos = currentInputPos;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
#endif
    }

    private bool ShouldMove()
    {
        float horizontalDelta = Mathf.Abs(currentInputPos.x - lastInputPos.x);
        return horizontalDelta > MINIMUM_DRAG_DISTANCE;
    }

    private void MoveObject()
    {
        // 計算位移並套用移動速度和時間
        float displacement = (currentInputPos.x - lastInputPos.x) * moveSpeed * Time.deltaTime;

        // 更新並限制位置
        float newX = Mathf.Clamp(transform.position.x + displacement, -MOVE_LIMIT, MOVE_LIMIT);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public Vector2 GetFixedPosition()
    {
        return transform.position;
    }

    public void MoveToMiddle(float moveSpeed, Action callBack = null)
    {
        root.SetActive(true);
        transform.position = new Vector3(transform.position.x, 6.5f, transform.position.z);
        transform.DOMoveY(-4.5f, moveSpeed).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }

    public void GetDamaged(int damage)
    {
        hp -= damage;
    }
}
public interface IHittable
{
    void GetDamaged(float damage);
    Vector2 GetFixedPosition();
}