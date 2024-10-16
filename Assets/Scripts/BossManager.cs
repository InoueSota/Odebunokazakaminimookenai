using UnityEngine;

public class BossManager : MonoBehaviour
{
    // �p�����[�^�[�n
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("��]�ړ�")]
    [SerializeField] private float chasePower;
    [SerializeField] private float targetDiffValue;
    private float diffValue;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.localRotation;

        // �v���C���[������ʗ����ĉ~�^��
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
        // �ړ��ʂƉ�]�ʂ�����������
        transform.position = originPosition;
        transform.localRotation = originRotation;
    }
    void Move()
    {
        // �v���C���[�Ƃ̍������������
        diffValue += (targetDiffValue - diffValue) * (chasePower * Time.deltaTime);
        // �v���C���[������ʗ����ĉ~�^��
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue() + diffValue);
    }
}
