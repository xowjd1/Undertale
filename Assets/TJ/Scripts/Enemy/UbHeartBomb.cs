using System;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class UbHeartBomb : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;  
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float rotAngle = 8f;
    [SerializeField] private float ScaleChangeSpeed = 10f;

    private Vector3 moveDirection;

    private void Start()
    {
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트가 없습니다!");
            return;
        }

        Vector3 toPlayer = playerGO.transform.position - transform.position;
        toPlayer.z = 0f;
        moveDirection = toPlayer.normalized;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        var t = Time.time;

        float zRot = Mathf.Sin(t * rotateSpeed) * rotAngle;
        transform.localRotation = Quaternion.Euler(0, 0, zRot);

        float scaleT   = (Mathf.Sin(t * ScaleChangeSpeed) + 1f) * 0.5f;
        float newScale = Mathf.Lerp(0.3f, 0.4f, scaleT); 
        transform.localScale = new Vector3(newScale,newScale,newScale);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            if (other.CompareTag("Bullet"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
