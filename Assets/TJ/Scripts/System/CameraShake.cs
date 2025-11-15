using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float defaultDuration  = 0.5f;
    [SerializeField] private float defaultMagnitude = 0.2f;
    [SerializeField] private Transform playerCamera;

    private Vector3 fixedPos = new Vector3(0f, 0f, -10f);
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
        
        fixedPos = new Vector3(0f, 0f, -10f);
    }

    public void TriggerShake() =>
        TriggerShake(defaultDuration, defaultMagnitude);

    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float x = (Mathf.PerlinNoise(Time.time * 10f, 0f) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f) * 2f;
            
            playerCamera.localPosition = fixedPos + new Vector3(x, y, 0f) * magnitude;
            yield return null;
        }
        
        playerCamera.localPosition = fixedPos;
        shakeRoutine = null;
    }
}