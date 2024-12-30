using System;
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
    
    
    [SerializeField] Vector3[] cameraPath;

    Camera m_cam;
    float m_time;

    Vector3 m_offset;
    Vector3 m_velocity;
    Vector3 m_electricianForward;

    public enum State
    {
        Stay,
        FollowPath,
        FollowCharacter
    }

    State m_state;

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


    void LateUpdate()
    {
        m_time += Time.deltaTime;

        switch (m_state)
        {
            case State.Stay:
                Sway();
                break;
            case State.FollowPath:
                if (FollowPath()) m_state = State.FollowCharacter;
                break;
            case State.FollowCharacter:
                Sway();
                FollowCharacter();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetCameraState(State state)
    {
        m_state = state;
    }

    public State GetCameraState()
    {
        return m_state;
    } 

    int m_pathPointIndex;
    float m_startPathTime;
    bool FollowPath()
    {
        if (m_pathPointIndex >= cameraPath.Length)
        {
            return true;
        }
        
        
        var targetRotation = Quaternion.LookRotation(electrician.GetNeck().position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4.0f);
        
        
        m_startPathTime = Mathf.Clamp(m_startPathTime + Time.deltaTime / 4.0f , 0.0f, 1.0f);
        var pos = Vector3.MoveTowards(transform.position, cameraPath[m_pathPointIndex], m_startPathTime * Time.deltaTime);
    
        if (Vector3.Distance(transform.position, cameraPath[m_pathPointIndex]) < 0.05f)
        {
            m_pathPointIndex++;
        }

        transform.position = pos;
        return false;
    }

    void Sway()
    {
        var angle = Mathf.Sin(m_time) * Mathf.Deg2Rad; // * 0.3f;
        var axis = transform.right * 0.21f + transform.forward * 0.13f;
        transform.RotateAround(m_offset, axis, angle);
    }


    void FollowCharacter()
    {
        var targetPosition = electrician.transform.position + m_offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, Time.deltaTime * moveSmoothTime);
        var targetRotation = Quaternion.LookRotation(electrician.transform.position - transform.position - m_electricianForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSmoothTime);
    }

    public void SetCameraToLevelZeroLocation()
    {
        transform.position = cameraPath[0];
        transform.LookAt(electrician.GetNeck());
    }
}