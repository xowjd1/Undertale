using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ThirdState : MonoBehaviour, IBossState
{
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private RectTransform panelRectTransform;
    
    [SerializeField] private GameObject gasterBlaster;
    
    private List<GameObject> blasterList;
    
    public void Enter()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 1000f, 500f, () =>
        {
            Proceed();
        }));
    }

    public void Proceed()
    {
        StartCoroutine(Attack());
    }

    public void Exit()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 1500f, 400f, () =>
            {
                bossBattleUIManager.PlayerTurn();
                BossBattleUIManager.IsPlayerTurn = true;
            },
            () =>
            {
                bossBattleUIManager.GetPlayer().ChangeStateToRed();
                bossBattleUIManager.GetPlayer().gameObject.SetActive(false);
            }));
    }
    
    private IEnumerator Attack()
    {
        bossBattleUIManager.BossTurn();
        bossBattleUIManager.GetPlayer().transform.position = new Vector3(0f, -1f, 0f);
        
        float radius = 3.5f;
        
        for (int i = 0; i < 11; i++)
        {
            var center = new Vector2(bossBattleUIManager.GetPlayer().transform.position.x,
                bossBattleUIManager.GetPlayer().transform.position.y);
            float angle = Random.Range(0f, 360f);
            
            var blaster = Instantiate(gasterBlaster,
                new Vector3(center.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    center.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0f), Quaternion.Euler(0f, 0f, angle - 90f));
            
            yield return new WaitForSeconds(0.85f);
        }
        
        yield return new WaitForSeconds(1f);
        Exit();
    }
}
