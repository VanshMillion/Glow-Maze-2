using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text movesLeftText;

    public Animator levelCompleteAnim;

    private int currentLevel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;

        UpdateUI();
    }

    void Update()
    {
        movesLeftText.text = BallMovement.Instance.movesLeft.ToString();
    }

    void UpdateUI()
    {
        currentLevelText.text = ("LEVEL " + currentLevel);
    }

    public void LevelCompletedUI()
    {
        levelCompleteAnim.SetTrigger("LevelFade");
    }
}
