using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeartMover : MonoBehaviour
{
    public float moveSpeed = 300f; // 이동 속도 
    public float delayTime = 7f;   // 7초 지연 시간
    public float moveEndTime = 12f; // 12초 후 종료 시간

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    private bool _isMoving = false;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 7초가 지나고 12초가 되지 않았을 때만 이동
        if (Time.time >= delayTime && Time.time <= moveEndTime)
        {
            // 상하좌우
            _movement.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // 대각선 이동 속도
            if (_movement.sqrMagnitude > 1)
            {
                _movement.Normalize();
            }
        }
        else
        {
            // 7초가 지나기 전에는 움직임 없음
            _movement = Vector2.zero;

            if (Time.time > moveEndTime && !_isMoving)
            {
                gameObject.SetActive(false);
                _isMoving = true;
            }
                
            

        }
    }

    private void FixedUpdate()
    {
        // 물리 기반 이동 (PlayerController와 동일)
        _rigidbody2D.MovePosition(_rigidbody2D.position + moveSpeed * Time.fixedDeltaTime * _movement);
    }
}