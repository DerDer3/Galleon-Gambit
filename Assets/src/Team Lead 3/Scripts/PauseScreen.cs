using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private bool freezeState = false;
    private bool resumeGame = false;
    public void pauseGame()
    {
        freezeState = !freezeState;
        if (freezeState)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void QuitGame()
    {
        Debug.Log("Game quit!");
        Application.Quit();
    }

    public void ResumeGame()
    {
        resumeGame = !resumeGame;
        if (resumeGame)
        {
            //unrender pause screen 
            pauseGame();
        }
        //SceneManager.LoadScene("MenuScene");
    }
}
