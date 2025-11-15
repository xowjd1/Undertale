using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondState : MonoBehaviour, IBossState
{
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private RectTransform panelRectTransform;
    
    [SerializeField] private GameObject bone;
    
    public void Enter()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 1200f, 350f, () =>
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
        bossBattleUIManager.GetPlayer().ChangeStateToBlue();
        bossBattleUIManager.GetPlayer().transform.position = new Vector3(0f, -2f, 0f);

        var offsetX1 = new Vector2(panelRectTransform.anchoredPosition.x - 600f, 0f);
        var offsetX2 = new Vector2(panelRectTransform.anchoredPosition.x + 600f, 0f);
        
        var OffsetY = new Vector2(0f, panelRectTransform.anchoredPosition.y + 350f);
        
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(0.7f);
            
            var boneRect1 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect1.sizeDelta = new Vector2(boneRect1.sizeDelta.x, 280f);
            boneRect1.rotation = Quaternion.Euler(0f, 0f, 180f);
            var boneRect2 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect2.sizeDelta = new Vector2(boneRect2.sizeDelta.x, 70f);
            
            boneRect1.anchoredPosition = offsetX2 + OffsetY;
            boneRect2.anchoredPosition = offsetX1;

            StartCoroutine(BonePosDeltaMinus(boneRect1, panelRectTransform));
            
            yield return new WaitForSeconds(0.1f);
            
            StartCoroutine(BonePosDeltaPlus(boneRect2, panelRectTransform));
        }
        
        yield return new WaitForSeconds(3f);
        Exit();
    }
    
    private IEnumerator BonePosDeltaPlus(RectTransform rect, RectTransform panelRect)
    {
        var maxValue = panelRect.anchoredPosition.x + 600f;
        
        while (rect.anchoredPosition.x < maxValue - 10f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, 
                new Vector2(maxValue, rect.anchoredPosition.y),
                400f * Time.deltaTime);
            
            yield return null;
        }
        
        rect.gameObject.SetActive(false);
        Destroy(rect.gameObject);
    }
    
    private IEnumerator BonePosDeltaMinus(RectTransform rect, RectTransform panelRect)
    {
        var maxValue = panelRect.anchoredPosition.x - 600f;
        
        while (rect.anchoredPosition.x > maxValue +10f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, 
                new Vector2(maxValue, rect.anchoredPosition.y),
                400f * Time.deltaTime);
            
            yield return null;
        }
        
        rect.gameObject.SetActive(false);
        Destroy(rect.gameObject);
    }
}
