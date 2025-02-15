using UnityEngine;

public class PlaySoundOnce : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Stop(); // Stop the current sound (if any)
            audioSource.Play(); // Play the new sound
        }
    }
}