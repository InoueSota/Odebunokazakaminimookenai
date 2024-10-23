using UnityEngine;

public class RotateIgnore : MonoBehaviour
{
    [Header("‹——£‚É‰‚¶‚ÄƒJƒƒ‰‚ğˆø‚­")]
    [SerializeField] private PlayerManager playerManager;

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, -playerManager.GetAddRotationZ());
    }
}
