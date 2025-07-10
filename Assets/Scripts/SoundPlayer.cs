using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(bool randomPitch = true)
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[0];

        if (randomPitch)
        {
            audioSource.pitch = Random.Range(-0.9f, 1.1f);
        }
        else
        {
            audioSource.pitch = 1f;
        }

            audioSource.Play();
    }

    public void PlayRandomSound(bool randomPitch = true)
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];

        if (randomPitch)
        {
            audioSource.pitch = Random.Range(-0.9f, 1.1f);
        }
        else
        {
            audioSource.pitch = 1f;
        }
        audioSource.Play();
    }
}
