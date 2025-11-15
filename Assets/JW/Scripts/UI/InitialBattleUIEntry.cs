using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InitialBattleUIEntry : MonoBehaviour
{
    [SerializeField] private GameObject fightButton;
    [SerializeField] private GameObject instruction;

    private Button button;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        button = fightButton.GetComponent<Button>();
    }

    private void OnEnable()
    {
        instruction.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventSystem.current.SetSelectedGameObject(fightButton);
            button.onClick.Invoke();
            instruction.SetActive(false);
            this.enabled = false;
        }
    }
}
