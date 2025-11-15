using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.onClick.Invoke();
            gameObject.SetActive(false);
        }
    }
}
