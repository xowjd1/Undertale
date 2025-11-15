using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerTalk;
    [SerializeField] private GameObject npcTalk;
    [SerializeField] private Image displayNpcImage;
    
    [SerializeField] private List<DictionaryEntry> SansSprites;
    private NpcImage npcImage;
    
    public TextMeshProUGUI playerTalkValue { get; private set; }
    public TextMeshProUGUI playerNext { get; private set; }
    public TextMeshProUGUI npcTalkValue { get; private set; }
    public TextMeshProUGUI npcNext { get; private set; }

    private void Awake()
    {
        var playerComponents = playerTalk.GetComponentsInChildren<TextMeshProUGUI>();
        playerTalkValue = playerComponents[0];
        playerNext = playerComponents[1];
        
        var npcComponents = npcTalk.GetComponentsInChildren<TextMeshProUGUI>();
        npcTalkValue = npcComponents[0];
        npcNext = npcComponents[1];
        
        npcImage = new NpcImage();
        foreach (var entry in SansSprites)
        {
            npcImage.SetSprites(GameCharacter.Boss, entry.emotion, entry.sprite);
        }
        
        DisablePlayerTalkUI();
        DisablePlayerNext();
        DisableNpcTalkUI();
        DisableNpcNext();
    }
    
    private void Update()
    {
        if (playerNext.isActiveAndEnabled)
        {
            TextEffect.Fade(playerNext);
        }

        if (npcNext.isActiveAndEnabled)
        {
            TextEffect.Fade(npcNext);
        }
    }

    public void EnablePlayerTalkUI()
    {
        playerTalk.SetActive(true);
    }

    public void DisablePlayerTalkUI()
    {
        playerTalk.SetActive(false);
    }

    public void EnableNpcTalkUI()
    {
        npcTalk.SetActive(true);
    }

    public void DisableNpcTalkUI()
    {
        npcTalk.SetActive(false);
    }

    public void EnablePlayerNext()
    {
        playerNext.gameObject.SetActive(true);
    }
    
    public void DisablePlayerNext()
    {
        playerNext.gameObject.SetActive(false);
        TextEffect.InitFade(playerNext);
    }

    public void DisplayNpcImage(GameCharacter character, CharacterEmotion emotion)
    {
        displayNpcImage.sprite = npcImage.GetSprite(character, emotion);
    }

    public void EnableNpcNext()
    {
        npcNext.gameObject.SetActive(true);
    }

    public void DisableNpcNext()
    {
        npcNext.gameObject.SetActive(false);
        TextEffect.InitFade(npcNext);
    }
}
