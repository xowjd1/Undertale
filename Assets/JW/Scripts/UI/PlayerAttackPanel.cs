using UnityEngine;

public class PlayerAttackPanel : MonoBehaviour
{
    [SerializeField] private GameObject playerAttack;
    [SerializeField] private BossStateMachine bossStateMachine;

    private PlayerAttackCursor cursor;
    private Animator cursorAnimator;
    [SerializeField] private bool isAttacked;

    private void Awake()
    {
        isAttacked = false;
        cursorAnimator = GetComponentInChildren<Animator>();
        cursorAnimator.speed = 0.7f;
        cursor = GetComponentInChildren<PlayerAttackCursor>();
    }

    private void OnEnable()
    {
        isAttacked = false;
        cursorAnimator.speed = 0.7f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacked)
        {
            Attack();
        }

        if (cursor.GetPosX() >= 1469.9f && !isAttacked)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacked = true;
        cursorAnimator.speed = 0f;
        playerAttack.SetActive(true);
        bossStateMachine.IsAttacked = true;
    }
}
