using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{

    public void ClickPlay()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Asteroids"); //The name of your gameplay scene

    }

    public void ClickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
