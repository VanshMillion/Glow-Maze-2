using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance;

    #region VARIABLES
    public int diamondsCount;

    [Space]
    [Header("Toggle Functions")]
    public Toggle vibrationToggle;
    public Toggle soundToggle;

    public Sprite vibrationOnSprite;
    public Sprite vibrationOffSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public bool isVibrationOn;
    public bool isSoundOn;

    public TMP_Text[] allDiamondCountText;
    public TMP_Text resumeInformText;

    public GameObject pausePanel;
    public TMP_Text versionText;

    public AudioSource buttonSFX;
    public AudioClip clickSound;

    public Animator fadeAnim;
    public Animator resumeInform;
    #endregion

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        isVibrationOn = vibrationToggle.isOn;
        isSoundOn = soundToggle.isOn;

        diamondsCount = PlayerPrefs.GetInt("Diamonds");
        versionText.text = "V . " + Application.version.ToString();
    }

    private void Update()
    {
        if(diamondsCount < 0)
        {
            diamondsCount = 0;
        }

        for(int i = 0; i < allDiamondCountText.Length; i++)
        {
            allDiamondCountText[i].text = diamondsCount.ToString();
        }
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        BallMovement.Instance.canMove = false;
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        BallMovement.Instance.canMove = true;
    }

    public void Restart()
    {
        ClosePausePanel();
        GameManager.Instance.DeductDiamonds();
        GameManager.Instance.RestartLevel();
    }

    public void RestartAfterGameOver()
    {
        GameManager.Instance.RestartLevel();
        GameManager.Instance.Invoke("CloseGameOverPanel", 0.1f);
    }

    public void ResumeAfterGameOver()
    {
        if(diamondsCount >= 3)
        {
            diamondsCount -= 3;
            GameManager.Instance.CloseGameOverPanel();
            BallMovement.Instance.movesLeft += 5;
            BallMovement.Instance.canMove = true;
        }
        else
        {
            resumeInformText.text = "Not enough diamonds \n Starting video AD";
            resumeInform.SetTrigger("NotEnoughDiamond");
            AdmobManager.Instance.Invoke("ShowRewardedAd", 0.8f);

            if (!AdmobManager.Instance.rewardAd.IsLoaded())
            {
                Invoke("StartNoAdLoadedAnim", 1.2f);
            }
        }
    }

    void StartNoAdLoadedAnim()
    {
        Debug.Log("NO ADS LOADED!");
        resumeInformText.text = "No AD available \n Check Internet connection!";
        resumeInform.SetTrigger("NoAdLoaded");
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;

        if (isVibrationOn)
        {
            vibrationToggle.GetComponent<Image>().sprite = vibrationOnSprite;
            GameManager.Instance.VibrateOnButtonPress();
            Debug.Log("VIBRATED");
        }
        else
        {
            vibrationToggle.GetComponent<Image>().sprite = vibrationOffSprite;
        }
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            soundToggle.GetComponent<Image>().sprite = soundOnSprite;
            GameManager.Instance.mainMixer.SetFloat("Volume", 0f);
        }
        else
        {
            soundToggle.GetComponent<Image>().sprite = soundOffSprite;
            GameManager.Instance.mainMixer.SetFloat("Volume", -80f);
        }
    }

    public void PlayButtonClickSound() // Play sound when Player clicks a Button
    {
        buttonSFX.PlayOneShot(clickSound, 0.2f);
    }
}
