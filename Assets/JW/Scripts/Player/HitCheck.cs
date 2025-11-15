using System.Collections;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private AudioClip audioClip;

    private const int damage = 1;
    private const float coolTime = 0.005f;
    private bool canHit = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BoneAndBlaster"))
        {
            if (!canHit)
            {
                return;
            }
            
            gameState.playerHp -= damage;
            canHit = false;
            SoundUtils.PlaySound(audioClip, transform.position, 0.4f);
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolTime);
        canHit = true;
    }
}
