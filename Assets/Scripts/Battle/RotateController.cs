using UnityEngine;

public class RotateController : MonoBehaviour
{
    [Header("旋轉設定")]
    public bool enabledRotate = true;
    [SerializeField] private float angleOffset = 0f; // 角度偏移 (度)
    private void Update()
    {
        //外部透過設定enabledRotate開關
        //來控制是否進行旋轉
        //此腳本不處理輸入偵測邏輯
        if (!enabledRotate)
            return;
        ProcessingRotate();
    }

    private void ProcessingRotate()
    {
        Vector3 inputPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, Camera.main.transform.position.z));
        Vector3 direction = (worldPosition - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public void SetRotate(bool rotate)
    {
        enabledRotate = rotate;
        Debug.Log("[Test] RotateController SetRotate to " + rotate);
    }

}
