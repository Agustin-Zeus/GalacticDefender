using UnityEngine;
using UnityEngine.UI; // Para usar el sistema de UI estándar
using TMPro;

public class VictoryScreenLevel2 : MonoBehaviour
{
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {

        int level = 2; // Cambia según el nivel
        int score = PlayerPrefs.GetInt("ScoreLevel" + level, 0);
        int highScore = PlayerPrefs.GetInt("HighScoreLevel" + level, 0);


        totalScoreText.text = " " + score;
        highScoreText.text = " " + highScore;

    }
}
