using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHitController : MonoBehaviour
{
    public Action<FloorBase> onClickSurvivorUp;
    public Action<ZombieBase> onClickZombie;
    public Action<SurvivorBase, Vector3> onClickSurvivor;
    public bool isPicking;
    private SurvivorBase currentSurvivor;
    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("點擊在 UI 上");
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("ClickHitController");
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = 10f;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 rayPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider == null) return;

            ZombieBase zombie = hit.collider.GetComponent<ZombieBase>();
            if (zombie != null)
            {
                OnClickZombie(zombie);
            }

            SurvivorBase survivor = hit.collider.GetComponent<SurvivorBase>();
            if (survivor != null)
            {
                isPicking = true;
                currentSurvivor = survivor;
            }
        }
        if (Input.GetMouseButton(0) && isPicking && currentSurvivor != null)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = 10f;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 pickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            OnClickSurvivor(currentSurvivor, pickPos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnClickUp();
        }

    }
    private void OnClickUp()
    {
        if (currentSurvivor == null) return;
        currentSurvivor = null;
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 rayPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        int floorLayer = LayerMask.GetMask("Floor");
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, Mathf.Infinity, floorLayer);

        if (hit.collider != null && hit.collider.TryGetComponent(out FloorBase floor))
        {
            Vector3 enterPos = floor.GetEnterPosition();
            onClickSurvivorUp?.Invoke(floor);
        }
    }
    private void OnClickZombie(ZombieBase zombie)
    {
        onClickZombie?.Invoke(zombie);
    }
    private void OnClickSurvivor(SurvivorBase survivor, Vector3 pickPos)
    {
        if (currentSurvivor != survivor)
            return;
        onClickSurvivor?.Invoke(survivor, pickPos);
    }
}
