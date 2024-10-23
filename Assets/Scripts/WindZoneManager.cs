using UnityEngine;

public class WindZoneManager : MonoBehaviour
{
    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private Transform topTransform;
    [SerializeField] private Transform bottomTransform;
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;

    [Header("�����_���ŕ������ύX")]
    [SerializeField] private float randomChangeMax;
    [SerializeField] private float randomChangeMin;
    private float changeIntervalTimer;
    private float changeIntervalTime;
    private Transform windDirectionTransform;
    private int windDirectionNumber;

    void Start()
    {
        changeIntervalTimer = randomChangeMax;
        windDirectionTransform = leftTransform;
        windDirectionNumber = 2;
    }

    void Update()
    {
        ChangeWindDirection();

        // �����_���Őݒ肳�ꂽ�������ɕ��𐁂�����
        transform.LookAt(windDirectionTransform.position);
    }
    void ChangeWindDirection()
    {
        changeIntervalTimer -= Time.deltaTime;

        if (changeIntervalTimer <= 0f)
        {
            /*
            int randomWindDirectionNumber = Random.Range(2, 3);

            // �K����������ς���
            while (randomWindDirectionNumber == windDirectionNumber)
            {
                randomWindDirectionNumber = Random.Range(2, 3);
            }
            
            // �ς�������������擾
            windDirectionNumber = randomWindDirectionNumber;
            if (windDirectionNumber == 0)
            {
                windDirectionTransform = topTransform;
            }
            else if (windDirectionNumber == 1)
            {
                windDirectionTransform = bottomTransform;
            }
            else if (windDirectionNumber == 2)
            {
                windDirectionTransform = leftTransform;
            }
            else
            {
                windDirectionTransform = rightTransform;
            }
            */

            if (windDirectionNumber == 2)
            {
                windDirectionTransform = rightTransform;
                windDirectionNumber = 3;
            }
            else
            {
                windDirectionTransform = leftTransform;
                windDirectionNumber = 2;
            }

            changeIntervalTimer = Random.Range(randomChangeMin, randomChangeMax);
        }
    }
}
