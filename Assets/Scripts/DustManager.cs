using DG.Tweening;
using UnityEngine;

public class DustManager : MonoBehaviour
{
    [Header("移動量")]
    [SerializeField] private float moveRange;

    [Header("移動時間")]
    [SerializeField] private float moveTime;

    [Header("移動タイプ")]
    [SerializeField] private Ease easeType;

    public void Initialize(Vector3 _toPlayer)
    {
        Vector3 targetPosition = _toPlayer * moveRange;
        transform.DOMove(transform.position + targetPosition, moveTime).SetEase(easeType).OnComplete(DestroySelf);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
