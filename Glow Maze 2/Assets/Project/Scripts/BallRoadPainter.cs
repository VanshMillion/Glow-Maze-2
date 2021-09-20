using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BallRoadPainter : MonoBehaviour
{
    public static BallRoadPainter Instance;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BallMovement ballMovement;
    [SerializeField] private MeshRenderer ballMeshRenderer;

    public int paintedRoadTiles = 0;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        //paint ball:
        ballMeshRenderer.material.SetColor("_BaseColor", levelManager.ballColor);
        ballMeshRenderer.material.SetColor("_EmissionColor", levelManager.ballColor);

        //paint default ball tile:
        Paint(levelManager.defaultRoadTile, .5f, 0f);

        //paint ball road :
        ballMovement.onMoveStart += OnBallMoveStartHandler;
    }

    private void OnBallMoveStartHandler(List<RoadTile> tileRoad, float totalDuration)
    {
        float stepDuration = totalDuration / tileRoad.Count;
        for (int i = 0; i < tileRoad.Count; i++)
        {
            RoadTile roadTile = tileRoad[i];
            if (!roadTile.isPainted)
            {
                float duration = totalDuration / 2f;
                float delay = i * (stepDuration / 4f);
                Paint(roadTile, duration, delay);

                //Check if Level Completed:
                if (paintedRoadTiles == levelManager.tileRoadList.Count)
                {
                    GameManager.Instance.isLevelCompleted = true;
                    Debug.Log("Level Completed");

                    LevelManager.Instance.Invoke("PlayWinFX", 0.3f);
                    GameManager.Instance.VibrateOnWinorLose();
                    if((SceneManager.GetActiveScene().buildIndex + 1) % 3 == 0)
                    {
                        AdmobManager.Instance.Invoke("ShowInterstitial", 1.3f);
                    }

                    Invoke("NextLevel", 1.2f);
                }
            }
        }
    }

    private void Paint(RoadTile tileRoad, float duration, float delay)
    {
        tileRoad.meshRenderer.material
           .DOColor(levelManager.paintColor, "_EmissionColor", duration)
           .SetDelay(delay);

        tileRoad.isPainted = true;
        paintedRoadTiles++;
    }

    private void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
