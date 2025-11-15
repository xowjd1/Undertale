using System.Collections;
using UnityEngine;

public class CrossArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private int maxSpawnPerSpawner = 10;
    
    private Vector3[] spawnerPositions = new Vector3[4]
    {
        new Vector3(0f,  4f, 0f),
        new Vector3(0f, -4f, 0f),
        new Vector3(-6.5f, 0f, 0f),
        new Vector3( 6.5f, 0f, 0f)
    };
    
    private void Start()
    {
        for (int i = 0; i < spawnerPositions.Length; i++)
            StartCoroutine(SpawnArrowsRepeatedly(i));
    }

    private IEnumerator SpawnArrowsRepeatedly(int index)
    {
        Vector3 spawnPos = spawnerPositions[index];
        int spawnCount = 0;

        while (spawnCount < maxSpawnPerSpawner)
        {
            SpawnArrow(spawnPos, arrowSpeed);
            spawnCount++;

            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void SpawnArrow(Vector3 position, float speed)
    {
        GameObject arrow = Instantiate(
            arrowPrefab, 
            position, 
            Quaternion.identity, 
            this.transform
        );

        Vector3 direction = (Vector3.zero - position).normalized;
        if (arrow.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = direction * speed;
        arrow.transform.up = direction;
    }
}