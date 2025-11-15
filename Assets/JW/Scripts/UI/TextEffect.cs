using TMPro;
using UnityEngine;

public static class TextEffect
{
    private static float fadeValue = 1f;

    public static void Fade(TextMeshProUGUI targetText)
    {
        if (targetText.alpha <= 0f || targetText.alpha >= 1f)
        {
            fadeValue *= -1f;
        }
        
        targetText.alpha += fadeValue * Time.deltaTime;
    }

    public static void InitFade(TextMeshProUGUI targetText)
    {
        targetText.alpha = 1f;
        fadeValue = 1f;
    }
}
