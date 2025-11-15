using UnityEngine;
using UnityEngine.UI;

public class UndyneHpBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    private int _maxHP;

    public void Initialize(int hp)
    {
        _maxHP = hp;
        hpSlider.maxValue = 1f;
        hpSlider.value    = 1f;
    }

    public void ApplyDamage(int damage)
    {
        if (hpSlider == null) return;
        float ratio = damage / (float)_maxHP; 
        hpSlider.value = Mathf.Clamp01(hpSlider.value - ratio);
    }
}