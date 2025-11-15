using UnityEngine;

public class ArrowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private Sprite newCrossArrowSprite;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask crossArrowLayer;
    [SerializeField] private GameState gameState;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float shakeDuration  = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
            if (h == -1)
            {
                shield.transform.position = new Vector3(-0.5f, 0f, 0f);
                shield.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if (h == 1)
            {
                shield.transform.position = new Vector3(0.5f, 0f, 0f);
                shield.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (v == -1)
            {
                shield.transform.position = new Vector3(0f, -0.5f, 0f);
                shield.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (v == 1)
            {
                shield.transform.position = new Vector3(0f, 0.5f, 0f);
                shield.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
            }
            ChangeNearestCrossArrowSprite();
    }
    private void ChangeNearestCrossArrowSprite()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,
            detectionRadius, crossArrowLayer);

        CrossArrow nearest = null;
        float minDistSq = float.MaxValue;

        foreach (var hit in hits)
        {
            var arrow = hit.GetComponent<CrossArrow>();
            if (arrow == null) continue;

            float distSq = (arrow.transform.position - transform.position).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                nearest = arrow;
            }
        }
        if (nearest != null)
        {
            var sr = nearest.GetComponent<SpriteRenderer>();
            if (sr != null && newCrossArrowSprite != null)
                sr.sprite = newCrossArrowSprite;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Arrow"))
        {
            gameState.playerHp -= 2;
            cameraShake.TriggerShake(shakeDuration, shakeMagnitude);
            Destroy(other.gameObject);
        }
    }
}
