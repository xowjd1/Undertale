using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private RectTransform box;
    [SerializeField] private RectTransform hit;
    [SerializeField] private RectTransform hitbar;
    public float delayTime = 10f; // 10초 지연 시간
    
    private Vector2 _movement;
    private Animator _hitbarAnimator;
    private bool _isBlinking = false;
    

    private void Awake()
    {
        StartCoroutine(BoxIncrease());
        // StartCoroutine(HitIncrease());
        hit.gameObject.SetActive(false);
        hitbar.gameObject.SetActive(false);
        
        // 하트 바 애니메이터 가져오기
        _hitbarAnimator = hit.GetComponent<Animator>();
    }

    private IEnumerator BoxIncrease()
    {
        // delayTime이 지난 후에 box 크기 증가 시작
        yield return new WaitForSeconds(delayTime);

        float fixedHeight = box.sizeDelta.y;
        float endWidth = 1000f;
        float deltaWidth = 600f;

        while (box.sizeDelta.x < endWidth)
        {
            box.sizeDelta = Vector2.MoveTowards(
                box.sizeDelta,
                new Vector2(endWidth, fixedHeight),
                deltaWidth * Time.deltaTime); // Time.deltaTime으로 이동
            yield return null;
        }
        
        hit.gameObject.SetActive(true);
        hitbar.gameObject.SetActive(true);

        
        //UI hitbar 이동 로직
        float startPosition = hitbar.anchoredPosition.x;
        // float hitHeight = hit.sizeDelta.y;
        float endPosition = startPosition + 1450f;
        float deltaPosition = 600f;
        float y = hitbar.anchoredPosition.y; // y값 고정

        while (hitbar.anchoredPosition.x < endPosition)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isBlinking)
            {
                StartCoroutine(BinkHitbar());
                yield return new WaitForSeconds(2f);
            }
            
            hitbar.anchoredPosition = Vector2.MoveTowards(
                hitbar.anchoredPosition,
                new Vector2(endPosition, y),
                deltaPosition * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator BinkHitbar()
    {
        _isBlinking = true;
        float switchTime = 0.2f;
        float totalTime = 0f;
        bool switchValue = true;

        while (totalTime <2f)
        {
            _hitbarAnimator.SetBool("Switch", switchValue);
            switchValue = !switchValue; 
            yield return new WaitForSeconds(switchTime);
            totalTime += switchTime;
        }
        
        _hitbarAnimator.SetBool("Switch", false);
        _isBlinking = false;
    }
}