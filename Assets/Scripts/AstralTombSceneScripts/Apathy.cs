using UnityEngine;
using UnityEngine.Audio;

public class Apathy : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;
    public Animator Animator;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
    }


    public void PlayRoarSound()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[0];

        audioSource.Play();
    }

    public void PlayCrackSound()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[1];

        audioSource.Play();
    }
}
