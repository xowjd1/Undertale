using UnityEngine;
using UnityEngine.UI;

namespace saera.Scripts
{
    public class SpriteBlinker : MonoBehaviour
    {
        public Image imageComponent;

        public SpriteRenderer spriteRenderer;
        public float blinkInterval = 0.5f;
        void Start()
        {
            // 컴포넌트 가져오기
            spriteRenderer = GetComponent<SpriteRenderer>();
            imageComponent = GetComponent<Image>();
            spriteRenderer.enabled = false;
            imageComponent.enabled = false;
        }

        void Update()
        {
            // Time.time을 이용해 주기적으로 상태를 변경
            
            if (Time.time >= 4.0f && Time.time <= 7.0f)
            {
                bool isVisible = Time.time % (blinkInterval * 2) < blinkInterval;
                spriteRenderer.enabled = isVisible;
                imageComponent.enabled = isVisible;
            }
            
        }

    }
}