using UnityEngine;

public class TextDisplayButton : MonoBehaviour
{
    [SerializeField] private GameObject text;

    public void OnDisplayText()
    {
        text.SetActive(true);
        gameObject.SetActive(false);
    }
}
