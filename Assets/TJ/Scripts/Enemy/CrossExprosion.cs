using System;
using UnityEngine;

public class CrossExprosion : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private BattlePlayerController playerController;
    private void Update()
    {
        Destroy(gameObject, destroyTime);
    }
}
