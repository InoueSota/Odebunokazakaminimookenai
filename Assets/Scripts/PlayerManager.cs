using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private InputManager inputManager;
    private bool isTriggerJump;

    // 座標系
    private Vector3 originPosition;
    private Vector3 nextPosition;

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
        inputManager = GetComponent<InputManager>();

        originPosition = transform.position;
        isJumping = false;
        isHovering = false;
        isGravity = false;
    }

    void Update()
    {
        GetInput();

        nextPosition = transform.position;

        Jump();
        Hovering();
        Gravity();

        transform.position = nextPosition;
    }

    void Jump()
    {
        // ジャンプ開始と初期化
        if (!isJumping && !isHovering && !isGravity && isTriggerJump)
        {
            jumpTarget = nextPosition.y + jumpDistance;
            isJumping = true;
        }

        // ジャンプ処理
        if (isJumping)
        {
            float deltaJumpSpeed = jumpSpeed * Time.deltaTime;
            nextPosition.y += (jumpTarget - nextPosition.y) * deltaJumpSpeed;

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
    void Hovering()
    {
        if (isHovering)
        {
            hangTimer -= Time.deltaTime;
            if (hangTimer <= 0f)
            {
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

    void GetInput()
    {
        isTriggerJump = false;

        if (inputManager.IsTrgger(InputManager.INPUTPATTERN.JUMP))
        {
            isTriggerJump = true;
        }
    }
}
