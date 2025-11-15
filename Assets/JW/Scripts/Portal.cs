using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] UISystemManager uiSystemManager;
    [SerializeField] private GameState gameState;
    [SerializeField] private GameState saveData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveToNextScene());
        }
    }

    private void DataSave()
    {
        saveData.killCount = gameState.killCount;
    }

    private IEnumerator MoveToNextScene()
    {
        yield return StartCoroutine(uiSystemManager.BGFadeInEffect());
        DataSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
