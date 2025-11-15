using TMPro;
using UnityEngine;

public class BattleItemButton : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    public void OnUseHpPotion()
    {
        if (gameState.hpPotionCount == 0 || gameState.playerHp == 60)
        {
            return;
        }
        
        gameState.hpPotionCount -= 1;
        gameState.playerHp += 30;
        
        var component = GetComponentInChildren<TextMeshProUGUI>();
        component.text = $"회복 물약 X {gameState.hpPotionCount}";
        
        if (gameState.playerHp > 60)
        {
            gameState.playerHp = 60;
        }
    }
}
