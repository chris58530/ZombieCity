using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class SurvivorBase : MonoBehaviour
{
    [Header("Setting")]
    public int id;
    public int level;
    public FloorType stayingFloor;
    [SerializeField] private AnimationView animationView;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject emotionImage;
    public Action<int, FloorType> onSaveStayingFloor;
    public Action<int, int> onSaveLevel;
    protected SurvivorJsonData survivorJsonData;
    public void SetData(SurvivorJsonData survivorJsonData)
    {
        this.survivorJsonData = survivorJsonData;
    }
    public void SetStayingFloor(FloorBase stayingFloor)
    {
        this.stayingFloor = stayingFloor.floorType;
        float floorY = stayingFloor.GetEnterPosition().y +stayingFloor.transform.position.y;
        float randomX = UnityEngine.Random.Range(stayingFloor.GetLimitPositionX().x, stayingFloor.GetLimitPositionX().y);
        transform.position = new Vector3(randomX, floorY, 1);
        Debug.Log($"SetStayingFloor {id} to {floorY}");
    }
    public void SetFlip(bool isFlip)    
    {
        sprite.flipX = isFlip;
    }
    public void OnPick(Vector3 pickPosition)
    {
        sprite.color = Color.white;
        transform.position = new Vector3(pickPosition.x, pickPosition.y, -0.01f);
    }
    public void SetCollider(bool isCollider)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = isCollider;

    }
    public void OnDrop(Vector3 dropPos, FloorType floorType)
    {
        SetCollider(true);
        OnSetStayingFloor(floorType);
        sprite.color = Color.white;
        transform.localScale = Vector3.one;
        transform.position = dropPos;
    }
    public virtual void OnAddLevel(int level)
    {
        this.level += level;
        onSaveLevel?.Invoke(id, level);
    }
    public virtual void OnSetStayingFloor(FloorType floorType)
    {
        stayingFloor = floorType;
        onSaveStayingFloor?.Invoke(id, floorType);
    }
}
