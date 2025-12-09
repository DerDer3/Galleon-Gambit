using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class CheckIdle : MonoBehaviour
{
    [Header("Idle Settings")]
    public float idleThreshold = 10f; // seconds before switching to BattleScene
    private float idleTimer = 0f;
    private bool sceneSwitching = false;
    private Vector2 lastMousePos;

    private string mapSceneName = "Map";
    private string battleSceneName = "BattleScene";
    private bool battleSceneLoaded = false;

    void Start()
    {
        if (Mouse.current != null)
            lastMousePos = Mouse.current.position.ReadValue();
    }

    void Update()
    {
        if (sceneSwitching) return;

        Vector2 currentMousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;

        // Only count UI clicks as active input
        bool pointerClicking = Mouse.current.leftButton.isPressed;
        bool pointerOverUI = pointerClicking && EventSystem.current != null &&
                             EventSystem.current.IsPointerOverGameObject();

        string currentScene = SceneManager.GetActiveScene().name;

        // --- Map Scene Idle Check ---
        if (currentScene == mapSceneName && !battleSceneLoaded)
        {
            if (Vector2.Distance(currentMousePos, lastMousePos) > 0.1f || pointerOverUI)
            {
                idleTimer = 0f;
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer >= idleThreshold)
                {
                    StartCoroutine(LoadBattleSceneAdditive());
                }
            }
        }
        // --- Battle Scene Check for Returning to Map ---
        else if (battleSceneLoaded)
        {
            if (Vector2.Distance(currentMousePos, lastMousePos) > 0.1f)
            {
                StartCoroutine(UnloadBattleScene());
            }
        }

        lastMousePos = currentMousePos;
    }

    private IEnumerator LoadBattleSceneAdditive()
    {
        sceneSwitching = true;
        yield return null; // wait a frame
        StaticClass.CrossSceneInformation = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(battleSceneName, LoadSceneMode.Additive);
        yield return asyncLoad;

        battleSceneLoaded = true;
        sceneSwitching = false;
    }

    private IEnumerator UnloadBattleScene()
    {
        sceneSwitching = true;
        yield return null; // wait a frame
        StaticClass.CrossSceneInformation = false;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(battleSceneName);
        yield return asyncUnload;

        battleSceneLoaded = false;
        idleTimer = 0f; // reset timer when returning
        sceneSwitching = false;
    }
}

public static class StaticClass
{
    public static bool CrossSceneInformation { get; set; }
}
