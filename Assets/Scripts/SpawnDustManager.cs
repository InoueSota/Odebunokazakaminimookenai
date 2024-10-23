using UnityEngine;

public class SpawnDustManager : MonoBehaviour
{
    [Header("生成対象")]
    [SerializeField] private GameObject dustPrefab;

    [Header("生成間隔")]
    [SerializeField] private float createIntervalMax;
    [SerializeField] private float createIntervalMin;
    private float createIntervalTimer;

    [Header("生成範囲")]
    [SerializeField] private Vector2 createRange;

    [Header("Player")]
    [SerializeField] private PlayerManager playerManager;
    private Transform playerTransform;

    [Header("Cloud")]
    [SerializeField] private Transform cloudTransform;

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
            createPosition.x = Random.Range(-createRange.x, createRange.x);
            createPosition.y = Random.Range(-createRange.y, createRange.y);

            // 対象オブジェクトを生成
            GameObject dustParticle = Instantiate(dustPrefab, createPosition, Quaternion.identity);

            // 生成位置を現在の回転量に合わせる
            dustParticle.transform.RotateAround(groundTransform.position, Vector3.back, playerManager.GetMoveValue(true));

            // 風向きの方に少し移動させる
            Vector3 toPlayer = (playerTransform.position - cloudTransform.position).normalized;
            dustParticle.GetComponent<DustManager>().Initialize(toPlayer);

            // 生成間隔をランダムに設定する
            createIntervalTimer = Random.Range(createIntervalMin, createIntervalMax);
        }
    }
}
