using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private Camera thisCamera;

    [Header("�����ɉ����ăJ����������")]
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
        // ���݂̋������擾����
        float nowDistance = Vector3.Distance(playerTransform.position, groundTransform.position);

        thisCamera.fieldOfView = Mathf.Clamp(60f + (nowDistance - baseDistance) * longMag, 60f, 120f);
    }
}
