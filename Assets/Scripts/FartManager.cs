using UnityEngine;

public class FartManager : MonoBehaviour
{
    private GameObject hitObj;

    private float damageValue;

    public void Initialize(float _damageValue)
    {
        hitObj = null;
        damageValue = _damageValue;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!hitObj && other.CompareTag("Boss"))
        {
            other.GetComponent<BossManager>().Damage(damageValue);
            hitObj = other;
        }
    }
}
