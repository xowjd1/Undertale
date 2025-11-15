using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType { Mettaton, Undyne }
public class EnemyStateMachine : MonoBehaviour
{
    private EnemyState currentState;
    [SerializeField] private BattlePlayerController player;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject mettatonPlayer;
    [SerializeField] private GameObject undynePlayer;
    [SerializeField] private GameState gameState;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject battleCamera;
    [SerializeField] private GameObject env;
    [SerializeField] private GameObject fieldPlayer;
    
    [Header("메타톤")]
    [SerializeField] private GameObject mettablock;
    [SerializeField] private GameObject mettabomb;
    [SerializeField] private GameObject umbrellabomb;
    [SerializeField] private GameObject mettaHeart;
    [SerializeField] private GameObject blockArray;
    [SerializeField] private GameObject mettaton;
    [SerializeField] private TextMeshProUGUI mettatonText;
    [SerializeField] private GameObject RECUI;
    [SerializeField] private GameObject REWUI;
    [SerializeField] private GameObject mettatonUI;

    [Header("언다인")]
    [SerializeField] private GameObject undyne;
    [SerializeField] private GameObject dodgeArrow;
    [SerializeField] private GameObject trackingArrow;
    [SerializeField] private GameObject crossArrowSpawner;
    [SerializeField] private GameObject heptagonSpawner;
    [SerializeField] private GameObject risingArrowSpawner;
    [SerializeField] private GameObject arrowPlayer;
    [SerializeField] private UndyneHP undyneHP;
    [SerializeField] private GameObject playerAttackBar;
    [SerializeField] private GameObject undyneUI;
    
    
    
    [Header("UI")]
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private GameObject playerTextUI;
    [SerializeField] private GameObject playerTurnUI;
    [SerializeField] private GameObject playerHPSlider;
    [SerializeField] private GameObject uiBox;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private UndyneHpBar hpBar;
    
    
    [SerializeField] private UISystemManager uISystemManager;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mettatonClip;
    [SerializeField] private AudioClip undyneClip;
    
    private List<Func<EnemyState>> _mettatonStateSequence;
    private List<Func<EnemyState>> _undyneStateSequence;
    private int _mNextIndex = 0;
    private int _uNextIndex = 0;

    private void Awake()
    {

            _mettatonStateSequence = new List<Func<EnemyState>>()
            {
                () => CreateIntroState(),
                () => CreatePlayerTurnState(),
                () => CreateRainAttackState(),
                () => CreatePlayerTurnState(),
                () => CreateUmbrellaState(),
                () => CreatePlayerTurnState(),
                () => CreateLightingState(),
                () => CreatePlayerTurnState(),
                () => CreateRecAttackState(),
                () => CreateWinState()
            };

            _undyneStateSequence = new List<Func<EnemyState>>()
            {
                () => CreateUndyneIntroState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateDodgeArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateArrowRisingState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateRhythmArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateCircleArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateDodgeArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateArrowRisingState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateRhythmArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateCircleArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateDodgeArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateArrowRisingState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateRhythmArrowState(),
                () => CreatePlayerTurnUndyneState(),
                () => CreateCircleArrowState()
                
            };
    }

    private void Start()
    {
        mettatonPlayer.SetActive(false);
        undynePlayer.SetActive(false);
        switch (enemyType)
        {
            case EnemyType.Mettaton:
                mettatonPlayer.SetActive(true);
                audioSource.clip = mettatonClip;
                audioSource.Play();
                mettatonAdvanceState();
                break;
            case EnemyType.Undyne:
                undynePlayer.SetActive(true);
                audioSource.clip = undyneClip;
                audioSource.Play();
                undyneAdvanceState();
                break;
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter();
        }
    }
     
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
    
    public void mettatonAdvanceState()
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        
        if (_mNextIndex < _mettatonStateSequence.Count)
        {
            currentState = _mettatonStateSequence[_mNextIndex++]();
        }
        else
        {
            currentState = new PlayerWinState(this, uISystemManager,gameState,player,mettaton,mettatonUI,mainCamera,battleCamera,env,fieldPlayer);
        }

        currentState.Enter();
    }
    
    public void undyneAdvanceState()
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        
        if (_uNextIndex < _undyneStateSequence.Count)
        {
            currentState = _undyneStateSequence[_uNextIndex++]();
        }
        else
        {
            currentState = new PlayerWinUndyneState(this,uISystemManager,gameState,player,undyne,
                undyneUI,mainCamera,battleCamera,env,fieldPlayer);
        }

        currentState.Enter();
    }

    public void EnemyStateUI()
    {
        var fadeColor = new Color32(255, 255, 255, 15);
        var sliderBgColor = new Color32(255, 0, 0, 30);
        var sliderFillColor = new Color32(255, 255, 0, 30);

        foreach (var img in buttonUI.GetComponentsInChildren<Image>())
        {
            img.color = fadeColor;
        }

        foreach (var tmp in playerTextUI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.color = fadeColor;
        }

        foreach (var spr in mettaton.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = fadeColor;
        }

        //playerTurnUI.SetActive(false);

        var slider = playerHPSlider.GetComponent<Slider>();
        if (slider != null)
        {
            var bgImg = slider.GetComponentInChildren<Image>();
            if (bgImg != null)
            {
                bgImg.color = sliderBgColor;
            }

            if (slider.fillRect != null)
            {
                var fillImg = slider.fillRect.GetComponent<Image>();
                if(fillImg != null)
                    fillImg.color = sliderFillColor;
            }
        }

        var rt = uiBox.GetComponent<RectTransform>();
        if (currentState is MettatonRECAttackState)
            StartCoroutine(ResizingUIBox(rt, 425f, 1.0f));
        else 
            StartCoroutine(ResizingUIBox(rt, 525f, 1.0f));

    }
    public void UnDyneEnemyStateUI()
    {
        var fadeColor = new Color32(255, 255, 255, 15);
        var sliderBgColor = new Color32(255, 0, 0, 30);
        var sliderFillColor = new Color32(255, 255, 0, 30);

        foreach (var img in buttonUI.GetComponentsInChildren<Image>())
        {
            img.color = fadeColor;
        }

        foreach (var tmp in playerTextUI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.color = fadeColor;
        }

        foreach (var spr in undyne.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = fadeColor;
        }
        
        var slider = playerHPSlider.GetComponent<Slider>();
        if (slider != null)
        {
            var bgImg = slider.GetComponentInChildren<Image>();
            if (bgImg != null)
            {
                bgImg.color = sliderBgColor;
            }

            if (slider.fillRect != null)
            {
                var fillImg = slider.fillRect.GetComponent<Image>();
                if(fillImg != null)
                    fillImg.color = sliderFillColor;
            }
        }

        var rt = uiBox.GetComponent<RectTransform>();
        if (currentState is DodgeArrowState)
            StartCoroutine(UndyneResizingUIBox(rt, 550f, 680f,0f,-40f,1f));
        else if ( currentState is ArrowRisingState)
            StartCoroutine(UndyneResizingUIBox(rt, 220f, 315f,-10f,-225f,1f));
        else if ( currentState is RhythmArrowState)
            StartCoroutine(UndyneResizingUIBox(rt, 250f, 300f,0f,140f,1f));
        else
            StartCoroutine(UndyneResizingUIBox(rt, 1720f, 1020f,0f,130f,1f));

    }

    public void PlayerStateUI()
    {
        var fadeColor = new Color32(255, 255, 255, 255);
        var sliderBgColor = new Color32(255, 0, 0, 255);
        var sliderFillColor = new Color32(255, 255, 0, 255);

        foreach (var img in buttonUI.GetComponentsInChildren<Image>())
        {
            img.color = fadeColor;
        }

        foreach (var tmp in playerTextUI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.color = fadeColor;
        }

         foreach (var spr in mettaton.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = fadeColor;
        }

        //playerTurnUI.SetActive(false);

        var slider = playerHPSlider.GetComponent<Slider>();
        if (slider != null)
        {
            var bgImg = slider.GetComponentInChildren<Image>();
            if (bgImg != null)
            {
                bgImg.color = sliderBgColor;
            }

            if (slider.fillRect != null)
            {
                var fillImg = slider.fillRect.GetComponent<Image>();
                if(fillImg != null)
                    fillImg.color = sliderFillColor;
            }
        }

        var rt = uiBox.GetComponent<RectTransform>();
        StartCoroutine(ResizingUIBox(rt, 1800, 1.0f));

    }
    public void UndynePlayerStateUI()
    {
        var fadeColor = new Color32(255, 255, 255, 255);
        var sliderBgColor = new Color32(255, 0, 0, 255);
        var sliderFillColor = new Color32(255, 255, 0, 255);

        foreach (var img in buttonUI.GetComponentsInChildren<Image>())
        {
            img.color = fadeColor;
        }

        foreach (var tmp in playerTextUI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.color = fadeColor;
        }

        foreach (var spr in undyne.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = fadeColor;
        }

        //playerTurnUI.SetActive(false);

        var slider = playerHPSlider.GetComponent<Slider>();
        if (slider != null)
        {
            var bgImg = slider.GetComponentInChildren<Image>();
            if (bgImg != null)
            {
                bgImg.color = sliderBgColor;
            }

            if (slider.fillRect != null)
            {
                var fillImg = slider.fillRect.GetComponent<Image>();
                if(fillImg != null)
                    fillImg.color = sliderFillColor;
            }
        }

        var rt = uiBox.GetComponent<RectTransform>();
        StartCoroutine(UndyneResizingUIBox(rt, 1800, 490,0,-120,1f));

    }

    private IEnumerator ResizingUIBox(RectTransform rt, float width, float duration)
    {
        float elapsed   = 0f;
        float fromWidth = rt.rect.width;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t    = Mathf.Clamp01(elapsed / duration);
            float w    = Mathf.Lerp(fromWidth, width, t);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            yield return null;
        }
        
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
    private IEnumerator UndyneResizingUIBox(RectTransform rt, float targetWidth,
        float targetHeight, float targetPosX, float targetPosY, float duration)
    {
        float elapsed = 0f;
        float fromWidth = rt.rect.width;
        float fromHeight = rt.rect.height;
        float fromPosX = rt.anchoredPosition.x;
        float fromPosY = rt.anchoredPosition.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            float w = Mathf.Lerp(fromWidth, targetWidth, t);
            float h  = Mathf.Lerp(fromHeight, targetHeight, t);
            float posX = Mathf.Lerp(fromPosX, targetPosX, t);
            float posY = Mathf.Lerp(fromPosY, targetPosY, t);
            
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            Vector2 ap = rt.anchoredPosition;
            ap.x = posX;
            ap.y = posY;
            rt.anchoredPosition = ap;

            yield return null;
        }

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        Vector2 finalPos = rt.anchoredPosition;
        finalPos.y = targetPosY;
        rt.anchoredPosition = finalPos;
    }

    
    #region 메타톤
    public EnemyState CreateIntroState()
    {
        return new MettatonIntroState(
            this,
            player,
            mettatonText,
            uISystemManager,
            buttonUI,
            playerTextUI,
            uiBox,
            playerTurnUI
            
        );
    }
  
    public EnemyState CreatePlayerTurnState()
    {
        return new PlayerTurnState(
            this,
            player,
            firstButton,
            playButton,
            gameState
            
            
        );
    }
    
    public EnemyState CreateRainAttackState()
    {
        return new MettatonRainAttackState(
            this,
            mettablock,
            mettabomb,
            player,
            gameState
        );
    }

    public EnemyState CreateUmbrellaState()
    {
        return new MettatonUmbAttackState(
            this,
            mettabomb,
            umbrellabomb,
            player,
            gameState
        );
    }

    public EnemyState CreateLightingState()
    {
        return new MettatonLightingAttackState(
            this,
            mettaHeart,
            player,
            gameState
        );
    }

    public EnemyState CreateRecAttackState()
    {
        return new MettatonRECAttackState(
            this,
            blockArray,
            player,
            RECUI,
            REWUI,
            gameState

        );
    }

    public EnemyState CreateWinState()
    {
        return new PlayerWinState(
            this,
            uISystemManager,
            gameState,
            player,
            mettaton,
            mettatonUI,
            mainCamera,
            battleCamera,
            env,
            fieldPlayer
            
        );
    }
    public EnemyState CreatePlayerLoseState()
    {
        return new PlayerLoseState(
            this,
            loseUI,
            player
        );
    }
    
    # endregion

    public EnemyState CreateUndyneIntroState()
    {
        return new UndyneIntroState(
            this,
            player,
            buttonUI,
            playerTextUI,
            playerHPSlider,
            uiBox,
            playerTurnUI
            
        );
    }

    public EnemyState CreateDodgeArrowState()
    {
        return new DodgeArrowState(
            this,
            player,
            dodgeArrow,
            gameState
        );
    }

    public EnemyState CreateArrowRisingState()
    {
        return new ArrowRisingState(
            this,
            player,
            risingArrowSpawner,
            gameState
        );
    }

    public EnemyState CreateRhythmArrowState()
    {
        return new RhythmArrowState(
            this,
            player,
            crossArrowSpawner,
            arrowPlayer,
            gameState
        );
    }

    public EnemyState CreateCircleArrowState()
    {
        return new CircleArrowState(
            this,
            player,
            heptagonSpawner,
            gameState
        );
    }

    public EnemyState CreatePlayerTurnUndyneState()
    {
        return new PlayerTurnUndyneState(
            this,
            undyne,
            undyneHP,
            player,
            playerAttackBar,
            firstButton,
            playButton,
            gameState
        );
    }

    public EnemyState CreatePlayerWinUndyneState()
    {
        return new PlayerWinUndyneState(
            this,
            uISystemManager,
            gameState,
            player,
            undyne,
            undyneUI,
            mainCamera,
            battleCamera,
            env,
            fieldPlayer
        );
    }

    public EnemyState CreatePlayerLoseUndyneState()
    {
        return new PlayerLoseUndyneState(
            this,
            loseUI
        );
    }
    
}