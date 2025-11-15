using System.Collections;
using UnityEngine;

public class BattlePlayerControllerForBoss : MonoBehaviour
{
    [HideInInspector] public bool IsDamaged = false;
    [HideInInspector] public bool IsOnGround = false;
    [HideInInspector] public bool IsJumping = false;
    [HideInInspector] public bool IsCollision = false;
    [HideInInspector] public bool IsDead = false;
    [HideInInspector] public float CurrentGravity = gravityScale;
    
    private const float gravityScale = 13f;
    private const float maxJumpForceDelay = 0.45f;
    private const float jumpForce = 50f;
    private const float jumpForceDecrease = 3f;
    private const float moveSpeedBlue = 200f;
    private const float moveSpeedRed = 5f;
    
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    [SerializeField] private Sprite redHeart;
    [SerializeField] private Sprite blueHeart;
    
    [SerializeField] private GameObject halfHeart1;
    [SerializeField] private GameObject halfHeart2;
    
    [SerializeField] private GameObject brokenHeart1;
    [SerializeField] private GameObject brokenHeart2;
    [SerializeField] private GameObject brokenHeart3;
    [SerializeField] private GameObject brokenHeart4;

    [SerializeField] private AudioClip playerDieSound1;
    [SerializeField] private AudioClip playerDieSound2;
    
    private BottomCheck bottomCheck;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private float currentJumpForceDelay = maxJumpForceDelay;
    
    private enum HeartState
    {
        Red,
        Blue,
    }

    private enum GravityState
    {
        Up,
        Down,
        Left,
        Right,
    }

    private HeartState currentState = HeartState.Red;
    private GravityState currentGravityState = GravityState.Down;

    private void Awake()
    {
        bottomCheck = GetComponentInChildren<BottomCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (bossBattleUIManager.GetGameState().playerHp <= 0 && !IsDead)
        {
            BossBattleUIManager.IsGameOver = true;
            IsDead = true;
            bossBattleUIManager.GetBoss().gameObject.SetActive(false);
            bossBattleUIManager.gameObject.SetActive(false);
            ChangeStateToRed();
            StartCoroutine(PlayerDieAnimation());
        }

        if (IsDead)
        {
            return;
        }
        
        if (currentState == HeartState.Red)
        {
            KeyInputRed();
        }
        else
        {
            Debug.Log("11");
            if (currentGravityState == GravityState.Up)
            {
                KeyInputBlue(KeyCode.S, KeyCode.D, KeyCode.A, Vector2.down);
            }
            else if (currentGravityState == GravityState.Down)
            {
                KeyInputBlue(KeyCode.W, KeyCode.A, KeyCode.D, Vector2.up);
            }
            else if (currentGravityState == GravityState.Left)
            {
                KeyInputBlue(KeyCode.D, KeyCode.W, KeyCode.S, Vector2.right);
            }
            else
            {
                KeyInputBlue(KeyCode.A, KeyCode.W, KeyCode.S, Vector2.left);
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentState == HeartState.Red)
        {
            return;
        }
        
        if (currentGravityState == GravityState.Up)
        {
            SetGravityToVertical(-CurrentGravity);
        }
        else if (currentGravityState == GravityState.Down)
        {
            SetGravityToVertical(CurrentGravity);   
        }
        else if (currentGravityState == GravityState.Left)
        {
            SetGravityToHorizontal(CurrentGravity);
        }
        else
        {
            SetGravityToHorizontal(-CurrentGravity);
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BattlePanel"))
        {
            if (bottomCheck.GetValue())
            {
                IsOnGround = true;
                IsJumping = false;
                currentJumpForceDelay = maxJumpForceDelay;
                CurrentGravity = gravityScale;
            }

            IsCollision = true;

            if (currentGravityState == GravityState.Up || currentGravityState == GravityState.Down)
            {
                rigidBody.linearVelocityX = 0f;
            }

            if (currentGravityState == GravityState.Left || currentGravityState == GravityState.Right)
            {
                rigidBody.linearVelocityY = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BattlePanel"))
        {
            if (!bottomCheck.GetValue())
            {
                IsOnGround = false;
            }
            
            IsCollision = false;
        }
    }

    private IEnumerator PlayerDieAnimation()
    {
        yield return new WaitForSeconds(1f);
        SoundUtils.PlaySound(playerDieSound1, gameObject.transform.position);
        
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        halfHeart1.SetActive(true);
        halfHeart2.SetActive(true);
        
        yield return new WaitForSeconds(1.5f);
        SoundUtils.PlaySound(playerDieSound2, gameObject.transform.position);
        
        halfHeart1.SetActive(false);
        halfHeart2.SetActive(false);
        
        brokenHeart1.SetActive(true);
        brokenHeart2.SetActive(true);
        brokenHeart3.SetActive(true);
        brokenHeart4.SetActive(true);

        var force = jumpForce / 1.5f;
        
        brokenHeart1.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force, ForceMode2D.Impulse);
        brokenHeart2.GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.up) * force, ForceMode2D.Impulse);
        brokenHeart3.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
        brokenHeart4.GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up) * force, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(2.5f);
        
        gameOverUI.SetActive(true);
        gameObject.SetActive(false);
    }

    private void KeyInputRed()
    {
        Vector2 direction = Vector2.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            direction.Normalize();
            rigidBody.MovePosition(rigidBody.position + moveSpeedRed * Time.fixedDeltaTime * direction);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            direction.Normalize();
            rigidBody.MovePosition(rigidBody.position + moveSpeedRed * Time.fixedDeltaTime * direction);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            direction.Normalize();
            rigidBody.MovePosition(rigidBody.position + moveSpeedRed * Time.fixedDeltaTime * direction);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            direction.Normalize();
            
            rigidBody.MovePosition(rigidBody.position + moveSpeedRed * Time.fixedDeltaTime * direction);
        }
        
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            rigidBody.linearVelocityY = 0f;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            rigidBody.linearVelocityX = 0f;
        }
    }

    private void KeyInputBlue(KeyCode jump, KeyCode left, KeyCode right, Vector2 direction)
    {
        if (CurrentGravity > gravityScale)
        {
            return;
        }
        
        if (Input.GetKey(left))
        {
            if (direction.x == 0f)
            {
                rigidBody.linearVelocityX = -moveSpeedBlue * Time.fixedDeltaTime;
            }

            if (direction.y == 0f)
            {
                rigidBody.linearVelocityY = moveSpeedBlue * Time.fixedDeltaTime;
            }
        }

        if (Input.GetKey(right))
        {
            if (direction.x == 0f)
            {
                rigidBody.linearVelocityX = moveSpeedBlue * Time.fixedDeltaTime;
            }

            if (direction.y == 0f)
            {
                rigidBody.linearVelocityY = -moveSpeedBlue * Time.fixedDeltaTime;
            }
        }

        if (Input.GetKeyUp(left) || Input.GetKeyUp(right))
        {
            if (direction.x == 0f)
            {
                rigidBody.linearVelocityX = 0f;
            }
            else
            {
                rigidBody.linearVelocityY = 0f;
            }
        }
                
        if (Input.GetKeyUp(jump))
        {
            IsJumping = true;
        }

        if (IsJumping)
        {
            return;
        }
                
        if (Input.GetKey(jump))
        {
            OnJump(direction);
        }
    }

    private void OnJump(Vector2 direction)
    {
        if (currentJumpForceDelay <= 0f)
        {
            return;
        }
        
        rigidBody.AddForce(Time.fixedDeltaTime * jumpForce * direction, ForceMode2D.Impulse);
        currentJumpForceDelay -= jumpForceDecrease * Time.fixedDeltaTime;
    }

    public float GetGravityScale()
    {
        return gravityScale;
    }

    public void SetGravityToVertical(float gravity)
    {
        rigidBody.linearVelocityY -= gravity * Time.fixedDeltaTime;
    }
    
    public void SetGravityToHorizontal(float gravity)
    {
        rigidBody.linearVelocityX -= gravity * Time.fixedDeltaTime;
    }

    public void ChangeStateToBlue()
    {
        currentState = HeartState.Blue;
        spriteRenderer.sprite = blueHeart;
    }

    public void ChangeStateToRed()
    {
        currentState = HeartState.Red;
        spriteRenderer.sprite = redHeart;
        rigidBody.linearVelocityX = 0f;
        rigidBody.linearVelocityY = 0f;
    }
    
    public void GravityUp()
    {
        currentGravityState = GravityState.Up;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }

    public void GravityDown()
    {
        currentGravityState = GravityState.Down;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void GravityLeft()
    {
        currentGravityState = GravityState.Left;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public void GravityRight()
    {
        currentGravityState = GravityState.Right;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }
}
