using System;
using UnityEngine;

public class ClickHitController : MonoBehaviour
{
    public Action<ZombieBase> onClickZombie;
    public Action<SurvivorBase> onClickSurvivor;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                OnClickSurvivor(survivor);
            }
        }
    }
    private void OnClickZombie(ZombieBase zombie)
    {
        onClickZombie?.Invoke(zombie);
    }
    private void OnClickSurvivor(SurvivorBase survivor)
    {
        onClickSurvivor?.Invoke(survivor);
    }
}
