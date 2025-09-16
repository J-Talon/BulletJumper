using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadSceneAsync("Scenes/MainScene");
    }
}
