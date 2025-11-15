using System.Collections;
using UnityEngine;

public class MettatonHeart : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private GameObject lighting;

    [Header("하트 움직임 설정값")]
    [SerializeField] private float width  = 1f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float speed  = 1f;
    
    private float t = 0f;
    private float _attackTimer = 0f;
    private float attackInterval = 4f;

    private Coroutine _lightingRoutine;

    private void Update()
    { 
        t += Time.deltaTime * speed;
        
        float x = Mathf.Sin(t) * width;
        float y = Mathf.Sin(2f * t) * (height * 0.5f);
        transform.position = new Vector3(0,2,0) + new Vector3(x, y, 0f);

        // 4초마다 자동 공격
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= attackInterval && _lightingRoutine == null)
        {
            _lightingRoutine = StartCoroutine(SpawnLightingWaves());
            _attackTimer = 0f;
        }
    }

    private IEnumerator SpawnLightingWaves()
    {
        const int countPerWave = 10;
        const int waveCount = 3;
        const float waveDelay = 0.2f;
        const float angularSpeed = 12f;
        const float radialSpeed = 3.0f;

        float angleStep = 360f / countPerWave;

        for (int wave = 0; wave < waveCount; wave++)
        {
            for (int i = 0; i < countPerWave; i++)
            {
                var go = Instantiate(lighting, transform.position, Quaternion.identity,transform);

                var lb = go.GetComponent<LightingBullet>();
                lb.pivot = transform;
                lb.initialAngle = angleStep * i;
                lb.angularSpeed = angularSpeed;
                lb.radialSpeed = radialSpeed;
            }
            yield return new WaitForSeconds(waveDelay);
        }

        _lightingRoutine = null;
    }
}