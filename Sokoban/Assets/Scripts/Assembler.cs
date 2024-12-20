using Objects;
using Objects.Boxes;
using UnityEngine;

public class Assembler : MainObject
{
    [SerializeField] float speed = 1.0f;


    InputSystemActions m_inputActions;

    Animator m_animator;

    const float RayDistance = 1.0f;

    int m_moveId;

    [HideInInspector] public bool autoMove;


    Vector2 m_direction;


    Vector3 m_forward;
    Vector3 m_right;

    Vector3 m_rotateDirection;


    void OnEnable()
    {
        Debug.Log(autoMove);
        SetRightForward();
        m_animator = GetComponent<Animator>();
        m_moveId = Animator.StringToHash("Move");
        m_inputActions = new InputSystemActions();
        m_inputActions.Enable();
    }


    void Start()
    {
        TargetPosition = transform.position;
    }

    void OnDisable()
    {
        m_inputActions.Disable();
    }

    bool CanMove(Vector3 direction)
    {
        var forwardStartPoint = transform.position + Vector3.up * 0.5f;
        Debug.DrawRay(forwardStartPoint, direction, Color.red);
        if (Physics.Raycast(forwardStartPoint, direction, out var forwardHit, RayDistance))
        {
            return forwardHit.transform.TryGetComponent(out Box box) && box.CanStep(direction);
        }

        var belowStartPoint = forwardStartPoint + direction;
        Debug.DrawRay(belowStartPoint, Vector3.down, Color.red);
        if (Physics.Raycast(belowStartPoint, Vector3.down, out var belowHit, RayDistance))
        {
            return belowHit.transform is not null;
        }

        return false;
    }


    void MoveAnimation(bool play)
    {
        m_animator.SetBool(m_moveId, play /*|| m_inputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero*/);
    }

    void RotateAnimation()
    {
        if (m_rotateDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(m_rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed * 8.0f);
        }
    }

    public void SetAutoMove(Vector3 targetPosition, Vector3 forward)
    {
        autoMove = true;
        TargetPosition = targetPosition + Vector3.down * 0.5f;
        m_rotateDirection = forward;
    }

    void Update()
    {
        // if (!autoMove)
        // {
            ControlledByPlayer();
        // }

        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * speed);
    }

    void ControlledByPlayer()
    {
        if (TargetPosition == transform.position)
        {
            var input = m_inputActions.Player.Move.ReadValue<Vector2>().Round();
            if (input != Vector2.zero)
            {
                var direction = Vector3.zero;

                if (input.x != 0 || input.y == 0)
                {
                    direction = (m_right * input.x).Round();
                }
                else if (input.y != 0 || input.x == 0)
                {
                    direction = (m_forward * input.y).Round();
                }

                m_rotateDirection = direction;
                if (CanMove(direction))
                {
                    if (direction.x != 0.0f && direction.z == 0.0f || direction.z != 0.0f && direction.x == 0.0f)
                    {
                        TargetPosition = transform.position.RoundWithoutY() + direction;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        MoveAnimation(transform.position != TargetPosition);
        RotateAnimation();
    }

    public void SetRightForward()
    {
        m_forward = transform.forward;
        m_right = transform.right;
        m_forward.Normalize();
        m_right.Normalize();
    }
}