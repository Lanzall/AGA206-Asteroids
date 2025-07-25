using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HoverSound()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[0];

        audioSource.Play();
    }

    public void ClickPlay()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Asteroids"); //The name of your gameplay scene

        if (audioClip == null || audioClip.Length == 0)
            return;
    }

    public void ClickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();

        if (audioClip == null || audioClip.Length == 0)
            return;
    }

    public void ClickKojima()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[2];

        audioSource.Play();
        Debug.Log("He created the Hideo Game");
    }
}
