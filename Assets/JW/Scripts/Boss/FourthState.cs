using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FourthState : MonoBehaviour, IBossState
{
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private RectTransform panelRectTransform;
    
    [SerializeField] private GameObject bone;
    [SerializeField] private GameObject cautionField;
    
    [SerializeField] private AudioClip gravitySound;
    [SerializeField] private AudioClip cautionSound;
    [SerializeField] private AudioClip boneRiseSound;

    public void Enter()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 450f, 450f, () =>
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
                bossBattleUIManager.GetPlayer().GravityDown();
                bossBattleUIManager.GetPlayer().gameObject.SetActive(false);
            }));
    }

    private IEnumerator Attack()
    {
        bossBattleUIManager.BossTurn();
        bossBattleUIManager.GetPlayer().ChangeStateToBlue();
        bossBattleUIManager.GetPlayer().gameObject.transform.position = new Vector3(0f, -1f, 0f);
        
        
        for (int i = 0; i < 9; i++)
        {
            int dir = Random.Range(0, 4);
            Vector2 offset = panelRectTransform.anchoredPosition;
            Vector2 addValue = Vector2.zero;

            switch (dir)
            {
                case 0:
                    
                    bossBattleUIManager.GetPlayer().GravityUp();
                    offset += new Vector2(-200f, 450f);
                    addValue += new Vector2(40f, 0f);
                    break;
                case 1:
                    
                    bossBattleUIManager.GetPlayer().GravityDown();
                    offset -= new Vector2(200f, 0f);
                    addValue += new Vector2(40f, 0f);
                    break;
                case 2:
                    
                    bossBattleUIManager.GetPlayer().GravityLeft();
                    offset -= new Vector2(225f, -25f);
                    addValue += new Vector2(0, 40f);
                    break;
                default:
                    
                    bossBattleUIManager.GetPlayer().GravityRight();
                    offset += new Vector2(225f, 25f);
                    addValue += new Vector2(0f, 40f);
                    break;
            }
            
            SoundUtils.PlaySound(gravitySound, gameObject.transform.position);
            bossBattleUIManager.GetPlayer().CurrentGravity = bossBattleUIManager.GetPlayer().GetGravityScale() * 30f;
            
            var cautionRect = bossBattleUIManager.InstantiateUIObject(cautionField, battleCanvas, panelRectTransform);
            cautionRect.sizeDelta = new Vector2(panelRectTransform.rect.width,
                cautionRect.rect.height + 200f);
            SoundUtils.PlaySound(cautionSound, gameObject.transform.position);
            
            if (dir == 0)
            {
                cautionRect.anchoredPosition = panelRectTransform.anchoredPosition + new Vector2(0f, 450f);
                cautionRect.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (dir == 1)
            {
                cautionRect.anchoredPosition = panelRectTransform.anchoredPosition;
                cautionRect.anchoredPosition = new Vector2();
                
            }
            else if (dir == 2)
            {
                cautionRect.anchoredPosition = panelRectTransform.anchoredPosition + new Vector2(-225f, 225f);
                cautionRect.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                cautionRect.anchoredPosition = panelRectTransform.anchoredPosition + new Vector2(225f, 225f);
                cautionRect.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        
            yield return new WaitForSeconds(0.8f);
            Destroy(cautionRect.gameObject);
            
            List<RectTransform> list = new List<RectTransform>();
        
            for (int j = 0; j < 11; j++)
            {
                var boneRect = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
                boneRect.anchoredPosition = offset;
                list.Add(boneRect);

                if (dir == 0)
                {
                    boneRect.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
                else if (dir == 2)
                {
                    boneRect.rotation = Quaternion.Euler(0f, 0f, -90f);
                }
                else if (dir == 3)
                {
                    boneRect.rotation = Quaternion.Euler(0f, 0f, 90f);
                }

                StartCoroutine(bossBattleUIManager.BoneSizeDelta(boneRect, 200f));
            
                offset += addValue;
            }
            SoundUtils.PlaySound(boneRiseSound, gameObject.transform.position);
        
            yield return new WaitForSeconds(0.5f);

            foreach (var rect in list)
            {
                Destroy(rect.gameObject);
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
        Exit();
    }
}
