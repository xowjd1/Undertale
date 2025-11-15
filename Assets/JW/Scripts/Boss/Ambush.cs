using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Ambush : MonoBehaviour, IBossState
{
    [SerializeField] private UISystemManager uiSystemManager;
    [SerializeField] private BossBattleUIManager bossBattleUIManager;
    
    [SerializeField] private Canvas battleCanvas;
    [SerializeField] private RectTransform panelRectTransform;
    
    [SerializeField] private GameObject bone;
    [SerializeField] private GameObject gasterBlaster;
    [SerializeField] private GameObject cautionField;
    
    [SerializeField] private AudioClip gravitySound;
    [SerializeField] private AudioClip sansReadySound;
    [SerializeField] private AudioClip cautionSound;
    [SerializeField] private AudioClip boneRiseSound;
    [SerializeField] private AudioClip bellSound;
    
    private void Start()
    {
        StartCoroutine(uiSystemManager.BGFadeOutEffect());
        Enter();
        bossBattleUIManager.PlayIntroSound();
    }

    public void Enter()
    {
        StartCoroutine(WaitForAnnoyPlayer());
    }

    public void Proceed()
    {
        StartCoroutine(FaceChange());
    }

    public void Exit()
    {
        StartCoroutine(bossBattleUIManager.PanelSizeChange(panelRectTransform, 1500f, 400f, () =>
        {
            bossBattleUIManager.PlayerTurn();
            BossBattleUIManager.IsPlayerTurn = true;
            this.enabled = false;
        },
            () =>
            {
                bossBattleUIManager.audioSource.Play();
                bossBattleUIManager.GetPlayer().ChangeStateToRed();
                bossBattleUIManager.GetPlayer().gameObject.SetActive(false);
                bossBattleUIManager.GetBoss().AnimatorControl(1f);
            }));
    }

    private IEnumerator WaitForAnnoyPlayer()
    {
        yield return new WaitForSeconds(4.2f);
        
        bossBattleUIManager.StopIntroSound();
        SoundUtils.PlaySound(sansReadySound, gameObject.transform.position);
        yield return new WaitForSeconds(0.2f);
        
        yield return StartCoroutine(Attack1());
    }

    private IEnumerator Attack1()
    {
        bossBattleUIManager.GetPlayer().ChangeStateToBlue();
        bossBattleUIManager.GetPlayer().CurrentGravity = bossBattleUIManager.GetPlayer().GetGravityScale() * 30f;
        SoundUtils.PlaySound(gravitySound, gameObject.transform.position);
        
        Proceed();
        
        var cautionRect = bossBattleUIManager.InstantiateUIObject(cautionField, battleCanvas, panelRectTransform);
        cautionRect.sizeDelta = new Vector2(panelRectTransform.rect.width,
            cautionRect.rect.height + 200f);
        SoundUtils.PlaySound(cautionSound, gameObject.transform.position);
        
        yield return new WaitForSeconds(0.6f);
        Destroy(cautionRect.gameObject);
        
        Vector2 offset = panelRectTransform.anchoredPosition - new Vector2(200f, 0f);

        List<RectTransform> list = new List<RectTransform>();
        
        for (int i = 0; i < 11; i++)
        {
            var boneRect = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect.anchoredPosition = offset;
            list.Add(boneRect);

            StartCoroutine(bossBattleUIManager.BoneSizeDelta(boneRect, 200f));
            
            offset += new Vector2(40f, 0f);
        }
        SoundUtils.PlaySound(boneRiseSound, gameObject.transform.position);
        
        yield return new WaitForSeconds(0.5f);
        SoundUtils.PlaySound(bellSound, gameObject.transform.position);
        
        foreach (var rect in list)
        {
            Destroy(rect.gameObject);
        }

        yield return StartCoroutine(Attack2());
    }

    private IEnumerator Attack2()
    {
        bossBattleUIManager.GetPlayer().ChangeStateToRed();
        
        float sinForBottom = 0f;
        
        Vector2 offsetX = panelRectTransform.anchoredPosition - new Vector2(200f, 0f);
        Vector2 offsetY = panelRectTransform.anchoredPosition + new Vector2(0f, 450f);

        yield return new WaitForSeconds(0.1f);
        SoundUtils.PlaySound(sansReadySound, gameObject.transform.position);
        
        for (int i = 1; i <= 36; i++)
        {
            yield return new WaitForSeconds(0.1f);
            
            var boneRect1 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect1.anchoredPosition = offsetX;
            boneRect1.sizeDelta = 
                new Vector2(boneRect1.rect.width, 
                    boneRect1.rect.height + (Mathf.Sin(sinForBottom * Mathf.Deg2Rad) + 2f) * 100f);
            
            var boneRect2 = bossBattleUIManager.InstantiateUIObject(bone, battleCanvas, panelRectTransform);
            boneRect2.anchoredPosition = offsetX + offsetY;
            boneRect2.sizeDelta = 
                new Vector2(boneRect2.rect.width, 
                    boneRect2.rect.height + 360f - (Mathf.Sin(sinForBottom * Mathf.Deg2Rad) + 2f) * 100f);
            boneRect2.rotation = Quaternion.Euler(0f, 0f, 180f);
            
            sinForBottom += 10f;
            
            StartCoroutine(BonePosDelta(boneRect1, panelRectTransform));
            StartCoroutine(BonePosDelta(boneRect2, panelRectTransform));
        }

        yield return StartCoroutine(Attack3());
    }

    private IEnumerator Attack3()
    {
        var offsetPos = new Vector3(0f, -1f, 0f);
        float offsetRot = 0f;
        float offset1 = 3.5f;
        float offset2 = -1.5f;
        float dir = 1f;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                dir = -1f;
                offsetRot += 180f;
            }
            
            var blaster = Instantiate(gasterBlaster,
                offsetPos + new Vector3(offset2 * dir, offset1 * dir, 0f), Quaternion.Euler(0f, 0f, offsetRot));
        }

        dir *= -1f;
        offsetRot += 90f;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                dir = -1f;
                offsetRot += 180f;
            }
            
            var blaster = Instantiate(gasterBlaster,
                offsetPos + new Vector3(offset1 * dir, offset2 * dir, 0f), Quaternion.Euler(0f, 0f, offsetRot));
        }
        
        yield return new WaitForSeconds(0.85f);

        Vector2 center = new Vector2(0f, -1f);
        float radius = 4.5f;
        float angle1 = -45f;
        float angle2 = 45f;
        
        for (int i = 0; i < 4; i++)
        {
            var blaster = Instantiate(gasterBlaster,
                new Vector3(center.x + Mathf.Cos(angle2 * Mathf.Deg2Rad) * radius,
                    center.y + Mathf.Sin(angle2 * Mathf.Deg2Rad) * radius, 0f), Quaternion.Euler(0f, 0f, angle1));
            angle1 -= 90f;
            angle2 -= 90f;
        }
        
        yield return new WaitForSeconds(0.85f);
        
        dir = 1f;
        offsetRot = 0f;
            
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                dir = -1f;
                offsetRot += 180f;
            }
            
            var blaster = Instantiate(gasterBlaster,
                offsetPos + new Vector3(offset2 * dir, offset1 * dir, 0f), Quaternion.Euler(0f, 0f, offsetRot));
        }

        dir *= -1f;
        offsetRot += 90f;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                dir = -1f;
                offsetRot += 180f;
            }
            
            var blaster = Instantiate(gasterBlaster,
                offsetPos + new Vector3(offset1 * dir, offset2 * dir, 0f), Quaternion.Euler(0f, 0f, offsetRot));
        }
        
        yield return new WaitForSeconds(1.05f);
        
        dir = 1f;
        offsetRot = -90f;
        offset1 = 4f;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                dir = -1f;
                offsetRot += 180f;
            }
            
            var blaster = Instantiate(gasterBlaster,
                offsetPos + new Vector3(offset1 * dir, 0f, 0f), Quaternion.Euler(0f, 0f, offsetRot));
            blaster.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
        }
        
        yield return new WaitForSeconds(2.5f);
        bossBattleUIManager.GetBoss().ChangeHeadNormal();
        Exit();
    }
    
    private IEnumerator BonePosDelta(RectTransform rect, RectTransform panelRect)
    {
        var maxValue = panelRect.anchoredPosition.x + 200f;
        
        while (rect.anchoredPosition.x < maxValue)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, 
                new Vector2(maxValue, rect.anchoredPosition.y),
                1000f * Time.deltaTime);
            
            yield return null;
        }
        
        Destroy(rect.gameObject);
    }

    private IEnumerator FaceChange()
    {
        for (int i = 0; i < 15; i++)
        {
            bossBattleUIManager.GetBoss().ChangeEyesBlue();
            yield return new WaitForSeconds(0.05f);
            bossBattleUIManager.GetBoss().ChangeEyesYellow();
            yield return new WaitForSeconds(0.05f);
        }
        
        bossBattleUIManager.GetBoss().ChangeEyesBlack();
    }
}
