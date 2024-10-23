using UnityEngine;

public class RotateIgnore : MonoBehaviour
{
    [Header("�����ɉ����ăJ����������")]
    [SerializeField] private PlayerManager playerManager;

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, -playerManager.GetAddRotationZ());
    }
}
