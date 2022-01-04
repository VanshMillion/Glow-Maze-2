using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.Advertisements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    #region VARIABLES

    [HideInInspector] public int reachedLevel;

    [SerializeField] private Transform diamondParent;

    [SerializeField] public Light ballLight;

    [Header("Level texture")]
    [SerializeField] private Texture2D levelTexture;

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject wallTilePrefab;
    [SerializeField] private GameObject roadTilePrefab;

    [Space]
    [Header("Materials and Colors")]
    [SerializeField] public Material roadMat;
    [SerializeField] public Material wallMat;

    public Material panelMat;

    [Space]
    [SerializeField] public Color roadColor;
    [SerializeField] public Color wallColor;
    [SerializeField] public Color camColor;

    [Header("Ball and Road paint color")]
    [SerializeField] public Color ballColor;
    [SerializeField] public Color paintColor;

    [HideInInspector] public List<RoadTile> tileRoadList = new List<RoadTile>();
    [HideInInspector] public RoadTile defaultRoadTile;

    private Color wallPosColor = Color.white;
    private Color roadPosColor = Color.black;

    private float unitPerPixel;

    [SerializeField] public int diamondstoRemove;

    [SerializeField] public int diamondPickUpCount;

    [SerializeField] private ParticleSystem winFX;

    [SerializeField] public TMP_Text addedMovesText;

    [SerializeField] public Animator addedMovesAnim;

    [SerializeField] public AudioSource levelAudio;
    [SerializeField] public AudioClip winSound;
    #endregion

    void Awake()
    {
        Generate();

        //assign first road tile as default poition for the ball:
        defaultRoadTile = tileRoadList[0];

        LevelColorManager();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        reachedLevel = SceneManager.GetActiveScene().buildIndex; //Set reachedLevel to Current Scene's buildindex

        GameManager.Instance.isGameOver = false;
        //TinySauceManager.Instance.OnLevelStarted(reachedLevel);

        FadeAtStart();

        PlayerPrefs.SetInt("SavedLevel", reachedLevel);
        Debug.Log("LEVEL SAVED! " + reachedLevel);

        diamondPickUpCount = diamondParent.childCount;
    }

    private void Generate()
    {
        unitPerPixel = wallTilePrefab.transform.lossyScale.x;
        float halfUnitPerPixel = unitPerPixel / 2f;

        float width = levelTexture.width;
        float height = levelTexture.height;

        Vector3 offset = (new Vector3(width / 2f, 0f, height / 2f) * unitPerPixel)
                         - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Get pixel color :
                Color pixelColor = levelTexture.GetPixel(x, y);

                Vector3 spawnPos = ((new Vector3(x, 0f, y) * unitPerPixel) - offset);

                if (pixelColor == wallPosColor)
                    Spawn(wallTilePrefab, spawnPos);
                else if (pixelColor == roadPosColor)
                    Spawn(roadTilePrefab, spawnPos);
            }
        }
    }

    private void Spawn(GameObject tilePrefab, Vector3 position)
    {
        //fix Y position:
        position.y = tilePrefab.transform.position.y;

        GameObject obj = Instantiate(tilePrefab, position, Quaternion.identity, transform);

        if (tilePrefab == roadTilePrefab)
            tileRoadList.Add(obj.GetComponent<RoadTile>());
    }

    public void FadeAtStart() // FadeOut fadePanel on Start
    {
        SettingsMenu.Instance.fadeAnim.SetTrigger("DoFade");
    }

    public void PlayWinFX()
    {
        UIManager.Instance.LevelCompletedUI();

        winFX.Play();
        levelAudio.PlayOneShot(winSound, 1f);
    }

    void LevelColorManager()
    {
        Camera.main.backgroundColor = camColor;
        ballLight.color = ballColor;

        roadMat.color = roadColor;
        wallMat.color = wallColor;

        panelMat.color = camColor;
    }

    void OnValidate()
    {
        LevelColorManager();
    }
}
