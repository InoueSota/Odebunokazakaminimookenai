using UnityEngine;

public class RotateIgnore : MonoBehaviour
{
    [Header("距離に応じてカメラを引く")]
    [SerializeField] private PlayerManager playerManager;

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, -playerManager.GetAddRotationZ());
    }
}
