using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region VARIABLES
    public AudioMixer mainMixer;

    private int savedLevel;
    public int gameOverCount;

    public GameObject gameOverPanel;

    [Header("Tutorial UI")]
    [SerializeField] GameObject tutuorialPanel;
    [SerializeField] public GameObject move_i;
    [SerializeField] public GameObject fill_i;
    [SerializeField] public GameObject powerUp_i;
    [SerializeField] public GameObject tapToContinue;

    public bool isLevelCompleted;
    public bool isGameOver;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        savedLevel = PlayerPrefs.GetInt("SavedLevel"); //Save current Level for later use
        Debug.Log("STARTED SAVED LEVEL! " + savedLevel);
        SceneManager.LoadScene(savedLevel); //Load game from savedLevel

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            tutuorialPanel.SetActive(true);

            move_i.SetActive(true);
            fill_i.SetActive(false);
            powerUp_i.SetActive(false);
            tapToContinue.SetActive(false);
        }

        isGameOver = false;

        gameOverCount = 0;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            tutuorialPanel.SetActive(false);
        }

        if(fill_i.activeInHierarchy == true || powerUp_i.activeInHierarchy == true)
        {
            BallMovement.Instance.canMove = false;
        }
        else
        {
            BallMovement.Instance.canMove = true;
        }

        DeviceBackFunction();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestoreGame2()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        SettingsMenu.Instance.diamondsCount = 0;
        PlayerPrefs.DeleteKey("Diamonds");
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("Level 01");
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverCount++;

        Invoke("DeductDiamonds", 0.3f);
        PlayerPrefs.GetInt("Diamonds");
        PlayerPrefs.Save();

        Invoke("OpenGameOverPanel", 0.4f);

        Debug.Log("GAME OVER!");
    }

    public void TapToContinue()
    {
        if (fill_i.activeInHierarchy == true)
        {
            fill_i.SetActive(false);
            Invoke("ActivatePowerupTutorial", 0.3f);
        }

        if (powerUp_i.activeInHierarchy == true)
        {
            powerUp_i.SetActive(false);
            tutuorialPanel.SetActive(false);
        }
    }

    public void ActivateFillTutorial()
    {
        fill_i.SetActive(true);
        tapToContinue.SetActive(true);
    }

    public void ActivatePowerupTutorial()
    {
        powerUp_i.SetActive(true);
    }

    public void DeductDiamonds()
    {
        if(SettingsMenu.Instance.diamondsCount > 0)
        {
            SettingsMenu.Instance.diamondsCount -= LevelManager.Instance.diamondstoRemove;
        }
    }

    public void AddDiamonds()
    {
        if (SettingsMenu.Instance.diamondsCount >= 0)
        {
            SettingsMenu.Instance.diamondsCount += LevelManager.Instance.diamondstoRemove;
        }
    }

    void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void VibrateOnMove()
    {
        if (SettingsMenu.Instance.isVibrationOn)
        {
            Vibrations.Vibrate(50);
        }
    }

    public void VibrateOnWinorLose()
    {
        if (SettingsMenu.Instance.isVibrationOn)
        {
            Vibrations.Vibrate(600);
        }
    }

    public void VibrateOnButtonPress()
    {
        Vibrations.Vibrate(200);
    }

    void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount && gameOverPanel.activeInHierarchy == false)
            {
                DeductDiamonds();
            }

            PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
            Debug.Log("SAVED DIAMONDS IN ANDROID " + PlayerPrefs.GetInt("Diamonds"));
        }

#if UNITY_EDITOR
        if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount && gameOverPanel.activeInHierarchy == false)
        {
            DeductDiamonds();
        }

        PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
        Debug.Log("SAVED DIAMONDS IN EDITOR " + PlayerPrefs.GetInt("Diamonds"));
#endif
    }

    void DeviceBackFunction()
    {
#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == false && SettingsMenu.Instance.cautionPanel.activeInHierarchy == false)
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == true && SettingsMenu.Instance.cautionPanel.activeInHierarchy == false)
        {
            SettingsMenu.Instance.ClosePausePanel();
        }

        if (Input.GetKey(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == true && SettingsMenu.Instance.cautionPanel.activeInHierarchy == true)
        {
            SettingsMenu.Instance.CloseCautionPanel();
        }
#else
        if (Input.GetKeyDown(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == false && SettingsMenu.Instance.cautionPanel.activeInHierarchy == false)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == true && SettingsMenu.Instance.cautionPanel.activeInHierarchy == false)
        {
            SettingsMenu.Instance.ClosePausePanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SettingsMenu.Instance.pausePanel.activeInHierarchy == true && SettingsMenu.Instance.cautionPanel.activeInHierarchy == true)
        {
            SettingsMenu.Instance.CloseCautionPanel();
        }
#endif
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount && isGameOver == false)
            {
                DeductDiamonds();
            }

            PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
            PlayerPrefs.Save();
        }
    }
}
