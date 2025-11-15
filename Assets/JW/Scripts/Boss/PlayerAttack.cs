using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0.7f;
    }

    private void OnEnable()
    {
        SoundUtils.PlaySound(audioClip, transform.position);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
