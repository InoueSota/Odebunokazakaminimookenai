using UnityEngine;

public class MeterManager : MonoBehaviour
{
    // �p�����[�^�[�n
    private Vector3 nextPosition;
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private GameObject[] slots;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("��]�ړ�")]
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

        // �v���C���[������ʗ����ĉ~�^��
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
        // �ړ��ʂƉ�]�ʂ�����������
        transform.position = nextPosition;
        transform.localRotation = originRotation;
    }
    void SinMove()
    {
        // Target��sin�^��������
        angle += flowSpeed * Time.deltaTime;
        nextPosition.y += Mathf.Sin(angle) * range;
    }
    void Move()
    {
        // �v���C���[�Ƃ̍������������
        diffValue += (targetDiffValue - diffValue) * (chasePower * Time.deltaTime);
        // �v���C���[������ʗ����ĉ~�^��
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue(false) + playerManager.GetAddMoveValue() + diffValue);
    }
    public void SetSlot()
    {
        for (int i = 1; i < slots.Length + 1; i++)
        {
            if (slots[i - 1] && i <= playerManager.GetRemainingPower())
            {
                slots[i - 1].SetActive(true);
            }
            else
            {
                slots[i - 1].SetActive(false);
            }
        }
    }
}
