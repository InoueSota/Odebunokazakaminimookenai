using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerAttackManager attackManager;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isReleaseJump;
    private bool isPushJump;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private MeterManager meterManager;

    // ���W�n
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 nextPosition;

    [Header("��]�ړ�")]
    [SerializeField] private GameObject ground;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followDiffValue;
    private float moveValue;

    [Header("�`���[�W")]
    [SerializeField] private float addRotateValue;
    [SerializeField] private float rotationChasePower;
    private float addRotationZ;
    private float targetRotationZ;
    private int remainingPower;
    private int usePowerValue;
    private bool isCharging;

    [Header("�u�[�X�g")]
    [SerializeField] private Vector3 boostBase;
    [SerializeField] private float boostTime;
    private Vector3 boostTarget;
    private float addMoveValue;
    private int boostLevel;
    private bool isBoosting;

    [Header("�W�����v")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    private bool isJumping;
    private float jumpTarget;

    [Header("�؋�")]
    [SerializeField] private float hangTime;
    private bool isHovering;
    private float hangTimer;

    [Header("�d��")]
    [SerializeField] private float gravityMax;
    [SerializeField] private float addGravity;
    private bool isGravity;
    private float gravityPower;

    void Start()
    {
        attackManager = GetComponent<PlayerAttackManager>();
        inputManager = GetComponent<InputManager>();

        Initialize();
    }
    void Initialize()
    {
        // ���W�n������
        originPosition = transform.position;
        nextPosition = originPosition;
        originRotation = transform.localRotation;

        // �t���O�ޏ�����
        isJumping = false;
        isHovering = false;
        isGravity = false;

        // �p�����[�^�[������
        remainingPower = 10;
    }

    void Update()
    {
        GetInput();

        Return();

        CheckAction();
        Jump();
        Charge();
        Hovering();
        Gravity();

        transform.position = nextPosition;

        RotateMove();
    }

    void Return()
    {
        // �ړ��ʂƉ�]�ʂ�����������
        transform.localRotation = originRotation;
    }
    void CheckAction()
    {
        // �n�ʂɂ���Ƃ�
        if (GetIsGround() && isTriggerJump)
        {
            jumpTarget = nextPosition.y + jumpDistance;
            isHovering = false;
            isGravity = false;
            isJumping = true;
        }
        // �󒆃W�����v�`���[�W
        else if (!GetIsGround() && 1 <= remainingPower && !isJumping && !isCharging && isPushJump)
        {
            targetRotationZ = 0f;
            isHovering = false;
            isGravity = false;
            isCharging = true;
        }
    }
    void Jump()
    {
        // �W�����v����
        if (isJumping)
        {
            nextPosition.y += (jumpTarget - nextPosition.y) * (jumpSpeed * Time.deltaTime);

            // �W�����v�I������
            if (Mathf.Abs(jumpTarget - nextPosition.y) < 0.03f)
            {
                nextPosition.y = jumpTarget;

                hangTimer = hangTime;
                isHovering = true;
                isJumping = false;
            }
        }
    }
    void Charge()
    {
        if (isCharging && !isBoosting)
        {
            targetRotationZ += addRotateValue * Time.deltaTime;

            usePowerValue = (int)-targetRotationZ / 360 + 1;
            usePowerValue = Mathf.Clamp(usePowerValue, 0, remainingPower);

            // �󒆃W�����v
            if (isReleaseJump)
            {
                // Vector3.right�i�P�ʃx�N�g���j����]������
                Vector3 moveDirection = Quaternion.Euler(0f, 0f, targetRotationZ + 90f) * Vector3.right;

                // �p���[�������
                remainingPower -= usePowerValue;
                meterManager.SetSlot();

                // BoostLevel�𒲐�
                boostLevel = usePowerValue;

                // newBoost�ɂ͉�]������boostBase��������
                Vector3 newBoost = Vector3.zero;
                newBoost.x = moveDirection.x * (boostBase.x * boostLevel);
                newBoost.y = moveDirection.y * (boostBase.y * boostLevel);

                // ���݂̈ړ��ʂɉ��Z����newBoost��boostTarget�ɑ������
                boostTarget.x = newBoost.x + addMoveValue;
                boostTarget.y = newBoost.y + nextPosition.y;

                Boost();

                // ���Ȃ�
                attackManager.AttackFart();

                isBoosting = true;
            }
        }

        addRotationZ += (targetRotationZ - addRotationZ) * (rotationChasePower * Time.deltaTime);
    }
    void Boost()
    {
        DOTween.To(() => addMoveValue, (x) => addMoveValue = x, boostTarget.x, boostTime);
        DOTween.To(() => nextPosition.y, (x) => nextPosition.y = x, boostTarget.y, boostTime).OnComplete(BoostComplete);
    }
    void BoostComplete()
    {
        addMoveValue = boostTarget.x;
        nextPosition.y = boostTarget.y;
        targetRotationZ = 0f;

        hangTimer = hangTime;
        isHovering = true;
        isCharging = false;
        isBoosting = false;
    }
    void Hovering()
    {
        if (isHovering)
        {
            hangTimer -= Time.deltaTime;
            if (hangTimer <= 0f)
            {
                gravityPower = 0f;
                isGravity = true;
                isHovering = false;
            }
        }
    }
    void Gravity()
    {
        if (isGravity)
        {
            gravityPower += addGravity * Time.deltaTime;

            float deltaGravityPower = gravityPower * Time.deltaTime;
            nextPosition.y -= deltaGravityPower;

            if (nextPosition.y < originPosition.y)
            {
                nextPosition.y = originPosition.y;
                gravityPower = 0f;
                isGravity = false;
            }
        }
    }
    void RotateMove()
    {
        moveValue += moveSpeed * Time.deltaTime;
        transform.RotateAround(ground.transform.position, Vector3.back, moveValue + addMoveValue);
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + addRotationZ);
    }

    // Getter
    void GetInput()
    {
        isTriggerJump = false;
        isReleaseJump = false;
        isPushJump = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
        if (inputManager.IsRelease(InputManager.INPUTPATTERN.JUMP))
        {
            isReleaseJump = true;
        }
        if (inputManager.IsPush(InputManager.INPUTPATTERN.JUMP))
        {
            isPushJump = true;
        }
    }
    bool GetIsGround()
    {
        if (isJumping || isHovering || isGravity || isBoosting)
        {
            return false;
        }
        return true;
    }
    public float GetMoveValue(bool _isFollow)
    {
        if (_isFollow)
        {
            return moveValue + addMoveValue;
        }
        return moveValue;
    }
    public float GetClampDiffValue(float _diffValue)
    {
        if (addMoveValue >= followDiffValue)
        {
            return addMoveValue - followDiffValue;
        }
        else if (addMoveValue <= -followDiffValue)
        {
            return addMoveValue + followDiffValue;
        }
        return _diffValue;
    }
    public float GetAddMoveValue()
    {
        return addMoveValue;
    }
    public float GetAddRotationZ()
    {
        return addRotationZ;
    }
    public int GetRemainingPower()
    {
        return remainingPower;
    }
    public int GetBoostLevel()
    {
        return boostLevel;
    }
}
