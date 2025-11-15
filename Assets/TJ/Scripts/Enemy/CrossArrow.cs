using System;
using UnityEngine;

public class CrossArrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.SetActive(false);
        }

        if (other.tag == "Shield")
        {
            gameObject.SetActive(false);
        }
    }
}
