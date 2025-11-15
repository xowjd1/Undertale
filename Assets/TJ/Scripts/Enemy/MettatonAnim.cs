using UnityEngine;

public class MettatonAnim : MonoBehaviour
{
    // 몸통은 좌우로 흔들리고
    // 팔은 위아래로 움직임
    [SerializeField] private Transform torso;  
    [SerializeField] private Transform arms; 
    
    [SerializeField] private float torsoAngleAmplitude = 3f;  // 최대 Z 회전 각도
    [SerializeField] private float torsoSpeed = 10f;            // 흔드는 속도

    [SerializeField] private float armsMoveAmplitude = 0.1f;   // 팔의 Y 이동 범위
    [SerializeField] private float armsSpeed = 4f;             // 팔 움직이는 속도
    [SerializeField] private Animator armsAnimator;  
    [SerializeField] private float armsAnimSpeed = 0.2f;
    
    private Vector3 armsStartPos;
    
    private void Awake()
    {
        armsStartPos = arms.localPosition;
        if (armsAnimator != null)
        {
            armsAnimator.speed = armsAnimSpeed;
        }
    }

    private void Update()
    {
        var t = Time.time;
        
        float zRot = Mathf.Sin(t * torsoSpeed) * torsoAngleAmplitude;
        torso.localRotation = Quaternion.Euler(0, 0, zRot);

        float yOffset = Mathf.Sin(t * armsSpeed) * armsMoveAmplitude;
        arms.localPosition = armsStartPos + Vector3.up * yOffset;
    }
}
