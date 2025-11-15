using UnityEngine;

public class HeptagonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float radius = 5f;
    private Transform centerPoint;  
    private const int vertexCount = 7;

    private void Awake()
    {
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogError("HeptagonSpawner: Player 태그가 없습니다!");
            enabled = false;
            return;
        }
        centerPoint = playerGO.transform;
    }
    void Start()
    {
        Vector3 centerPos = centerPoint.position;
        for (int i = 0; i < vertexCount; i++)
        {

            float angleDeg = i * 360f / vertexCount;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);
            Vector3 spawnPos = centerPos + dir * radius;
            
            GameObject obj = Instantiate(objectPrefab, spawnPos, Quaternion.identity, transform);
            
            Vector3 toCenter = (centerPos - spawnPos).normalized;
            obj.transform.up = toCenter;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centerPoint.position, radius);
        }
    }
}
