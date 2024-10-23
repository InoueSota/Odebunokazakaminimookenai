using UnityEngine;

public class WindZoneManager : MonoBehaviour
{
    [Header("他オブジェクト取得")]
    [SerializeField] private Transform topTransform;
    [SerializeField] private Transform bottomTransform;
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;

    [Header("ランダムで風向き変更")]
    [SerializeField] private float randomChangeMax;
    [SerializeField] private float randomChangeMin;
    private float changeIntervalTimer;
    private float changeIntervalTime;
    private Transform windDirectionTransform;
    private int windDirectionNumber;

    void Start()
    {
        changeIntervalTimer = randomChangeMax;
        windDirectionTransform = leftTransform;
        windDirectionNumber = 2;
    }

    void Update()
    {
        ChangeWindDirection();

        // ランダムで設定された風向きに風を吹かせる
        transform.LookAt(windDirectionTransform.position);
    }
    void ChangeWindDirection()
    {
        changeIntervalTimer -= Time.deltaTime;

        if (changeIntervalTimer <= 0f)
        {
            /*
            int randomWindDirectionNumber = Random.Range(2, 3);

            // 必ず風向きを変える
            while (randomWindDirectionNumber == windDirectionNumber)
            {
                randomWindDirectionNumber = Random.Range(2, 3);
            }
            
            // 変わった風向きを取得
            windDirectionNumber = randomWindDirectionNumber;
            if (windDirectionNumber == 0)
            {
                windDirectionTransform = topTransform;
            }
            else if (windDirectionNumber == 1)
            {
                windDirectionTransform = bottomTransform;
            }
            else if (windDirectionNumber == 2)
            {
                windDirectionTransform = leftTransform;
            }
            else
            {
                windDirectionTransform = rightTransform;
            }
            */

            if (windDirectionNumber == 2)
            {
                windDirectionTransform = rightTransform;
                windDirectionNumber = 3;
            }
            else
            {
                windDirectionTransform = leftTransform;
                windDirectionNumber = 2;
            }

            changeIntervalTimer = Random.Range(randomChangeMin, randomChangeMax);
        }
    }
}
