using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextButton : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject highlight;
    
    private void Awake()
    {
        if (highlight != null)
            highlight.SetActive(false);
        
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(true);
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //gameObject.SetActive(false);
    }
}