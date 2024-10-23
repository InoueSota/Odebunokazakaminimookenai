using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerManager playerManager;

    [Header("Prefab�擾")]
    [SerializeField] private GameObject fartPrefab;

    [Header("�U����")]
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
