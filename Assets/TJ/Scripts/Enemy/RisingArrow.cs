using System.Collections;
using UnityEngine;

public class RisingArrow : MonoBehaviour
{
    [SerializeField] private float firstMoveSpeed = 2f;
    [SerializeField] private float secondMoveSpeed = 8f;
    [SerializeField] private float initialPause = 0.5f;  // 맨 처음 멈춤
    [SerializeField] private float midPause     = 0.5f;  // 첫 번째 상승 후 멈춤
    [SerializeField] private float finalPause   = 0.2f;  // 두 번째 상승 후 멈춤

    private void Start()
    {
        StartCoroutine(RiseSequence());
    }

    private IEnumerator RiseSequence()
    {
        // 1) 스폰 직후 initialPause 만큼 대기
        yield return new WaitForSecondsRealtime(initialPause);

        // 2) firstTarget 까지 상승
        Vector3 firstTarget = transform.position + Vector3.up * 0.5f;
        while (Vector3.Distance(transform.position, firstTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                firstTarget, 
                firstMoveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 3) midPause 만큼 대기
        yield return new WaitForSecondsRealtime(midPause);

        // 4) secondTarget 까지 상승
        Vector3 secondTarget = firstTarget + Vector3.up * 1.3f;
        while (Vector3.Distance(transform.position, secondTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                secondTarget, 
                secondMoveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 5) finalPause 만큼 대기
        yield return new WaitForSecondsRealtime(finalPause);

        // 6) 파괴
        Destroy(gameObject);
    }
}