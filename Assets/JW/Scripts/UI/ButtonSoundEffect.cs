using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffect : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioClip audioClip;


    public void OnSelect(BaseEventData eventData)
    {
        SoundUtils.PlaySound(audioClip, transform.position);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SoundUtils.PlaySound(audioClip, transform.position);
    }
}
