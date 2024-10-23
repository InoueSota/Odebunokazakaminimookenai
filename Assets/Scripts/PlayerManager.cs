using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerAttackManager attackManager;
    private InputManager inputManager;
    private bool isTriggerJump;
    private bool isReleaseJump;
    private bool isPushJump;

    [Header("他オブジェクト取得")]
    [SerializeField] private MeterManager meterManager;

    // 座標系
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 nextPosition;

    [Header("回転移動")]
    [SerializeField] private GameObject ground;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followDiffValue;
    private float moveValue;

    [Header("チャージ")]
    [SerializeField] private float addRotateValue;
    [SerializeField] private float rotationChasePower;
    private float addRotationZ;
    private float targetRotationZ;
    private int remainingPower;
    private int usePowerValue;
    private bool isCharging;

    [Header("ブースト")]
    [SerializeField] private Vector3 boostBase;
    [SerializeField] private float boostTime;
    private Vector3 boostTarget;
    private float addMoveValue;
    private int boostLevel;
    private bool isBoosting;

    [Header("ジャンプ")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    private bool isJumping;
    private float jumpTarget;

    [Header("滞空")]
    [SerializeField] private float hangTime;
    private bool isHovering;
    private float hangTimer;

    [Header("重力")]
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
        // 座標系初期化
        originPosition = transform.position;
        nextPosition = originPosition;
        originRotation = transform.localRotation;

        // フラグ類初期化
        isJumping = false;
        isHovering = false;
        isGravity = false;

        // パラメーター初期化
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
        // 移動量と回転量を初期化する
        transform.localRotation = originRotation;
    }
    void CheckAction()
    {
        // 地面にいるとき
        if (GetIsGround() && isTriggerJump)
        {
            jumpTarget = nextPosition.y + jumpDistance;
            isHovering = false;
            isGravity = false;
            isJumping = true;
        }
        // 空中ジャンプチャージ
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
        // ジャンプ処理
        if (isJumping)
        {
            nextPosition.y += (jumpTarget - nextPosition.y) * (jumpSpeed * Time.deltaTime);

            // ジャンプ終了処理
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

            // 空中ジャンプ
            if (isReleaseJump)
            {
                // Vector3.right（単位ベクトル）を回転させる
                Vector3 moveDirection = Quaternion.Euler(0f, 0f, targetRotationZ + 90f) * Vector3.right;

                // パワーを消費する
                remainingPower -= usePowerValue;
                meterManager.SetSlot();

                // BoostLevelを調整
                boostLevel = usePowerValue;

                // newBoostには回転させたboostBaseを代入する
                Vector3 newBoost = Vector3.zero;
                newBoost.x = moveDirection.x * (boostBase.x * boostLevel);
                newBoost.y = moveDirection.y * (boostBase.y * boostLevel);

                // 現在の移動量に加算したnewBoostをboostTargetに代入する
                boostTarget.x = newBoost.x + addMoveValue;
                boostTarget.y = newBoost.y + nextPosition.y;

                Boost();

                // おなら
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
