using System.Collections;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private UISystemManager uiSystemManager;
    [SerializeField] private GameObject playerHeart;
    [SerializeField] private GameObject playerCaution;
    
    [SerializeField] private GameState gameState;
    [SerializeField] private GameState saveData;

    [SerializeField] private AudioClip blinkSound;
    [SerializeField] private AudioClip battleStartSound;

    [SerializeField] private GameCharacter monsterType;
    
    private readonly Vector3 offset = new Vector3(8f, 7.5f, 0f);
    private static bool canEnterBoss = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameState.playerHp = saveData.playerHp;
            gameState.hpPotionCount = saveData.hpPotionCount;
            
            if (monsterType == GameCharacter.Undyne || monsterType == GameCharacter.Mettaton)
            {
                if (monsterType == GameCharacter.Mettaton)
                {
                    uiSystemManager.SetBattleToMettaton();
                }
                else
                {
                    uiSystemManager.SetBattleToUndyne();
                }
                
                StartCoroutine(EncounterEffect(monsterType));
            }
            else if (monsterType == GameCharacter.Boss)
            {
                uiSystemManager.SetBattleToBoss();
                uiSystemManager.StartDialogue(DialogueID.GetID("Boss"));
            }
        }
    }

    private void Update()
    {
        if (!canEnterBoss)
        {
            return;
        }
        
        canEnterBoss = false;
        StartCoroutine(EncounterEffect(GameCharacter.Boss));
    }

    public static void EnterBoss()
    {
        canEnterBoss = true;
    }

    private IEnumerator EncounterEffect(GameCharacter character)
    {
        uiSystemManager.IsFacing = true;
        playerHeart.transform.position = uiSystemManager.GetPlayerPosition();

        if (character != GameCharacter.Boss)
        {
            yield return StartCoroutine(CautionEffect());
        }
        
        playerCaution.SetActive(false);
        uiSystemManager.GetReadyToBattle();
        
        for (int i = 0; i < 3; i++)
        {
            playerHeart.SetActive(true);
            
            if (i == 2)
            {
                break;
            }
            
            yield return new WaitForSeconds(0.1f);
            playerHeart.SetActive(false);
            SoundUtils.PlaySound(blinkSound, gameObject.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
        
        var destination = playerHeart.transform.position - offset;
        yield return StartCoroutine(HeartMovement(destination));
        
        Destroy(gameObject);
    }

    private IEnumerator CautionEffect()
    {
        playerCaution.SetActive(true);
        
        SoundUtils.PlaySound(blinkSound, gameObject.transform.position);
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator HeartMovement(Vector3 destination)
    {
        const float heartSpeed = 25f;
        
        SoundUtils.PlaySound(battleStartSound, gameObject.transform.position);
        while ((playerHeart.transform.position - destination).sqrMagnitude >= 1f)
        {
            playerHeart.transform.position = Vector3.MoveTowards(playerHeart.transform.position,
                destination, heartSpeed * Time.deltaTime);

            yield return null;
        }

        yield return StartCoroutine(uiSystemManager.BGFadeInEffect());
        uiSystemManager.StartBattle();
    }
}
