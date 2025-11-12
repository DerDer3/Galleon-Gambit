using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void QuitGame()
    {
        Debug.Log("Request to quit succesful.");
        Application.Quit();
    }
}
