using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 自コンポーネント取得
    private Camera thisCamera;

    [Header("距離に応じてカメラを引く")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private float longMag;
    private float baseDistance;

    void Start()
    {
        thisCamera = GetComponent<Camera>();

        baseDistance = Vector3.Distance(playerTransform.position, groundTransform.position);
    }

    void LateUpdate()
    {
        CalcByDistance();
    }

    void CalcByDistance()
    {
        // 現在の距離を取得する
        float nowDistance = Vector3.Distance(playerTransform.position, groundTransform.position);

        thisCamera.fieldOfView = Mathf.Clamp(60f + (nowDistance - baseDistance) * longMag, 60f, 120f);
    }
}
