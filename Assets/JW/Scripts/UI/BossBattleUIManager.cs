using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BossBattleUIManager : MonoBehaviour
{
    [HideInInspector] public static bool IsClear = false;
    [HideInInspector] public static bool IsGameOver = false;
    [HideInInspector] public static bool IsPlayerTurn = false;
    [HideInInspector] public AudioSource audioSource;
    
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private CameraShake cameraShake;
    
    [SerializeField] private GameState gameState;
    [SerializeField] private HitCheck hitCheck;
    [SerializeField] private InitialBattleUIEntry entry;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private GameObject selectFight;
    [SerializeField] private GameObject selectAct;
    [SerializeField] private GameObject selectItem;
    [SerializeField] private GameObject selectMercy;
    [SerializeField] private AudioClip introSound;

    [SerializeField] private BossStateMachine bossStateMachine;
    [SerializeField] private BattlePlayerControllerForBoss playerController;
    
    [SerializeField] private GameObject playerHp;
    [SerializeField] private TextMeshProUGUI hpText;
    private RectTransform hpImageAmount;

    private const float maxHpImageAmount = 350f;
    private const float maxPlayerHp = 60f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        hpImageAmount = playerHp.GetComponentInChildren<RectTransform>();
        bossStateMachine.gameObject.SetActive(true);
        playerController.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (IsClear)
        {
            if (!gameClearUI.activeSelf)
            {
                gameClearUI.SetActive(true);
                cameraShake.TriggerShake();
            }
            
            return;
        }
        
        hpImageAmount.sizeDelta = 
            new Vector2(maxHpImageAmount / maxPlayerHp * gameState.playerHp, hpImageAmount.rect.height);
        hpText.text = $"60 / {gameState.playerHp}";
    }

    public CameraShake GetCameraShake()
    {
        return cameraShake;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void PlayIntroSound()
    {
        audioSource.PlayOneShot(introSound);
    }

    public void StopIntroSound()
    {
        audioSource.Stop();
    }

    public BattlePlayerControllerForBoss GetPlayer()
    {
        return playerController;
    }

    public BossStateMachine GetBoss()
    {
        return bossStateMachine;
    }

    public void PlayerTurn()
    {
        entry.enabled = true;
        if (bossStateMachine.GetStateNum() >= 1)
        {
            instructionText.text = "* 언제까지고 피할 수는 없다. 계속 공격하자.";
            
            if (bossStateMachine.GetStateNum() == 8)
            {
                instructionText.text = "* 잠들었다! 지금이 기회다!";
            }
        }
    }

    public void BossTurn()
    {
        playerController.gameObject.SetActive(true);
    }
    
    public RectTransform InstantiateUIObject(GameObject prefab, Canvas canvas, RectTransform rect)
    {
        var temp = Instantiate(prefab, canvas.transform, false);
        var tempRect = temp.GetComponent<RectTransform>();
        
        tempRect.anchorMin = rect.anchorMin;
        tempRect.anchorMax = rect.anchorMax;
            
        tempRect.pivot = rect.pivot;
            
        tempRect.anchoredPosition = rect.anchoredPosition;

        return tempRect;
    }
    
    public IEnumerator BoneSizeDelta(RectTransform rect, float size)
    {
        var initValue = new Vector2(rect.rect.width, rect.rect.height);
        var maxValue = new Vector2(rect.rect.width, size);
        
        while (rect.rect.height < size)
        {
            initValue = Vector2.MoveTowards(initValue, maxValue,
                2000f * Time.deltaTime);
            rect.sizeDelta = initValue;
            
            yield return null;
        }
        
    }
    
    public IEnumerator PanelSizeChange(RectTransform panelRect, float xSize, float ySize, Action callback2 = null, Action callback1 = null)
    {
        callback1?.Invoke();
        
        var initialSize = panelRect.sizeDelta;
        var maxSize = new Vector2(xSize, ySize);

        while ((initialSize - maxSize).sqrMagnitude > 0.01f)
        {
            initialSize = Vector2.MoveTowards(initialSize, maxSize,
                2000f * Time.deltaTime);
            panelRect.sizeDelta = initialSize;
            
            yield return null;
        }
        
        callback2?.Invoke();
    }

    public void OnFight()
    {
        selectFight.SetActive(true);
        selectAct.SetActive(false);
        selectItem.SetActive(false);
        selectMercy.SetActive(false);
    }

    public void OnAct()
    {
        selectFight.SetActive(false);
        selectAct.SetActive(true);
        selectItem.SetActive(false);
        selectMercy.SetActive(false);
    }

    public void OnItem()
    {
        selectFight.SetActive(false);
        selectAct.SetActive(false);
        selectItem.SetActive(true);
        selectMercy.SetActive(false);
        
        var component = selectItem.GetComponentInChildren<TextMeshProUGUI>();
        component.text = $"회복 물약 X {gameState.hpPotionCount}";
    }

    public void OnMercy()
    {
        selectFight.SetActive(false);
        selectAct.SetActive(false);
        selectItem.SetActive(false);
        selectMercy.SetActive(true);
    }
}
