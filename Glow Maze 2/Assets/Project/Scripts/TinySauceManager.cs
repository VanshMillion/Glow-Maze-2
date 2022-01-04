//using UnityEngine;

//public class TinySauceManager : MonoBehaviour
//{
//    public static TinySauceManager Instance;

//    private void awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(this.gameObject);
//        }

//        DontDestroyOnLoad(this);
//    }

//    public void OnLevelStarted(int _lvl)
//    {
//        TinySauce.OnGameStarted(levelNumber: _lvl.ToString()) ;
//    }

//    public void OnLevelFinished(bool islevelcompleted, float score, int _lvl)
//    {
//        TinySauce.OnGameFinished(islevelcompleted, score, levelNumber: _lvl.ToString());
//    }
//}
