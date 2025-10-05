using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private AmmoData ammoData;
   // [SerializeField] private HighscoreData highscoreData;
    
    public static ScoreManager Instance { get; private set; }
    
    public ScoreData ScoreData => scoreData;
    public AmmoData AmmoData => ammoData;
   // public HighScoreData HighscoreData => highscoreData;

    public UIDocument uiDocument;

    public Label scoreText;
    public Label ammoText;
    public Label highscoreText;

    private void Awake()
    {
        
        // Singleton pattern to ensure only one ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
            
            // Load the score data if it exists, otherwise create a new one
            if (scoreData == null)
            {
                scoreData = Resources.Load<ScoreData>("ScoreData");
                if (scoreData == null)
                {
                    Debug.LogError("ScoreData ScriptableObject not found! Please create one in the Resources folder.");
                }
            }

            if (ammoData == null)
            {
                ammoData = Resources.Load<AmmoData>("AmmoData");
                if (ammoData == null)
                {
                    Debug.LogError("AmmoData ScriptableObject not found! Please create one in the Resources folder.");
                }
            }

            //     if (highscoreData == null)
            // {
            //     highscoreData = Resources.Load<HighscoreData>("HighscoreData");
            //     if (highscoreData == null)
            //     {
            //         Debug.LogError("HighscoreData ScriptableObject not found! Please create one in the Resources folder.");
            //     }
            // }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        if (scoreData != null)
        {
            scoreData.AddScore(points);
        //    Debug.Log($"Score added: {points}. Current score: {scoreData.CurrentScore}");
        }
    }

    public void ResetScore()
    {
        if (scoreData != null)
        {
            scoreData.ResetScore();
        //    Debug.Log("Score reset to 0");
        }
    }

    public int GetCurrentScore()
    {
        return scoreData != null ? scoreData.CurrentScore : 0;
    }

    public int GetHighScore()
    {
        return scoreData != null ? scoreData.HighScore : 0;
    }

        public void AddAmmo(int points)
    {
        if (ammoData != null)
        {
            ammoData.AddAmmo(points);
            //Debug.Log($"Score added: {points}. Current score: {scoreData.CurrentScore}");
        }
    }

    public void ResetAmmo()
    {
        if (ammoData != null)
        {
            ammoData.ResetAmmo();
         //   Debug.Log("Ammo reset to 0");
        }
    }

    public int GetCurrentAmmo()
    {
        return ammoData != null ? ammoData.CurrentAmmo : 0;
    }

    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        ammoText = uiDocument.rootVisualElement.Q<Label>("AmmoCount");
        highscoreText = uiDocument.rootVisualElement.Q<Label>("Highscore");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("UI_prototype"))
        {
            ResetScore();
            ResetAmmo();
        }


    }
    void Update()
    {
        scoreText.text = "Score: " + GetCurrentScore();
        ammoText.text = "Ammo: " + GetCurrentAmmo();
        highscoreText.text = "Highscore: " + GetHighScore();

    }
}
