using System.Collections;
using UnityEngine;

public class RisingArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spacing = 0.4f;

    private Transform[] spawners = new Transform[3];
    private int[] spawnOrder = { 3, 1, 2, 1, 2, 3, 2, 3, 1, 2, 1, 3, 1, 3 };
    private int currentIndex;

    private void Start()
    {
        CreateSpawners();
        StartCoroutine(ShootArrows());
    }

    private void CreateSpawners()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject go = new GameObject($"Spawner_{i+1}");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(i * spacing, 0f, 0f);
            spawners[i] = go.transform;
        }
    }

    private IEnumerator ShootArrows()
    {
        while (currentIndex < spawnOrder.Length)
        {
            int idx = spawnOrder[currentIndex] - 1;
            SpawnArrowAt(spawners[idx]);
            currentIndex++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnArrowAt(Transform spawner)
    {
        GameObject arrow = Instantiate(arrowPrefab, spawner.position, Quaternion.identity);

        if (arrow.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        arrow.transform.SetParent(transform, true);
    }

}