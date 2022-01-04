using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using System.Linq;
using UnityEngine.Events;

public class BallMovement : MonoBehaviour
{
    public static BallMovement Instance;

    #region VARIABLES
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private float stepDuration = 0.1f;
    [SerializeField] private LayerMask wallsAndRoadsLayer;
    private const float MAX_RAY_DISTANCE = 25f;

    private Vector3 ballYOffset = new Vector3(0f, 0.1f, 0f);

    [HideInInspector] public Vector3 moveDirection;
    [SerializeField]  public bool canMove = true;

    [SerializeField] public AudioSource ballAudio;
    [SerializeField] public AudioClip diamondSound, movePickUpSound;

    [SerializeField] public UnityAction<List<RoadTile>, float> onMoveStart;

    [SerializeField] public int moves;
    [SerializeField] public int movesLeft;

    [SerializeField] public int movesByPickup = 0;
    #endregion

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // change default ball position :
        transform.position = levelManager.defaultRoadTile.position + ballYOffset;

        GameManager.Instance.isLevelCompleted = false;

        movesLeft = moves;

        swipeListener.OnSwipe.AddListener(swipe =>
        {
            switch (swipe)
            {
                case "Right":
                    moveDirection = Vector3.right;
                    break;
                case "Left":
                    moveDirection = Vector3.left;
                    break;
                case "Up":
                    moveDirection = Vector3.forward;
                    break;
                case "Down":
                    moveDirection = Vector3.back;
                    break;
            }

            MoveBall();
        });
    }

    private void MoveBall()
    {
        if (GameManager.Instance.move_i.activeInHierarchy == true && moveDirection == Vector3.forward)
        {
            GameManager.Instance.move_i.SetActive(false);
            GameManager.Instance.Invoke("ActivateFillTutorial", 0.2f);
        }

        if (canMove)
        {
            canMove = false;
            // add raycast in the swipe direction (from the ball) :
            RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, MAX_RAY_DISTANCE, wallsAndRoadsLayer.value).OrderBy(h => h.distance).ToArray();

            Vector3 targetPosition = transform.position + ballYOffset;

            int steps = 0;

            List<RoadTile> pathRoadTiles = new List<RoadTile>();

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                { // Road tile
                  // add road tiles to the list to be painted:
                    pathRoadTiles.Add(hits[i].transform.GetComponent<RoadTile>());
                }
                else
                { // Wall tile
                    if (i == 0)
                    { // means wall is near the ball
                        canMove = true;
                        return;
                    }
                    //else:
                    steps = i;
                    targetPosition = hits[i - 1].transform.position + ballYOffset;
                    break;
                }
            }

            Debug.DrawLine(transform.position, targetPosition, Color.red, 2f);

            //move the ball to targetPosition:
            float moveDuration = stepDuration * steps;

            transform
               .DOMove(targetPosition, moveDuration / 3)
               .SetEase(Ease.Flash)
               .OnComplete(() => OnTweenComplete());

            if (onMoveStart != null)
                onMoveStart.Invoke(pathRoadTiles, moveDuration);

            GameManager.Instance.VibrateOnMove();

            if (movesLeft != 0)
            {
                movesLeft--;
            }

            if (movesByPickup != 0)
            {
                movesByPickup--;
            }

            GameManager.Instance.Invoke("GameOver", 0.2f);
        }
    }

    void OnTweenComplete()
    {
        canMove = true;
    }
}
