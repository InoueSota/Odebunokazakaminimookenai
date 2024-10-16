using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isReleaseJump;
    private bool isPushJump;

    // �p�����[�^�[�n
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 nextPosition;

    [Header("��]�ړ�")]
    [SerializeField] private GameObject ground;
    [SerializeField] private float moveSpeed;
    private float moveValue;

    [Header("�`���[�W")]
    [SerializeField] private float chargeTime;
    [SerializeField] private float addRotationZ;
    private float chargeTimer;
    private bool isCharging;

    [Header("�u�[�X�g")]
    [SerializeField] private Vector2 boostBase;
    [SerializeField] private float boostSpeed;
    private Vector2 boostTarget;
    private float addMoveValue;
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
        inputManager = GetComponent<InputManager>();

        originPosition = transform.position;
        nextPosition = originPosition;
        originRotation = transform.localRotation;
        isJumping = false;
        isHovering = false;
        isGravity = false;
    }

    void Update()
    {
        GetInput();

        Return();

        nextPosition = transform.position;

        Jump();
        Charge();
        Boost();
        Hovering();
        Gravity();

        transform.position = nextPosition;

        RotateMove();
    }

    void Return()
    {
        // �ړ��ʂƉ�]�ʂ�����������
        transform.position = nextPosition;
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
        else if (!GetIsGround() && !isJumping && isPushJump)
        {
            isGravity = false;
            isCharging = true;
        }
    }
    void Jump()
    {
        // �W�����v����
        if (isJumping)
        {
            float deltaJumpSpeed = jumpSpeed * Time.deltaTime;
            nextPosition.y += (jumpTarget - nextPosition.y) * deltaJumpSpeed;

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


            // �󒆃W�����v
            if (isReleaseJump)
            {
                boostTarget.x = boostBase.x + addMoveValue;
                boostTarget.y = boostBase.y + nextPosition.y;
                isBoosting = true;
            }
        }
    }
    void Boost()
    {
        if (isBoosting)
        {
            float deltaBoostSpeed = boostSpeed * Time.deltaTime;

            // X������
            addMoveValue += (boostTarget.x - addMoveValue) * deltaBoostSpeed;
            // Y������
            nextPosition.y += (boostTarget.y - nextPosition.y) * deltaBoostSpeed;

            // �u�[�X�g�I������
            if (Mathf.Abs(boostTarget.y - nextPosition.y) < 0.03f)
            {
                addMoveValue = boostTarget.x;
                nextPosition.y = boostTarget.y;

                hangTimer = hangTime;
                isHovering = true;
                isCharging = false;
                isBoosting = false;
            }
        }
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
        addRotationZ += 15 * Time.deltaTime;
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
    public float GetMoveValue()
    {
        return moveValue;
    }
    public float GetAddRotationZ()
    {
        return addRotationZ;
    }
}
