using UnityEngine;

public class BossManager : MonoBehaviour
{
    // パラメーター系
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("他オブジェクト取得")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("回転移動")]
    [SerializeField] private float chasePower;
    [SerializeField] private float targetDiffValue;
    private float diffValue;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.localRotation;

        // プレイヤーから一定量離して円運動
        diffValue = targetDiffValue;
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue() + diffValue);
    }

    void LateUpdate()
    {
        Return();

        Move();
    }

    void Return()
    {
        // 移動量と回転量を初期化する
        transform.position = originPosition;
        transform.localRotation = originRotation;
    }
    void Move()
    {
        // プレイヤーとの差を少しずつ作る
        diffValue += (targetDiffValue - diffValue) * (chasePower * Time.deltaTime);
        // プレイヤーから一定量離して円運動
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue() + diffValue);
    }
}
