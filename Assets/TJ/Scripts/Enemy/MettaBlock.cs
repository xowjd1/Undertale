using UnityEngine;

public class MettaBlock : MonoBehaviour
{
   [SerializeField] private float fallSpeed = 5f;
   [SerializeField] private BattlePlayerController player;

   [Header("메타톤하트에 붙어있을 경우")]
   [SerializeField] private float orbitSpeed = 240f;
   [SerializeField] private float baseRadius = 0.8f;   
   [SerializeField] private float bigRadius = 13f;  
   [SerializeField] private float brTime = 3f;  
   [SerializeField] private float expandSpeed = 10f;
   
   private float orbitRadius;
   private float lastTriggerTime;
   private bool isWaiting = true;
   private bool isExpanding = false;
   private bool isContracting = false;
   private float _orbitAngle;
   private Transform _heartPivot;
   private BoxCollider2D _collider;
   private SpriteRenderer _spriteRenderer;

   private bool isArray = false;

   private void Awake()
   {
      orbitRadius = baseRadius;
      lastTriggerTime = Time.time;
      _collider = GetComponent<BoxCollider2D>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
      var heart = GetComponentInParent<MettatonHeart>();
      if (heart != null)
      {
         _heartPivot = heart.transform;
         
         Vector3 offset = transform.position - _heartPivot.position;
         
         orbitRadius = offset.magnitude;
         
         _orbitAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
      }
      var array = GetComponentInParent<BlockArray>();
      if (array != null)
         isArray = true;

   }
   
   private void Update()
   {
      
      if (_heartPivot != null)
      {
         _orbitAngle += orbitSpeed * Time.deltaTime;
         float rad = _orbitAngle * Mathf.Deg2Rad;
         
         Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;

         transform.position = _heartPivot.position + offset;
         BigRevolution();
      }
      else if (isArray)
      {
         
      }
      else
      {
         transform.position += Vector3.down * fallSpeed * Time.deltaTime;

         /*if (transform.position.y < -4.6f)
            Destroy(gameObject);*/
      }
      
   }

   private void BigRevolution()
   {
      if (isWaiting)
      {
         if (Time.time - lastTriggerTime >= brTime)
         {
            isWaiting = false;
            isExpanding = true;
         }
      }
      else if (isExpanding)
      {
         orbitRadius = Mathf.MoveTowards(orbitRadius, bigRadius, expandSpeed * Time.deltaTime);

         if (orbitRadius >= bigRadius - 0.01f)
         {
            _collider.enabled = true;
            _spriteRenderer.enabled = true;
            isExpanding = false;
            isContracting = true;
         }
      }
      else if (isContracting)
      {
         orbitRadius = Mathf.MoveTowards(orbitRadius, baseRadius, expandSpeed * Time.deltaTime);

         if (orbitRadius <= baseRadius + 0.01f)
         {
            orbitRadius = baseRadius;
            isContracting = false;
            isWaiting = true;
            lastTriggerTime = Time.time;
         }
      }
   }
   
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Bullet"))
      {
         other.gameObject.SetActive(false);
         var heart = GetComponentInParent<MettatonHeart>();
         if (heart != null)
         {
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
         }
         else
         {
            Destroy(gameObject);
            
         }
      }
   }
}
