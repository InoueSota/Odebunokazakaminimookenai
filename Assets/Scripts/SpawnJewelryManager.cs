using UnityEngine;

public class SpawnJewelryManager : MonoBehaviour
{
    [Header("�����Ώ�")]
    [SerializeField] private GameObject jewelryPrefab;

    [Header("�����Ԋu")]
    [SerializeField] private float createIntervalMax;
    [SerializeField] private float createIntervalMin;
    private float createIntervalTimer;

    [Header("�����͈�")]
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
            // �����ʒu���J�������Ɏ��܂�͈͂Ń����_���ɐݒ肷��
            Vector3 createPosition = Vector3.zero;
            createPosition.y = Random.Range(createRange.x, createRange.y);

            // �ΏۃI�u�W�F�N�g�𐶐�
            GameObject jewelry = Instantiate(jewelryPrefab, createPosition, Quaternion.identity);

            // �����ʒu�����݂̉�]�ʂɍ��킹��
            jewelry.transform.RotateAround(groundTransform.position, Vector3.back, playerManager.GetMoveValue(true) + 45f);

            // �����Ԋu�������_���ɐݒ肷��
            createIntervalTimer = Random.Range(createIntervalMin, createIntervalMax);
        }
    }
}
