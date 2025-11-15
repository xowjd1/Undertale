using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mettatonHpText;
    [SerializeField] private TextMeshProUGUI undyneHpText;
    [SerializeField] private Slider mettatonHpSlider;
    [SerializeField] private Slider undyneHpSlider;
    [SerializeField] private GameState gameState;

    private int maxPlayerHp;

    private void Start()
    {
        maxPlayerHp = gameState.playerHp;

        if (mettatonHpSlider != null)
        {
            mettatonHpSlider.minValue     = 0;
            mettatonHpSlider.maxValue     = maxPlayerHp;
            mettatonHpSlider.wholeNumbers = true;
        }
        if (undyneHpSlider != null)
        {
            undyneHpSlider.minValue     = 0;
            undyneHpSlider.maxValue     = maxPlayerHp;
            undyneHpSlider.wholeNumbers = true;
        }
    }

    private void Update()
    {
        int currentHp = gameState.playerHp;

        if (mettatonHpText != null)
            mettatonHpText.text = $"{currentHp} / {maxPlayerHp}";
        if (undyneHpText != null)
            undyneHpText.text   = $"{currentHp} / {maxPlayerHp}";

        if (mettatonHpSlider != null)
            mettatonHpSlider.value = currentHp;
        if (undyneHpSlider != null)
            undyneHpSlider.value   = currentHp;
    }
}