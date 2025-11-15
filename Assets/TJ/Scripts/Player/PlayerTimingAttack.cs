using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimingAttack : MonoBehaviour
{
    [SerializeField] private RectTransform cursorPrefab;
    [SerializeField] private RectTransform targetZone;
    [SerializeField] private float speed = 400f;
    [SerializeField] private int cursorCount = 3;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private RectTransform cursorContainer;
    [SerializeField] private UndyneHpBar undyneHpBar;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject d900;
    [SerializeField] private GameObject d1100;
    [SerializeField] private GameObject d1300;
    [SerializeField] private GameObject d1500;
    [SerializeField] private GameObject d1700;
    [SerializeField] private GameObject d1900;
    [SerializeField] private GameObject d2100;
    
    private readonly List<RectTransform> _cursors = new List<RectTransform>();
    private int _nextCursorToRemove = 0;

    private int perfectDamage = 700;
    private int normalDamage  = 500;
    private int weakDamage    = 300;

    private int totalDamage = 0;
    public int TotalDamage => totalDamage;
        
    private bool _isCompleted = false;
    public bool IsCompleted => _isCompleted;


    private void OnEnable()
    {
        totalDamage = 0;
        _nextCursorToRemove = 0;
        foreach (var c in _cursors)
            if (c != null) Destroy(c.gameObject);
        _cursors.Clear();
        StartCoroutine(SpawnCursors());
        d900.SetActive(false);
        d1100.SetActive(false);
        d1300.SetActive(false);
        d1500.SetActive(false);
        d1900.SetActive(false);
        d2100.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (var c in _cursors)
            if (c != null) Destroy(c.gameObject);
        _cursors.Clear();
    }
    public void StartTiming()
    {
        _isCompleted = false;
        totalDamage = 0;
        _nextCursorToRemove = 0;
    }

    private IEnumerator SpawnCursors()
    {
        for (int i = 0; i < cursorCount; i++)
        {
            var cursor = Instantiate(cursorPrefab, cursorContainer, false);
            _cursors.Add(cursor);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _cursors.Count; i++)
        {
            var c = _cursors[i];
            if (c != null && c.gameObject.activeInHierarchy)
                c.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && _nextCursorToRemove < _cursors.Count)
        {
            RemoveCursorAt(_nextCursorToRemove);
            _nextCursorToRemove++;

            if (_nextCursorToRemove >= cursorCount)
            {
                ApplyAllDamage();
                cameraShake.TriggerShake(0.2f, 0.2f);
            }
        }
    }

    private void RemoveCursorAt(int index)
    {
        var c = _cursors[index];
        if (c == null) return;

        float targetCenter = targetZone.anchoredPosition.x;
        float halfWidth = targetZone.rect.width * 0.5f;
        float distance = Mathf.Abs(c.anchoredPosition.x - targetCenter);

        int addDamage;

        if (distance < halfWidth * 0.3f)
            addDamage = perfectDamage;
        else if (distance < halfWidth * 0.6f)
            addDamage = normalDamage;
        else
            addDamage = weakDamage;

        totalDamage += addDamage;


        Debug.Log(
            $"Hit {index} | cursorX={c.anchoredPosition.x:F1}, targetX={targetCenter:F1}, " +
            $"dist={distance:F1}, halfW={halfWidth:F1}, add={addDamage}, total={totalDamage}"
        );

        c.gameObject.SetActive(false);
    }


    private void ApplyAllDamage()
    {
        if      (totalDamage == 900)  d900.SetActive(true);
        else if (totalDamage == 1100) d1100.SetActive(true);
        else if (totalDamage == 1300) d1300.SetActive(true);
        else if (totalDamage == 1500) d1500.SetActive(true);
        else if (totalDamage == 1700) d1700.SetActive(true);
        else if (totalDamage == 1900) d1900.SetActive(true);
        else                          d2100.SetActive(true);

        _isCompleted = true;
    }
}
