using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageButton : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private GameObject thisDetailsUI;
    [SerializeField] private GameObject otherDetailsUI1;
    [SerializeField] private GameObject ohterDetailsUI2;
    [SerializeField] private GameObject otherDetailsUI3;

    private void Awake()
    {
        thisDetailsUI.SetActive(false);
        otherDetailsUI1.SetActive(false);
        ohterDetailsUI2.SetActive(false);
        otherDetailsUI3.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        targetImage.sprite = highlightedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetImage.sprite = normalSprite;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("버튼 클릭!");
        thisDetailsUI.SetActive(true);
        otherDetailsUI1.SetActive(false);
        ohterDetailsUI2.SetActive(false);
        otherDetailsUI3.SetActive(false);
    }
}