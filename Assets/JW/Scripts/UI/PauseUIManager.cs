using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private UISystemManager uiSystemManager;
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private RectTransform restartBar;
    [SerializeField] private RectTransform quitBar;

    [SerializeField] private RectTransform currentSelect;
    [SerializeField] private AudioClip selectSound;
    
    private bool isSelected = false;

    private readonly Vector3 offset = new Vector3(-300f, 0f, 0f);

    public int Index { get; set; } = 1;

    private void Awake()
    {
        SelectBar(restartBar);
    }

    private void Update()
    {
        if (Index < 1)
        {
            Index = 1;
        }

        if (Index > 2)
        {
            Index = 2;
        }

        switch (Index)
        {
            case 1:
                SelectBar(restartBar);
                if (!isSelected)
                {
                    isSelected = true;
                    SoundUtils.PlaySound(selectSound, gameObject.transform.position);
                }
                break;
            
            case 2:
                SelectBar(quitBar);
                if (!isSelected)
                {
                    isSelected = true;
                    SoundUtils.PlaySound(selectSound, gameObject.transform.position);
                }
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            isSelected = false;
            Index++;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isSelected = false;
            Index--;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (Index)
            {
                case 1:
                    SoundUtils.PlaySound(selectSound, gameObject.transform.position);
                    storyManager.StoryReset();
                    SceneManager.LoadScene(1);
                    Time.timeScale = 1f;
                    break;
                
                case 2:
                    SoundUtils.PlaySound(selectSound, gameObject.transform.position);
                    Application.Quit();
                    break;
            }
        }
    }

    private void SelectBar(RectTransform bar)
    {
        currentSelect.transform.SetParent(bar.transform);
        currentSelect.transform.localPosition = Vector3.zero;
        currentSelect.transform.localPosition += offset;
    }
}
