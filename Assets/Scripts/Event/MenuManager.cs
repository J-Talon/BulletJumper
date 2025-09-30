using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // public ScoreManager ScoreManager => ScoreManager;

    public void startGame()
    {
        SceneManager.LoadSceneAsync("Scenes/MainScene");
    }

        public void endGame()
    {
        // ScoreManager.ResetScore();
        // ScoreManager.ResetAmmo();
        SceneManager.LoadSceneAsync("Scenes/UI_prototype");

    }

        public void deadGame()
    {
        SceneManager.LoadSceneAsync("Scenes/EndCard");
    }
   
}
