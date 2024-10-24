using UnityEngine;

public class CloudManager : MonoBehaviour
{
    // パラメーター系
    private Vector3 nextPosition;
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("他オブジェクト取得")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("回転移動")]
    [SerializeField] private float chasePower;
    [SerializeField] private float targetDiffValue;
    private float diffValue;

    [Header("Sin")]
    [SerializeField] private float flowSpeed;
    [SerializeField] private float range;
    private float angle;

    void Start()
    {
        nextPosition = transform.position;
        originPosition = transform.position;
        originRotation = transform.localRotation;

        // プレイヤーから一定量離して円運動
        diffValue = playerManager.GetClampDiffValue(targetDiffValue);
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue(false) + diffValue);
    }

    void LateUpdate()
    {
        Return();

        SinMove();

        transform.position = nextPosition;

        Move();
    }

    void Return()
    {
        // 移動量と回転量を初期化する
        transform.position = nextPosition;
        transform.localRotation = originRotation;
    }
    void SinMove()
    {
        // Targetをsin運動させる
        angle += flowSpeed * Time.deltaTime;
        nextPosition.y += Mathf.Sin(angle) * range;
    }
    void Move()
    {
        // プレイヤーとの差を少しずつ作る
        diffValue += (playerManager.GetClampDiffValue(targetDiffValue) - diffValue) * (chasePower * Time.deltaTime);
        // プレイヤーから一定量離して円運動
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue(false) + diffValue);
    }
}
