using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Image fadeEffectImage;
    
    private const float addValueForImage = 3f;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        TextEffect.Fade(instructionText);
        
        if (Input.anyKeyDown)
        {
            StartCoroutine(NextScene());
        }
    }

    private IEnumerator NextScene()
    {
        while (fadeEffectImage.color.a < 1f)
        {
            fadeEffectImage.color += new Color(0f, 0f, 0f, addValueForImage * Time.deltaTime);
            yield return new WaitForSeconds(0.005f);
        }
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
