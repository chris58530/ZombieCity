using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform panelRectTransform;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation lastScreenOrientation = ScreenOrientation.AutoRotation;

    void Awake()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Rect currentSafeArea = Screen.safeArea;
        ScreenOrientation currentScreenOrientation = Screen.orientation;

        if (currentSafeArea != lastSafeArea || currentScreenOrientation != lastScreenOrientation)
        {
            ApplySafeArea(currentSafeArea);
            
            lastSafeArea = currentSafeArea;
            lastScreenOrientation = currentScreenOrientation;
        }
    }

    private void ApplySafeArea(Rect safeArea)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;


        Vector2 anchorMin = new Vector2(safeArea.x / screenWidth, safeArea.y / screenHeight);

        Vector2 anchorMax = new Vector2((safeArea.x + safeArea.width) / screenWidth, (safeArea.y + safeArea.height) / screenHeight);

        panelRectTransform.anchorMin = anchorMin;
        panelRectTransform.anchorMax = anchorMax;
        
        panelRectTransform.offsetMin = Vector2.zero;
        panelRectTransform.offsetMax = Vector2.zero;

    }
}
