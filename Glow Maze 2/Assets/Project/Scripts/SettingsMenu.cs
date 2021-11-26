using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

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
    public TMP_Text resumeInformText2;

    public GameObject pausePanel;
    [SerializeField] public GameObject cautionPanel;
    public TMP_Text versionText;

    public AudioSource buttonSFX;
    public AudioClip clickSound;

    public Animator fadeAnim;
    public Animator resumeInform;
    [SerializeField] Animator resumeInform2;

    private string privacyLink = "https://sites.google.com/view/glowmaze/privacy-policy";

    private string facebookLink = "https://www.facebook.com/vanmillionstudios";
    private string twitterLink = "https://twitter.com/VanmillionSt_";
    private string mail = "https://sites.google.com/view/vanmillionstudios/contacts";
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

        if(pausePanel.activeInHierarchy == true)
        {
            BallMovement.Instance.canMove = false;
        }
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        BallMovement.Instance.canMove = true;
    }

    public void OpenCautionPanel()
    {
        cautionPanel.SetActive(true);
    }

    public void CloseCautionPanel()
    {
        cautionPanel.SetActive(false);
    }

    public void Restart()
    {
        ClosePausePanel();
        GameManager.Instance.DeductDiamonds();
        GameManager.Instance.RestartLevel();
    }

    public void RestoreGame()
    {
        OpenCautionPanel();
    }

    public void YestoRestoreGame()
    {
        AdsManager.Instance.ShowRewardedAd();

        if (!Advertisement.IsReady(AdsManager.Instance.rewarded_Android))
        {
            StartNoAdLoadedAnim2();
        }
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
            GameManager.Instance.isGameOver = false;
            BallMovement.Instance.canMove = true;
        }
        else
        {
            resumeInformText.text = "Not enough diamonds \n Starting video AD";
            resumeInform.SetTrigger("NotEnoughDiamond");
            AdsManager.Instance.Invoke("ShowRewardedAd", 0.8f);

            if (!Advertisement.IsReady(AdsManager.Instance.rewarded_Android))
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

    void StartNoAdLoadedAnim2()
    {
        Debug.Log("NO ADS LOADED!");
        resumeInformText2.text = "No AD available \n Check Internet connection!";
        resumeInform2.SetTrigger("NotEnoughDiamond");
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

    public void OpenPrivacy()
    {
        Application.OpenURL(privacyLink);
    }

    public void OpenFacebook()
    {
        Application.OpenURL(facebookLink);
    }

    public void OpenTwitter()
    {
        Application.OpenURL(twitterLink);
    }

    public void OpenMailList()
    {
        Application.OpenURL(mail);
    }

    public void PlayButtonClickSound() // Play sound when Player clicks a Button
    {
        buttonSFX.PlayOneShot(clickSound, 0.2f);
    }
}
