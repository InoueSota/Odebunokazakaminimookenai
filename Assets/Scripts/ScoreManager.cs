using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // フラグ類
    private bool isActive;

    // 増加対象（スコア）
    private int score;

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text resultText;

    [Header("増加量")]
    [SerializeField] private float autoAddIntervalTime;
    private float autoAddIntervalTimer;

    [Header("Result")]
    [SerializeField] private float scoreTime;
    private float scoreTimer;
    private int targetScore;
    private bool isComplete;

    void Start()
    {
        autoAddIntervalTimer = autoAddIntervalTime;
    }

    void LateUpdate()
    {
        AutoScore();

        // ScoreをUIに適用する（８桁）
        scoreText.text = string.Format("{0:00000000}", score);
    }
    void AutoScore()
    {
        if (isActive)
        {
            autoAddIntervalTimer -= Time.deltaTime;

            if (autoAddIntervalTimer <= 0f)
            {
                score++;
                autoAddIntervalTimer = autoAddIntervalTime;
            }
        }
    }

    public void ResultScore()
    {
        scoreTimer -= Time.deltaTime;
        scoreTimer = Mathf.Clamp(scoreTimer, 0f, scoreTime);
        float t = scoreTimer / scoreTime;
        score = (int)Mathf.Lerp(targetScore, 0, t * t);

        if (scoreTimer <= 0f) { isComplete = true; }

        resultText.text = string.Format("{0:00000000}", score);
    }

    // Getter
    public bool GetIsComplete()
    {
        return isComplete;
    }

    // Setter
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }
    public void SetResultScore()
    {
        targetScore = score;
        scoreTimer = scoreTime;
    }
}
