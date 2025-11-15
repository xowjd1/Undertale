using System;
using System.Collections;
using UnityEngine;

public class UndyneArrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private Transform playerTransform;
    

    private Vector3 moveDirection;
    private bool isMoving = false;
    
    private Collider2D _collider2D;
    


    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(RotateThenShoot());
        
    }


    private void Update()
    {
        if (isMoving)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        if (transform.position.y < -7 || transform.position.y > 7
                                      || transform.position.x < -11 || transform.position.x > 11)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator RotateThenShoot()
    {
        float timer = 0f;

        while (timer < 1.5f)
        {
            transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        var playerGO = GameObject.FindWithTag("Player");
        
        Vector3 toPlayer = playerGO.transform.position - transform.position;
        toPlayer.z = 0f;
        moveDirection = toPlayer.normalized;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        
        isMoving = true;
    }
    
}