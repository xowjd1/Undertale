using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISystemManager : MonoBehaviour
{
    [HideInInspector] public GameObject BattleUIManager;
    [HideInInspector] public Camera BattleCamera;
    
    [SerializeField] private DialogueUIManager dialogueUIManager;
    [SerializeField] private PauseUIManager pauseUIManager;
    [SerializeField] private GameObject utilStyleUIManager;
    
    [SerializeField] private GameObject mettatonUIManager;
    [SerializeField] private GameObject undyneUIManager;
    [SerializeField] private GameObject bossUIManager;
    
    [SerializeField] private Camera mettatonBattleCamera;
    [SerializeField] private Camera undyneBattleCamera;
    [SerializeField] private Camera bossBattleCamera;
    
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject env;
    
    [SerializeField] private AudioClip playerTalkSound;
    [SerializeField] private AudioClip bossTalkSound;
    
    private DialogueData dialogueData;
    private Vector3 fightButtonPos;
    private CanvasGroup utilStyleUIGroupForBG;
    private CanvasGroup utilStyleUIGroupForThirdQuest;
    
    [HideInInspector] public bool IsPaused = false;
    [HideInInspector] public bool IsTalking = false;
    [HideInInspector] public bool IsFacing = false;
    [HideInInspector] public bool IsFighting = false;
    
    private void Awake()
    {
        var tempCanvasGroups = utilStyleUIManager.GetComponentsInChildren<CanvasGroup>();
        utilStyleUIGroupForBG = tempCanvasGroups[0];
        utilStyleUIGroupForThirdQuest = tempCanvasGroups[1];
        
        DisableDialogueUI();
        DisablePauseUI();
        
        dialogueData = new DialogueData();
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartDialogue(DialogueID.GetID("Intro"));
        }
    }
    
    private void Update()
    {
        if (BossBattleUIManager.IsClear || BossBattleUIManager.IsGameOver)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                EnablePauseUI();
                pauseUIManager.Index = 1;
                IsPaused = true;
                Time.timeScale = 0f;
            }
            else
            {
                DisablePauseUI();
                IsPaused = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void SetBattleToMettaton()
    {
        BattleUIManager = mettatonUIManager;
        BattleCamera = mettatonBattleCamera;
        
    }

    public void SetBattleToUndyne()
    {
        BattleUIManager = undyneUIManager;
        BattleCamera = undyneBattleCamera;

    }

    public void SetBattleToBoss()
    {
        BattleUIManager = bossUIManager;
        BattleCamera = bossBattleCamera;
    }

    public void EnableDialogueUI()
    {
        dialogueUIManager.gameObject.SetActive(true);
    }
    
    public void DisableDialogueUI()
    {
        dialogueUIManager.gameObject.SetActive(false);
    }
    
    public void EnablePauseUI()
    {
        pauseUIManager.gameObject.SetActive(true);
    }
    
    public void DisablePauseUI()
    {
        pauseUIManager.gameObject.SetActive(false);
    }
    
    public void EnableUtilStyleUI()
    {
        utilStyleUIManager.SetActive(true);
    }
    
    public void DisableUtilStyleUI()
    {
        utilStyleUIManager.SetActive(false);
    }

    public CanvasGroup GetFlashBackImage()
    {
        return utilStyleUIGroupForThirdQuest;
    }

    public void PhaseCloseEffect()
    {
        StartCoroutine(BGFadeInEffect(utilStyleUIGroupForBG));
    }
    
    public void PhaseOpenEffect()
    {
        StartCoroutine(BGFadeOutEffect(utilStyleUIGroupForBG));
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public void GetReadyToBattle()
    {
        player.gameObject.SetActive(false);
        env.SetActive(false);
    }

    public void StartBattle()
    {
        Debug.Log("[UISystemManager] Starting battle with UIManager = " + BattleUIManager);
        if (BattleUIManager == null)
        {
            Debug.LogError("BattleUIManager가 할당되지 않았습니다!");
            return;
        }
        IsFighting = true;
        BattleUIManager.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        BattleCamera.gameObject.SetActive(true);
    }

    public void StartDialogue(int dialogueID)
    {
        StartCoroutine(PrintDialogueNodeData(dialogueID));
    }

    public void UnlockTrueEnding()
    {
        dialogueData.SetBossDialogueForTrueEnding();
    }
    
    public IEnumerator BGFadeInEffect(CanvasGroup canvasGroup = null)
    {
        const float fadeSpeed = 4f;
        
        if (canvasGroup == null)
        {
            canvasGroup = utilStyleUIGroupForBG;
        }
        
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator BGFadeOutEffect(CanvasGroup canvasGroup = null)
    {
        const float fadeSpeed = 4f;
        
        if (canvasGroup == null)
        {
            canvasGroup = utilStyleUIGroupForBG;
        }
        
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
    private IEnumerator PrintDialogueNodeData(int dialogueID)
    {
        EnableDialogueUI();
        IsTalking = true;
        var node = dialogueData.GetNode(dialogueID);

        while (node != null)
        {
            switch (node.WhoIsThis())
            {
                case GameCharacter.Player:
                    
                    dialogueUIManager.EnablePlayerTalkUI();
                    dialogueUIManager.DisableNpcTalkUI();

                    yield return StartCoroutine(
                        PrintTextOneByOne(dialogueUIManager.playerTalkValue, node, GameCharacter.Player));
                    node = node.MoveNext();
                    
                    break;
                
                case GameCharacter.Boss:
                    
                    dialogueUIManager.DisablePlayerTalkUI();
                    dialogueUIManager.EnableNpcTalkUI();
                    dialogueUIManager.DisplayNpcImage(node.WhoIsThis(), node.GetEmotion());
                    
                    yield return StartCoroutine(
                        PrintTextOneByOne(dialogueUIManager.npcTalkValue, node, GameCharacter.Boss));
                    node = node.MoveNext();
                    
                    break;
            }
        }

        if (dialogueID == DialogueID.GetID("Boss"))
        {
            EnemyEncounter.EnterBoss();
        }
        
        DisableDialogueUI();
        IsTalking = false;
    }

    private IEnumerator PrintTextOneByOne(TextMeshProUGUI textMesh, DialogueNode node, GameCharacter character)
    {
        textMesh.text = "* ";
        int index = 0;
                    
        while (index < node.Data.Length)
        {
            textMesh.text += node.Data[index].ToString();
                            
            bool didSkip = false;
            yield return StartCoroutine(CheckIfSkipPrintText(() => didSkip = true));
                            
            if (didSkip)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("* ");
                sb.Append(node.Data);
                textMesh.text = sb.ToString();
                break;
            }
            
            if (character == GameCharacter.Boss)
            {
                SoundUtils.PlaySound(bossTalkSound, transform.position);
            }
            else
            {
                SoundUtils.PlaySound(playerTalkSound, transform.position);
            }
                            
            index++;
        }
                        
        yield return new WaitForSeconds(0.02f);
        
        if (textMesh == dialogueUIManager.playerTalkValue)
        {
            dialogueUIManager.EnablePlayerNext();
        }
        else
        {
            dialogueUIManager.EnableNpcNext();
        }
        
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) && !IsPaused);
        
        if (textMesh == dialogueUIManager.playerTalkValue)
        {
            dialogueUIManager.DisablePlayerNext();
        }
        else
        {
            dialogueUIManager.DisableNpcNext();
        }
                    
        textMesh.text = String.Empty;
    }

    private IEnumerator CheckIfSkipPrintText(Action callback)
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.001f / 20f);
                            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                callback?.Invoke();
                yield break;
            }
        }
    }
}
