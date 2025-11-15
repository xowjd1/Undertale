using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    private bool isBottom = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BattlePanel"))
        {
            isBottom = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BattlePanel"))
        {
            isBottom = false;
        }
    }

    public bool GetValue()
    {
        return isBottom;
    }
}
