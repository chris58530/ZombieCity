using UnityEngine;

public class FloorBase : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField]private Transform enterPosition;
    [SerializeField] private FacilityBase[] facilities;
    public Transform GetEnterPosition()
    {
        return enterPosition;
    }
    public FacilityBase[] GetFacilities()
    {
        return facilities;
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
