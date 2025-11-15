using UnityEngine;
using UnityEngine.UI;

public class BoneColliderAutoSet : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Image image;
    
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        boxCollider.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        boxCollider.offset = new Vector2(0f, image.rectTransform.rect.height / 2f);
    }
}
