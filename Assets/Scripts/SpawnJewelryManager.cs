using UnityEngine;

public class SpawnJewelryManager : MonoBehaviour
{
    [Header("生成対象")]
    [SerializeField] private GameObject jewelryPrefab;

    [Header("生成間隔")]
    [SerializeField] private float createIntervalMax;
    [SerializeField] private float createIntervalMin;
    private float createIntervalTimer;

    [Header("生成範囲")]
    [SerializeField] private Vector2 createRange;

    [Header("Player")]
    [SerializeField] private PlayerManager playerManager;
    private Transform playerTransform;

    [Header("Ground")]
    [SerializeField] private Transform groundTransform;

    void Start()
    {
        playerTransform = playerManager.transform;
    }

    void LateUpdate()
    {
        createIntervalTimer -= Time.deltaTime;

        if (createIntervalTimer <= 0f)
        {
            // 生成位置をカメラ内に収まる範囲でランダムに設定する
            Vector3 createPosition = Vector3.zero;
            createPosition.y = Random.Range(createRange.x, createRange.y);

            // 対象オブジェクトを生成
            GameObject jewelry = Instantiate(jewelryPrefab, createPosition, Quaternion.identity);

            // 生成位置を現在の回転量に合わせる
            jewelry.transform.RotateAround(groundTransform.position, Vector3.back, playerManager.GetMoveValue(true) + 45f);

            // 生成間隔をランダムに設定する
            createIntervalTimer = Random.Range(createIntervalMin, createIntervalMax);
        }
    }
}
