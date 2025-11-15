using System.Collections;
using UnityEngine;

public class FirstState : MonoBehaviour, IBossState
{
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private RectTransform panelRectTransform;
    
    [SerializeField] private GameObject bone;
    
    public void Enter()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 1000f, 400f, () =>
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

        var offsetX1 = new Vector2(panelRectTransform.anchoredPosition.x - 500f, 0f);
        var offsetX2 = new Vector2(panelRectTransform.anchoredPosition.x + 500f, 0f);
        
        var OffsetY = new Vector2(0f, panelRectTransform.anchoredPosition.y + 400f);
        
        for (int i = 0; i < 9; i++)
        {
            yield return new WaitForSeconds(0.7f);
            
            var boneRect1 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect1.sizeDelta = new Vector2(boneRect1.sizeDelta.x, 60f);
            var boneRect2 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect2.sizeDelta = new Vector2(boneRect2.sizeDelta.x, 240f);
            boneRect2.rotation = Quaternion.Euler(0f, 0f, 180f);
            
            var boneRect3 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect3.sizeDelta = new Vector2(boneRect3.sizeDelta.x, 60f);
            var boneRect4 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect4.sizeDelta = new Vector2(boneRect4.sizeDelta.x, 240f);
            boneRect4.rotation = Quaternion.Euler(0f, 0f, 180f);
            
            boneRect1.anchoredPosition = offsetX1;
            boneRect2.anchoredPosition = offsetX1 + OffsetY;
            
            boneRect3.anchoredPosition = offsetX2;
            boneRect4.anchoredPosition = offsetX2 + OffsetY;

            StartCoroutine(BonePosDeltaPlus(boneRect1, panelRectTransform));
            StartCoroutine(BonePosDeltaPlus(boneRect2, panelRectTransform));

            StartCoroutine(BonePosDeltaMinus(boneRect3, panelRectTransform));
            StartCoroutine(BonePosDeltaMinus(boneRect4, panelRectTransform));
        }
        
        yield return new WaitForSeconds(2f);
        Exit();
    }
    
    private IEnumerator BonePosDeltaPlus(RectTransform rect, RectTransform panelRect)
    {
        var maxValue = panelRect.anchoredPosition.x + 500f;
        
        while (rect.anchoredPosition.x < maxValue)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, 
                new Vector2(maxValue, rect.anchoredPosition.y),
                600f * Time.deltaTime);
            
            yield return null;
        }
        
        Destroy(rect.gameObject);
    }
    
    private IEnumerator BonePosDeltaMinus(RectTransform rect, RectTransform panelRect)
    {
        var maxValue = panelRect.anchoredPosition.x - 500f;
        
        while (rect.anchoredPosition.x > maxValue)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, 
                new Vector2(maxValue, rect.anchoredPosition.y),
                600f * Time.deltaTime);
            
            yield return null;
        }
        
        Destroy(rect.gameObject);
    }
}
