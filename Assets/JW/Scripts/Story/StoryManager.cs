using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private UISystemManager uiSystemManager;
    [SerializeField] private GameState gameState;
    [SerializeField] private GameState saveData;

    private readonly UnityEvent<UISystemManager> storyEvent = new UnityEvent<UISystemManager>();

    private void Awake()
    {
        StoryUpdate();
    }
    
    private void Update()
    {
        storyEvent.Invoke(uiSystemManager);
    }

    public void StoryRetry()
    {
        gameState.hpPotionCount = saveData.hpPotionCount;
        gameState.playerHp = saveData.playerHp;
        gameState.killCount = saveData.killCount;
    }

    public void StoryReset()
    {
        gameState.hpPotionCount = 0;
        gameState.playerHp = 20;
        gameState.killCount = 0;

        saveData.hpPotionCount = 0;
        saveData.playerHp = 20;
        saveData.killCount = 0;
    }

    private void StoryUpdate()
    {
        if (saveData.killCount == 4)
        {
            uiSystemManager.UnlockTrueEnding();
        }
        else if (saveData.killCount >= 2)
        {
            storyEvent.AddListener(StoryLineThird);
        }
        else if (saveData.killCount == 1)
        {
            storyEvent.AddListener(StoryLineThird);
            storyEvent.AddListener(StoryLineSecond);
        }
        else
        {
            storyEvent.AddListener(StoryLineThird);
            storyEvent.AddListener(StoryLineSecond);
            storyEvent.AddListener(StoryLineFirst);
        }
    }

    private void StoryLineFirst(UISystemManager manager)
    {
        if (gameState.killCount == 1)
        {
            manager.StartDialogue(DialogueID.GetID("First"));
            saveData.playerHp = 300;
            saveData.hpPotionCount += 1;
            storyEvent.RemoveListener(StoryLineFirst);
        }
    }
    
    private void StoryLineSecond(UISystemManager manager)
    {
        if (gameState.killCount == 2)
        {
            manager.StartDialogue(DialogueID.GetID("Second"));
            saveData.playerHp = 300;
            saveData.hpPotionCount += 1;
            storyEvent.RemoveListener(StoryLineSecond);
        }
    }
    
    private void StoryLineThird(UISystemManager manager)
    {
        if (gameState.killCount == 4)
        {
            StartCoroutine(FlashBack(manager.GetFlashBackImage()));
            manager.UnlockTrueEnding();
            saveData.playerHp = 300;
            saveData.hpPotionCount = 3;
            storyEvent.RemoveListener(StoryLineThird);
        }
    }

    private IEnumerator FlashBack(CanvasGroup canvasGroup)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(uiSystemManager.BGFadeInEffect(canvasGroup));
            yield return StartCoroutine(uiSystemManager.BGFadeOutEffect(canvasGroup));
        }
        
        uiSystemManager.StartDialogue(DialogueID.GetID("Third"));
    }
}
