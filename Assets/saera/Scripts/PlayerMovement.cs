using UnityEngine;
using UnityEngine.Tilemaps;
 
[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
 
    public Tilemap tilemap;
     
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
 
    private readonly int HashX = Animator.StringToHash("x");
    private readonly int HashY = Animator.StringToHash("y");
    private readonly int HashIsMoving = Animator.StringToHash("IsMoving");
 
    private Vector2 _movement;
     
    private Vector3Int _cellPos;
    public TileBase _currentTile;
     
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
 
    public void Update()
    {
        _movement.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
 
 
        if (_movement.sqrMagnitude > 1)
        {
            _movement.Normalize();
        }
         
        _cellPos = tilemap.WorldToCell(transform.position);
        _currentTile = tilemap.GetTile(_cellPos);
         
        bool isMoving = _movement.sqrMagnitude > 0f;
        _animator.SetBool(HashIsMoving, isMoving);
         
        if (isMoving)
        {
            _animator.SetFloat(HashX, _movement.x);
            _animator.SetFloat(HashY, _movement.y);
        }
         
    }
 
    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + moveSpeed * Time.fixedDeltaTime * _movement);
    }
}