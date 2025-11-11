using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuScreen;
    [SerializeField] private Button Resume;
    [SerializeField] private Button Quit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(MenuScreen.activeInHierarchy == false)
            {
                MenuScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void Unpause()
    {
        MenuScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Game quit!");
        Application.Quit();
    }
}