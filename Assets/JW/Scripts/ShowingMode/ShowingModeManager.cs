using UnityEngine;

public class ShowingModeManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject teleportSpot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameState.killCount = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameState.killCount = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameState.killCount = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameState.killCount = 4;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            player.transform.position = teleportSpot.transform.position;
        }
    }
}
