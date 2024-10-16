using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 自コンポーネント取得
    private Camera thisCamera;

    // 他コンポーネント取得
    private PlayerManager playerManager;

    [Header("距離に応じてカメラを引く")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private float longMag;
    private float baseDistance;

    // 回転量を無視する
    private Transform rotateIgnoreTransform;

    void Start()
    {
        thisCamera = GetComponent<Camera>();

        playerManager = playerTransform.GetComponent<PlayerManager>();
        baseDistance = Vector3.Distance(playerTransform.position, groundTransform.position);

        rotateIgnoreTransform = transform.parent.transform;
    }

    void LateUpdate()
    {
        CalcByDistance();
        RotateIgnore();
    }

    void CalcByDistance()
    {
        // 現在の距離を取得する
        float nowDistance = Vector3.Distance(playerTransform.position, groundTransform.position);

        thisCamera.fieldOfView = Mathf.Clamp(60f + (nowDistance - baseDistance) * longMag, 60f, 120f);
    }
    void RotateIgnore()
    {
        rotateIgnoreTransform.localRotation = Quaternion.Euler(0f, 0f, -playerManager.GetAddRotationZ());
    }
}
