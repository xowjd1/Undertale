using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector] public bool IsAttacked = false;
    [HideInInspector] public bool CanBeAttacked = false;
    
    private const float offset = 1.8f;
    
    [SerializeField] private Image panel;
    
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite eyesClosed;
    [SerializeField] private Sprite eyesBlack;
    [SerializeField] private Sprite blueEyes;
    [SerializeField] private Sprite yellowEyes;
    [SerializeField] private Sprite dieHead;
    [SerializeField] private Sprite dieTorso;
    [SerializeField] private Sprite dieLeg;
    [SerializeField] private AudioClip dieAudio;
    
    private List<IBossState> states;
    
    private SpriteRenderer currentHeadSprite;
    private SpriteRenderer currentTorsoSprite;
    private SpriteRenderer currentLegSprite;
    
    private Animator animator;

    private int state = 0;

    private void Awake()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        currentTorsoSprite = spriteRenderers[0];
        currentHeadSprite = spriteRenderers[1];
        currentLegSprite = spriteRenderers[2];
        
        animator = GetComponent<Animator>();
        animator.speed = 0f;
        
        states = new List<IBossState>();
        states = GetComponents<IBossState>().ToList();
    }
    
    private void Update()
    {
        // For Showing Mode
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(DieAnimation());
        }
        
        float yPos = panel.rectTransform.rect.height / 100f - offset;
        gameObject.transform.position = 
            new Vector3(gameObject.transform.position.x, yPos, gameObject.transform.position.z);

        if (!CanBeAttacked && IsAttacked)
        {
            IsAttacked = false;
            StartCoroutine(DodgeAnimation());
            state++;
        }
        else if (CanBeAttacked && IsAttacked)
        {
            IsAttacked = false;
            StartCoroutine(DieAnimation());
        }

        if (state == 8 && BossBattleUIManager.IsPlayerTurn)
        {
            CanBeAttacked = true;
            animator.speed = 0.2f;
            currentHeadSprite.sprite = eyesClosed;
        }
    }

    private IEnumerator DodgeAnimation()
    {
        BossBattleUIManager.IsPlayerTurn = false;
        
        const float distance = -1.5f;
        const float moveSpeed = 5f;
        
        while (gameObject.transform.position.x > distance)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                new Vector3(distance, gameObject.transform.position.y, gameObject.transform.position.z),
                Time.deltaTime * moveSpeed);
            
            yield return null;
        }
        
        yield return new WaitForSeconds(0.7f);
        
        while (gameObject.transform.position.x < 0f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                new Vector3(0f, gameObject.transform.position.y, gameObject.transform.position.z),
                Time.deltaTime * moveSpeed);
            
            yield return null;
        }
        
        panel.GetComponentInChildren<PlayerAttackPanel>().gameObject.SetActive(false);

        switch (state)
        {
            case 1:
                states[1].Enter();
                break;
            
            case 2:
                states[2].Enter();
                break;
            
            case 3:
                states[3].Enter();
                break;
            
            case 4:
                states[4].Enter();
                break;
            
            default:
                int temp = Random.Range(1, 5);
                states[temp].Enter();
                break;
        }
    }
    
    private IEnumerator DieAnimation()
    {
        currentHeadSprite.sprite = dieHead;
        AnimatorControl(0f);
        
        yield return new WaitForSeconds(0.7f);
        panel.GetComponentInChildren<PlayerAttackPanel>().gameObject.SetActive(false);
            
        yield return new WaitForSeconds(2f);
        
        SoundUtils.PlaySound(dieAudio, transform.position);
        currentTorsoSprite.sprite = dieTorso;
        currentLegSprite.sprite = dieLeg;

        BossBattleUIManager.IsClear = true;
        Time.timeScale = 0.2f;
    }

    public int GetStateNum()
    {
        return state;
    }

    public void AnimatorControl(float speed)
    {
        animator.speed = speed;
    }

    public void ChangeHeadNormal()
    {
        currentHeadSprite.sprite = normal;
    }
    
    public void ChangeEyesBlue()
    {
        currentHeadSprite.sprite = blueEyes;
    }

    public void ChangeEyesYellow()
    {
        currentHeadSprite.sprite = yellowEyes;
    }

    public void ChangeEyesBlack()
    {
        currentHeadSprite.sprite = eyesBlack;
    }
}
