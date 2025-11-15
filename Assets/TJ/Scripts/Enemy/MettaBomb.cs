using System;
using System.Collections;
using UnityEngine;

public class MettaBomb : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private GameObject crossExplosion;
    [SerializeField] private BattlePlayerController playerController;
    private bool _hasSpawned = false;
    private bool isArray = false;

    private Collider2D collider2D;

    private void Awake()
    {
        var array = GetComponentInParent<BlockArray>();
        if (array != null)
            isArray = true;
        
        collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        /*if (transform.position.y < -10f)
            Destroy(gameObject);*/
        if (isArray)
        {
            
        }
        else
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Bullet") && !_hasSpawned))
        {
            _hasSpawned = true;
            collider2D.enabled = false;
            if (other.CompareTag("Bullet"))
                other.gameObject.SetActive(false);
            
            Destroy(gameObject, 0.1f);
            
            StartCoroutine(SpawnCrossExplosion());
        }
    }

    private IEnumerator SpawnCrossExplosion()
    {
        yield return new WaitForSeconds(0.083f);
        Instantiate(crossExplosion, transform.position, Quaternion.identity);
    }
}