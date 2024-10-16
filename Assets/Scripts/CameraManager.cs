using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private Camera thisCamera;

    // ���R���|�[�l���g�擾
    private PlayerManager playerManager;

    [Header("�����ɉ����ăJ����������")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private float longMag;
    private float baseDistance;

    // ��]�ʂ𖳎�����
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
        // ���݂̋������擾����
        float nowDistance = Vector3.Distance(playerTransform.position, groundTransform.position);

        thisCamera.fieldOfView = Mathf.Clamp(60f + (nowDistance - baseDistance) * longMag, 60f, 120f);
    }
    void RotateIgnore()
    {
        rotateIgnoreTransform.localRotation = Quaternion.Euler(0f, 0f, -playerManager.GetAddRotationZ());
    }
}
