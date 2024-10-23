using UnityEngine;

public class SpawnDustManager : MonoBehaviour
{
    [Header("�����Ώ�")]
    [SerializeField] private GameObject dustPrefab;

    [Header("�����Ԋu")]
    [SerializeField] private float createIntervalMax;
    [SerializeField] private float createIntervalMin;
    private float createIntervalTimer;

    [Header("�����͈�")]
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
            // �����ʒu���J�������Ɏ��܂�͈͂Ń����_���ɐݒ肷��
            Vector3 createPosition = Vector3.zero;
            createPosition.x = Random.Range(-createRange.x, createRange.x);
            createPosition.y = Random.Range(-createRange.y, createRange.y);

            // �ΏۃI�u�W�F�N�g�𐶐�
            GameObject dustParticle = Instantiate(dustPrefab, createPosition, Quaternion.identity);

            // �����ʒu�����݂̉�]�ʂɍ��킹��
            dustParticle.transform.RotateAround(groundTransform.position, Vector3.back, playerManager.GetMoveValue(true));

            // �������̕��ɏ����ړ�������
            Vector3 toPlayer = (playerTransform.position - cloudTransform.position).normalized;
            dustParticle.GetComponent<DustManager>().Initialize(toPlayer);

            // �����Ԋu�������_���ɐݒ肷��
            createIntervalTimer = Random.Range(createIntervalMin, createIntervalMax);
        }
    }
}
