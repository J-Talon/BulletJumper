using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public ScoreManager ScoreManager => ScoreManager;

    public void startGame()
    {
        SceneManager.LoadSceneAsync("Scenes/MainScene");
    }

        public void endGame()
    {
        SceneManager.LoadSceneAsync("Scenes/UI_prototype");
        ScoreManager.ResetScore();
        ScoreManager.ResetAmmo();
    }

        public void deadGame()
    {
        SceneManager.LoadSceneAsync("Scenes/EndCard");
    }
   
}
