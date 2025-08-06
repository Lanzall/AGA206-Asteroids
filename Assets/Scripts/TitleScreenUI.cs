using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public Animator animator;

    public AudioClip[] audioClip;
    private AudioSource audioSource;
    

    private int levelToLoad;


    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        FadeToLevel(1);      
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void TitleGunshot()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[1];

        audioSource.Play();
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
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
