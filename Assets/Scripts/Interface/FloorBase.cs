using TMPro;
using UnityEngine;

public class FloorBase : MonoBehaviour
{
    [SerializeField]private int floorProductAmount;
    [SerializeField]private TMP_Text floorProductText;
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform enterPosition; //get y
    private Vector2 limitPositionX;
    [SerializeField] private FacilityBase mainFacilitie;
    [SerializeField] private FacilityBase[] facilities;
    public FloorType floorType;
    public Vector3 GetEnterPosition()
    {
        return enterPosition.position;
    }
    public Vector2 GetLimitPositionX()
    {
        limitPositionX = new Vector2(-2.3f, 2.3f);
        return new Vector2(limitPositionX.x, limitPositionX.y);
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
    public void UseFacility(FacilityBase facilitie)
    {

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
    public void SetProductAmount(int amount)
    {
        floorProductAmount = amount;
        floorProductText.text = floorProductAmount.ToString();
    }
}
public enum FloorType
{
    Main,
    Floor_1,
    Floor_2,
    Floor_3,
    Floor_4,
}
