using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Game/Score Data")]
public class ScoreData : ScriptableObject
{
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int highScore = 0;

    public int CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value; }
    }

    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public void AddScore(int points)
    {
        currentScore = points;
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void ResetAll()
    {
        currentScore = 0;
        highScore = 0;
    }
}
