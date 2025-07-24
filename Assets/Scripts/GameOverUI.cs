using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text ScoreTextBox, HighScoreTextBox;

    public GameObject GameOverPanel;
    public GameObject Celebrate;
    void Start()
    {
        Hide();
        Celebrate.SetActive(false);
    }

    public void Hide()
    {
        GameOverPanel.SetActive(false);
    }

    public void Show(bool celebrateHighScore, int score, int highScore)
    {
        ScoreTextBox.text = "Score: " + score.ToString();
        HighScoreTextBox.text = "High Score: " + highScore.ToString();
        GameOverPanel.SetActive(true);
        Celebrate.SetActive(celebrateHighScore);
    }

    public void ClickPlayAgain()
    {
        SceneManager.LoadScene("Asteroids");
    }

    public void ClickMainMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
