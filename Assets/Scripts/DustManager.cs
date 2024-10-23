using DG.Tweening;
using UnityEngine;

public class DustManager : MonoBehaviour
{
    [Header("�ړ���")]
    [SerializeField] private float moveRange;

    [Header("�ړ�����")]
    [SerializeField] private float moveTime;

    [Header("�ړ��^�C�v")]
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
