using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 自コンポーネント取得
    private ScoreManager scoreManager;
    private InputManager inputManager;
    private bool isTriggerDecide;

    // フラグ類
    private bool isStart;
    private bool isGameClear;
    private bool isGameOver;

    [Header("Character")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject bossObj;
    private PlayerManager playerManager;
    private BossManager bossManager;

    [Header("UI")]
    [SerializeField] private GameObject groupBeggining;
    [SerializeField] private GameObject groupIngame;
    [SerializeField] private GameObject groupGameClear;
    [SerializeField] private GameObject groupGameOver;
    [SerializeField] private GameObject groupResultScore;
    [SerializeField] private GameObject pushSpaceToTitle;

    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        inputManager = GetComponent<InputManager>();

        // Character
        playerManager = playerObj.GetComponent<PlayerManager>();
        bossManager = bossObj.GetComponent<BossManager>();
    }

    void Update()
    {
        Initialize();

        GetInput();

        ToStart();

        CheckGameClear();
        CheckGameOver();
        Result();
    }

    void Initialize()
    {
        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.RESET))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void ToStart()
    {
        if (!isStart && isTriggerDecide)
        {
            // Score
            scoreManager.SetIsActive(true);

            // UI
            groupBeggining.SetActive(false);
            groupIngame.SetActive(true);
            isStart = true;
        }
    }
    void CheckGameClear()
    {
        if (!isGameClear && !isGameOver)
        {
            // ボスの体力がなくなる
            if (bossManager.GetHp() <= 0f)
            {
                // Score
                scoreManager.SetIsActive(false);
                scoreManager.SetResultScore();

                // UI
                groupIngame.SetActive(false);
                groupGameClear.SetActive(true);
                groupResultScore.SetActive(true);
                isGameClear = true;
            }
        }
    }
    void CheckGameOver()
    {
        if (!isGameClear && !isGameOver)
        {
            // プレイヤーがボスよりも左にいる
            if (playerManager.GetAddMoveValue() <= bossManager.GetDiffValue())
            {
                // Boss
                bossManager.gameObject.SetActive(false);

                // Score
                scoreManager.SetIsActive(false);
                scoreManager.SetResultScore();

                // UI
                groupIngame.SetActive(false);
                groupGameOver.SetActive(true);
                groupResultScore.SetActive(true);
                isGameOver = true;
            }
        }
    }
    void Result()
    {
        if (isGameClear || isGameOver)
        {
            scoreManager.ResultScore();

            if (scoreManager.GetIsComplete() && isTriggerDecide)
            {
                pushSpaceToTitle.SetActive(true);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // Getter
    void GetInput()
    {
        isTriggerDecide = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerDecide = true;
        }
    }
}
