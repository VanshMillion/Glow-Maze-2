using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAManager : MonoBehaviour
{
    public static GAManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
    }

    public void OnLevelCompleted(int _level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LEVEL " + _level);
        Debug.Log("LEVEL " + _level);
    }
}
