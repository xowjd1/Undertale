using UnityEngine;

public class HeptagonArrow : MonoBehaviour
{
    private Transform centerPoint;
    public float angularSpeed = 90f;
    public float gatherDuration = 3f;

    private float angleRad;
    private float radius;
    private float gatherSpeed;
    private Vector3 gatherCenter;

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
        gatherCenter = centerPoint.position;
        Vector3 offset = transform.position - centerPoint.position;
        radius = offset.magnitude;
        angleRad = Mathf.Atan2(offset.y, offset.x);

        gatherSpeed = radius / gatherDuration;
    }

    void Update()
    {
        angleRad += angularSpeed * Mathf.Deg2Rad * Time.deltaTime;
        
        radius = Mathf.MoveTowards(radius, 0f, 
            gatherSpeed * Time.deltaTime);
        
        float x = Mathf.Cos(angleRad) * radius;
        float y = Mathf.Sin(angleRad) * radius;
        transform.position = gatherCenter + new Vector3(x, y, 0f);

        transform.up = (gatherCenter - transform.position).normalized;

        if (radius <= 0)
        {
            Destroy(gameObject);
        }
    }
}
