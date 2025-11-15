using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    [SerializeField] private UISystemManager uiSystemManager;
    
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private GameState gameState;
    
    [SerializeField] private CanvasGroup bgCanvasGroup;
    
    [SerializeField] private CanvasGroup normalCutSceneCanvasGroup;
    [SerializeField] private GameObject normalRestart;
    [SerializeField] private GameObject normalQuit;
    
    [SerializeField] private CanvasGroup trueCutSceneCanvasGroup;
    [SerializeField] private GameObject trueRestart;
    [SerializeField] private GameObject trueQuit;

    private void Awake()
    {
        if (gameState.killCount == 4)
        {
            StartCoroutine(Ending(trueCutSceneCanvasGroup, trueRestart, trueQuit));
        }
        else
        {
            StartCoroutine(Ending(normalCutSceneCanvasGroup, normalRestart, normalQuit));
        }
    }

    private IEnumerator Ending(CanvasGroup canvasGroup, GameObject restart, GameObject quit)
    {
        yield return StartCoroutine(uiSystemManager.BGFadeInEffect(bgCanvasGroup));
        
        yield return new WaitForSeconds(0.5f);
        
        Time.timeScale = 1f;
        yield return StartCoroutine(uiSystemManager.BGFadeInEffect(canvasGroup));
        restart.SetActive(true);
        quit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restart);
    }
    
    public void OnRestart()
    {
        storyManager.StoryReset();
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
