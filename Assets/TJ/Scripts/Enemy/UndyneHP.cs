// UndyneHP.cs
using UnityEngine;

public class UndyneHP : MonoBehaviour
{
    [SerializeField] private UndyneHpBar _hpBar;
    public int undyneHP = 10000;

    private void Awake()
    {
        if (_hpBar == null)
            Debug.LogError("UndyneHP: HpBar가 누락되었습니다.");
        
        _hpBar.Initialize(undyneHP);
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log($"[TakeDamage] before={undyneHP}, dmg={dmg}");
        undyneHP = Mathf.Max(0, undyneHP - dmg);
        Debug.Log($"[TakeDamage] after={undyneHP}");
        _hpBar.ApplyDamage(dmg);
    }
}