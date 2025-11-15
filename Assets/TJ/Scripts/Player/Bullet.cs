using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private float  bulletSpeed = 10f;

    private void Update()
    {
        transform.position += Time.deltaTime * bulletSpeed * transform.up ;
    }
    
    private void OnEnable()
    {
        Invoke("DisableBullet", bulletLifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }*/
}
