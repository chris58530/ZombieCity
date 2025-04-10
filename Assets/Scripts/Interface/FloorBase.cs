using UnityEngine;

public class FloorBase : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform enterPosition;
    [SerializeField] private FacilityBase[] facilities;
    public Vector3 GetEnterPosition()
    {
        return enterPosition.position;
    }
    public FacilityBase[] GetFacilities()
    {
        return facilities;
    }
    public void SetCollider(bool enabled)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = enabled;
    }

    public void SetMask(bool active)
    {
        mask.SetActive(active);
    }
    public FacilityBase CheckEmptyFacility()
    {
        for (int i = 0; i < facilities.Length; i++)
        {
            if (facilities[i].isUsing)
            {
                return facilities[i];
            }
        }
        return null;
    }

}
