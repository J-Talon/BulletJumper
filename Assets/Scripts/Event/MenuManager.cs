using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void startGame()
    {
        SceneManager.LoadSceneAsync("Scenes/MainScene");
    }

        public void endGame()
    {
        SceneManager.LoadSceneAsync("Scenes/UI_prototype");

    }

        public void deadGame()
    {
        SceneManager.LoadSceneAsync("Scenes/EndCard");
    }
   
}
