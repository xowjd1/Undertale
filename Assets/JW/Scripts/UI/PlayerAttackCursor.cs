using UnityEngine;

public class PlayerAttackCursor : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public float GetPosX()
    {
        return rectTransform.anchoredPosition.x;
    }
}
