using UnityEngine;
public class UmbrellaBomb : MonoBehaviour
{
    [Header("낙하 설정")]
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float slowFallSpeed = 2f;
    [SerializeField] private float slowStartY = -1f;
    [SerializeField] private float shiftThresholdY = -1.5f;

    [Header("좌우 이동 설정")]
    [SerializeField] private float maxSideMoveSpeed = 8f;
    [SerializeField] private float sideAcceleration = 5f;

    private float currentSideSpeed = 0f;
    private Vector3 sideDirection;
    [SerializeField] private GameObject heartBomb;
    [SerializeField] private BattlePlayerController playerController;

    private enum BombState { Falling, SlowingDown, Sideways }
    private BombState currentState = BombState.Falling;

    private void Update()
    {
        switch (currentState)
        {
            case BombState.Falling:
                Fall();
                break;
            case BombState.SlowingDown:
                SlowFall();
                break;
            case BombState.Sideways:
                SideMove();
                break;
        }
        if (transform.position.y < -7 || transform.position.y > 7
                                      || transform.position.x < -11 || transform.position.x > 11)
        {
            Destroy(gameObject);
        }
    }

    private void Fall()
    {
        transform.position += fallSpeed * Time.deltaTime * Vector3.down;

        if (transform.position.y < slowStartY)
        {
            currentState = BombState.SlowingDown;
        }
    }

    private void SlowFall()
    {
        transform.position += slowFallSpeed * Time.deltaTime * Vector3.down;

        if (transform.position.y <= shiftThresholdY)
        {
            currentState = BombState.Sideways;
            sideDirection = (transform.position.x < 0) ? Vector3.left + Vector3.up : Vector3.right + Vector3.up;
            currentSideSpeed = 0f;
            
            Instantiate(heartBomb, transform.position, Quaternion.identity);
        }
    }

    private void SideMove()
    {
        currentSideSpeed += sideAcceleration * Time.deltaTime;
        currentSideSpeed = Mathf.Min(currentSideSpeed, maxSideMoveSpeed);

        transform.position += currentSideSpeed * Time.deltaTime * sideDirection;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            if (other.CompareTag("Bullet"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
