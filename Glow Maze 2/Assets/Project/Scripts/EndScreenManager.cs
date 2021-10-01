using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    GameObject settingsCanvas;
    GameObject gameManager;

    void Start()
    {
        settingsCanvas = GameObject.Find("SettingsCanvas");
        gameManager = GameObject.Find("GameManager");

        DestroyInEndLevel();
    }

    void Update()
    {
        Quit();
    }

    void DestroyInEndLevel() // Destroys gameObjects that no longer needed in End scene
    {
        if (SceneManager.GetActiveScene().name == "EndScreen") //Checks if Scene name is EndScreen
        {
            Destroy(settingsCanvas);
            Destroy(gameManager);
        }
    }

    private void Quit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
