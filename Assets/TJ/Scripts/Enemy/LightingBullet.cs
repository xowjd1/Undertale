using UnityEngine;

public class LightingBullet : MonoBehaviour
{
    public Transform pivot;
    [HideInInspector] public float angularSpeed;
    [HideInInspector] public float radialSpeed;
    [HideInInspector] public float initialAngle;
    [SerializeField] private BattlePlayerController player;

    private float _angle;
    private float _radius;

    private void Start()
    {
        _angle = initialAngle;
        _radius = 0f;
    }

    private void Update()
    {
        _angle += angularSpeed * Time.deltaTime;
        _radius += radialSpeed * Time.deltaTime;

        float rad = _angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * _radius;

        transform.position = pivot.position + offset;
        if (transform.position.y < -7 || transform.position.y > 7
                                       || transform.position.x < -11 || transform.position.x > 11)
        {
            Destroy(gameObject);
        }
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