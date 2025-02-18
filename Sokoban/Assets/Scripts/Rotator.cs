using Objects.Boxes;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Турникет.")]
    [SerializeField] Turnstile turnstile;
    [Header("Трансформ относительно которого будет вращение.")]
    [SerializeField] Transform transformPivot;
    public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду


    float m_targetAngle;
    float m_currentAngle; // Текущий угол поворота


    void OnEnable()
    {
        if (transformPivot == null) return;
        turnstile.OnRotate += Rotate;
    }

    void OnDisable()
    {
        if (transformPivot == null) return;
        turnstile.OnRotate -= Rotate;
    }

    void Start()
    {
        m_targetAngle = transform.eulerAngles.y;
        m_currentAngle = m_targetAngle;
    }


    void Update()
    {
        if (Mathf.Abs(m_currentAngle - m_targetAngle) > Mathf.Epsilon)
        {
            var step = rotationSpeed * Time.deltaTime * Global.Instance.gameSpeed;
            var deltaAngle = Mathf.MoveTowardsAngle(m_currentAngle, m_targetAngle, step);
            var rotationDelta = deltaAngle - m_currentAngle;
            transform.RotateAround(transformPivot.position, Vector3.up, rotationDelta);
            m_currentAngle = deltaAngle;
        }

        // transform.RotateAround(transformPivot.position, Vector3.up, 1);
    }

    public void Rotate(float sign,float angle)
    {
        if (sign > 0.5f)
        {
            RotateLeft();
        }
        else
        {
            RotateRight();
        }
    }

    public void RotateLeft()
    {
        m_targetAngle = Mathf.Round((m_targetAngle + 90.0f) / 90.0f) * 90.0f;
    }

    public void RotateRight()
    {
        m_targetAngle = Mathf.Round((m_targetAngle - 90.0f) / 90.0f) * 90.0f;
    }
}