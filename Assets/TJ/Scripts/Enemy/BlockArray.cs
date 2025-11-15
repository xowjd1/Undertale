using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockArray : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private GameObject mettaBlock;
    [SerializeField] private GameObject mettaBomb;
    
    [Header("배열 설정")]
    [SerializeField] private int count = 4;
    [SerializeField] private float spacing = 1.6f;
    
    private int maxBombs = 2;
    [SerializeField] private float speed = 3f;
    private Vector3 moveDirection = Vector3.down;
    
    private bool isSlowingDown = false;
    private bool isMoving = true;
    private void Start()
    {
        int bombCount = Random.Range(1, maxBombs + 1);

        var bombIndex = new List<int>();
        while (bombIndex.Count < bombCount)
        {
            int index = Random.Range(0, count);
            if(!bombIndex.Contains(index))
                bombIndex.Add(index);
        }

        for (int i = 0; i < count; i++)
        {
            bool isBomb = bombIndex.Contains(i);
            
            GameObject go = isBomb ? mettaBomb : mettaBlock;
            
            Vector3 pos = transform.position + Vector3.right * spacing * i;

            Instantiate(go, pos, Quaternion.identity, transform);

        }
    }

    private void Update()
    {
        if (isMoving)
            transform.position += moveDirection * speed * Time.deltaTime;
    }
    public void BeginSlowDownAndRise()
    {
        if (!isSlowingDown)
            StartCoroutine(SlowDownAndRise());
    }
    private IEnumerator SlowDownAndRise()
    {
        isSlowingDown = true;

        float duration = 1f;
        float elapsed = 0f;
        float startSpeed = speed;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            speed = Mathf.Lerp(startSpeed, 0f, elapsed / duration);
            yield return null;
        }

        speed = 0f;
        moveDirection = Vector3.up;
        
        elapsed = 0f;
        float targetSpeed = startSpeed;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            speed = Mathf.Lerp(0f, targetSpeed, elapsed / duration);
            yield return null;
        }

        speed = targetSpeed;
        isSlowingDown = false;
    }
}
