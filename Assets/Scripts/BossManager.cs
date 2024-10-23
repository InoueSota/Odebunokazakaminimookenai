using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    // 座標系
    private Vector3 originPosition;
    private Quaternion originRotation;

    [Header("他オブジェクト取得")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject ground;

    [Header("体力系")]
    [SerializeField] private float maxHp;
    private float hp;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private float healIntervalTime;
    private float healIntervalTimer;

    [Header("回転移動")]
    [SerializeField] private float chasePower;
    [SerializeField] private float targetDiffValue;
    private float diffValue;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.localRotation;

        hp = maxHp;

        // プレイヤーから一定量離して円運動
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
        // 移動量と回転量を初期化する
        transform.position = originPosition;
        transform.localRotation = originRotation;
    }
    void Move()
    {
        // プレイヤーとの差を少しずつ作る
        diffValue += (targetDiffValue - diffValue) * (chasePower * Time.deltaTime);
        // プレイヤーから一定量離して円運動
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
