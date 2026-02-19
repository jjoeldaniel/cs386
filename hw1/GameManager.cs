using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    private int leftScore = 0;
    private int rightScore = 0;

    public void OnScore(bool isLeftGoal)
    {
        if (isLeftGoal)
        {
            rightScore++;
            rightScoreText.text = rightScore.ToString();
        }
        else
        {
            leftScore++;
            leftScoreText.text = leftScore.ToString();
        }
    }
}