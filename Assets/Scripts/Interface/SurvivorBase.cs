using UnityEngine;
using DG.Tweening;
using System;
using Lean.Touch;

public class SurvivorBase : MonoBehaviour
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isBusy;
    private bool isDragging = false;

    public SurvivorBase GetSurvivor()
    {
        return this;
    }

    public void SetBusy(bool isBusy, Action callBack = null)
    {
        this.isBusy = isBusy;
        DOVirtual.DelayedCall(1, () =>
        {
            callBack?.Invoke();
        }).SetId(GetHashCode());
    }
    public void Hit()
    {
        sprite.color = Color.red;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            sprite.color = Color.white;
        }).SetId(GetHashCode());
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }
    public void OnPick(Vector3 pickPosition)
    {
        transform.position = new Vector3(pickPosition.x, pickPosition.y, -0.01f);
    }
    public void SetCollider(bool isCollider)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = isCollider;

    }
    public void OnDrop(Vector3 floor)
    {
        transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, floor.y, 0);

    }

    // void OnEnable()
    // {
    //     LeanTouch.OnFingerDown += OnFingerDown;
    //     LeanTouch.OnFingerUp += OnFingerUp;
    //     LeanTouch.OnFingerUpdate += OnFingerUpdate;
    // }

    // void OnDisable()
    // {
    //     LeanTouch.OnFingerDown -= OnFingerDown;
    //     LeanTouch.OnFingerUp -= OnFingerUp;
    //     LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    // }

    void OnFingerDown(LeanFinger finger)
    {
        if (finger.StartedOverGui) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.transform == transform)
        {
            isDragging = true;
        }
    }

    void OnFingerUpdate(LeanFinger finger)
    {
        if (!isDragging) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    void OnFingerUp(LeanFinger finger)
    {
        isDragging = false;
    }
}
