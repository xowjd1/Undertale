using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UISystemManager uiSystemManager;
    
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private GameState gameState;
    
    [SerializeField] private CanvasGroup bgCanvasGroup;
    
    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject restart;
    [SerializeField] private GameObject quit;
    
    private void Awake()
    {
        StartCoroutine(Over());
    }

    private IEnumerator Over()
    {
        yield return StartCoroutine(uiSystemManager.BGFadeInEffect(bgCanvasGroup));
        
        yield return new WaitForSeconds(0.5f);
        
        retry.SetActive(true);
        restart.SetActive(true);
        quit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restart);
    }

    public void OnRetry()
    {
        storyManager.StoryRetry();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
