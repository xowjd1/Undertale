using System.Collections;
using UnityEngine;

public class BattlePlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public Transform firePoint;// 총알 발사 위치
    
    [SerializeField] private BulletPooler bulletPooler;
    [SerializeField] private SpriteRenderer mettatonPlayer;
    [SerializeField] private SpriteRenderer undynePlayer;
    
    [SerializeField] private GameState gameState;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float shakeDuration  = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    [HideInInspector] public float normalMinX = -1.6f;
    [HideInInspector] public float normalMaxX = 1.6f;
    [HideInInspector] public float normalMinY = -2.9f;
    [HideInInspector] public float normalMaxY = -0.4f;
    
    private void Awake()
    {
        normalMinX = -1.6f;
        normalMaxX =  1.6f;
        normalMinY = -2.9f;
        normalMaxY = -0.4f;
    }
    private void Start()
    {
        bulletPooler = GetComponent<BulletPooler>();
        //마우스 커서 중앙에 고정, 숨김처리
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    
    private void Update()
    {
        Move();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);
        
        newPosition.x = Mathf.Clamp(newPosition.x, normalMinX, normalMaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, normalMinY, normalMaxY);
        
        transform.position = newPosition;
    }

    private void Shoot()
    {
        GameObject newInstance = bulletPooler.GetInstanceFromPool();
        newInstance.transform.position = firePoint.position;
        newInstance.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hit = false;
        if (other.CompareTag("EnemyNormal"))
        {
            gameState.playerHp -= 2;
            hit = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyHard"))
        {
            gameState.playerHp -= 4;
            hit = true;
        }
        else if (other.CompareTag("Arrow"))
        {
            gameState.playerHp -= 2;
            hit = true;
            Destroy(other.gameObject);
        }
        if (hit)
        {
            cameraShake.TriggerShake(shakeDuration, shakeMagnitude);
            StartCoroutine(FlashPlayerSprites());
            
        }
    }
    private IEnumerator FlashPlayerSprites()
    {
        var flashColor = new Color32(175, 175, 175, 255);
        
        Color origMeta = mettatonPlayer.color;
        Color origUndy = undynePlayer.color;
        
        mettatonPlayer.color = flashColor;
        undynePlayer.color   = flashColor;
        
        yield return new WaitForSeconds(0.3f);
        
        mettatonPlayer.color = origMeta;
        undynePlayer.color   = origUndy;
    }
    
}
