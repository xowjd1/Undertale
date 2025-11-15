using UnityEngine;

public class BattleFightButton : MonoBehaviour
{
    [SerializeField] private GameObject playerAttackPanel;
    
    public void OnFight()
    {
        playerAttackPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
