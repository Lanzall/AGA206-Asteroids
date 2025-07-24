using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text ScoreTextBox;
    private Spaceship ship;

    void Start()
    {
        ship = FindFirstObjectByType<Spaceship>();
    }

    void Update()
    {
        if (ship != null)
        {
            ScoreTextBox.text = "Score: " + ship.Score.ToString();
        }
    }
}
