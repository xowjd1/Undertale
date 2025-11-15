using UnityEngine;

public static class SoundUtils
{
    public static void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject soundObject = new GameObject();
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        
        audioSource.PlayOneShot(clip, volume);
        Object.Destroy(soundObject, clip.length);
    }
}
