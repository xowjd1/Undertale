using UnityEngine;
using UnityEngine.UI;

public class BattlePanelColliderAutoSet : MonoBehaviour
{
    private const float interColliderOffsetX = 30f;
    private const float interColliderOffsetY = 30f;
    
    [SerializeField] private BoxCollider2D interCollider;
    [SerializeField] private BoxCollider2D outerCollider;
    
    private Image panel;

    private void Awake()
    {
        panel = GetComponent<Image>();
    }

    private void Update()
    {
        outerCollider.offset = new Vector2(0f, panel.rectTransform.rect.height / 2f);
        interCollider.offset = outerCollider.offset;
        outerCollider.size = new Vector2(panel.rectTransform.rect.width, panel.rectTransform.rect.height);
        interCollider.size = new Vector2(outerCollider.size.x - interColliderOffsetX,
            outerCollider.size.y - interColliderOffsetY);
    }
}
