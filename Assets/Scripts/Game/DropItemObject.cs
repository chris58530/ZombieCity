using UnityEngine;

public class DropItemObject : MonoBehaviour
{
    public DropItemType dropItemType;
}
public enum DropItemType
{
    None,
    Money,
}