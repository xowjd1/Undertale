using UnityEngine;

public class GasterBlast : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnSummon()
    {
        SoundUtils.PlaySound(audioClip, transform.position);
    }

    public void Shoot()
    {
        animator.SetBool("IsShooting", true);
    }
    
    public void DestroyObject()
    {
        animator.speed = 0f;
        Destroy(gameObject);
    }
}
