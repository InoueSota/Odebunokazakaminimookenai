using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    // ���W�n
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("�̗͌n")]
    [SerializeField] private float maxHp;
    private float hp;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private float healIntervalTime;
    private float healIntervalTimer;

    [Header("��]�ړ�")]
    [SerializeField] private float chasePower;
    [SerializeField] private float targetDiffValue;
    private float diffValue;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.localRotation;

        hp = maxHp;

        // �v���C���[������ʗ����ĉ~�^��
        diffValue = targetDiffValue;
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue(false) + diffValue);
    }

    void LateUpdate()
    {
        Return();

        Move();

        Hp();
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
        transform.RotateAround(ground.transform.position, Vector3.back, playerManager.GetMoveValue(false) + diffValue);
    }
    void Hp()
    {
        hpSlider.value = Mathf.Clamp(hp / maxHp, 0f, 1f);

        healIntervalTimer -= Time.deltaTime;
        if (healIntervalTimer <= 0f)
        {
            hp++;
            hp = Mathf.Clamp(hp, 0f, maxHp);
            healIntervalTimer = healIntervalTime;
        }
    }

    // Getter
    public float GetDiffValue()
    {
        return diffValue;
    }
    public float GetHp()
    {
        return hp;
    }

    // Setter
    public void Damage(float _damageValue)
    {
        hp -= _damageValue;
    }
}
