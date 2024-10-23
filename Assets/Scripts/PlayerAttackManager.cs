using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerManager playerManager;

    [Header("Prefab取得")]
    [SerializeField] private GameObject fartPrefab;

    [Header("攻撃力")]
    [SerializeField] private float baseDamage;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    void Update()
    {
        
    }
    public void AttackFart()
    {
        GameObject fart = Instantiate(fartPrefab, transform.position, Quaternion.identity);

        fart.GetComponent<FartManager>().Initialize(playerManager.GetBoostLevel() * baseDamage);
    }
}
