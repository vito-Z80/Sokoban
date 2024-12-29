using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] Assembler electrician;
    [SerializeField] Vector3 forward;


    [SerializeField] float moveSmoothTime;
    [SerializeField] float rotateSmoothTime;

    public Vector3 levelZeroOffsetPosition;
    public Vector3 levelZeroOffsetRotation;

    Camera m_cam;
    float m_time;

    Vector3 m_offset;
    Vector3 m_velocity;
    Vector3 m_electricianForward;


    void Start()
    {
        m_cam = GetComponent<Camera>();
        // SetFollow();
        // m_velocity = new Vector3(10, 23, 0.3f);
    }

    public void SetFollow()
    {
        //  TODO m_offset - нужно еще одну переменную для плавного движения камеры при переходе на следующий уровень. 
        //  m_offset должна интерполироваться в новую переменную.

        m_offset = electrician.transform.forward * -4 + electrician.transform.right * 0.1f + Vector3.up * 8;
        m_electricianForward = electrician.transform.forward;
    }

    public void OnMainMenuWatch()
    {
    }


    void LateUpdate()
    {
        m_time += Time.deltaTime;
        // FollowCharacter();
        Sway();
    }

    void Sway()
    {
        var angle = Mathf.Sin(m_time) * Mathf.Deg2Rad; // * 0.3f;
        var axis = transform.right * 0.21f + transform.forward * 0.13f;
        m_cam.transform.RotateAround(m_offset, axis, angle);
    }


    void FollowCharacter()
    {
        var targetPosition = electrician.transform.position + m_offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, Time.deltaTime * moveSmoothTime);
        var targetRotation = Quaternion.LookRotation(electrician.transform.position - transform.position - m_electricianForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSmoothTime);
    }

    public void SetCameraToLevelZeroPosition(Transform levelZero)
    {
        transform.position = levelZero.position + levelZero.rotation * levelZeroOffsetPosition;

        var rotationOffset = new Quaternion(
            levelZeroOffsetRotation.x,
            levelZeroOffsetRotation.y,
            levelZeroOffsetRotation.z,
            90.0f
        );

        transform.rotation = levelZero.rotation * rotationOffset;
    }
}