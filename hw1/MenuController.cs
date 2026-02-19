using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT button pressed.");
        Application.Quit();
    }
}