using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private InputManager inputManager;
    private bool isTriggerDecide;

    // �t���O��
    private bool isStart;
    private bool isGameClear;
    private bool isGameOver;

    [Header("UI")]
    [SerializeField] private GameObject groupBeggining;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        Initialize();

        GetInput();

        ToStart();
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
            groupBeggining.SetActive(false);
            isStart = true;
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
