using Objects;
using Objects.Boxes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Assembler : MainObject
{
    [SerializeField] float speed = 1.0f;
    Vector3 m_startPosition;


    InputSystemActions m_inputActions;
    InputAction.CallbackContext m_input;
    Animator m_animator;

    const float RayDistance = 1.0f;

    int m_moveId;

    public bool autoMove;


    Vector2 m_direction;


    Vector3 m_forward;
    Vector3 m_right;

    Vector3 m_rotateDirection;

    void OnEnable()
    {
        SetRightForward();
        m_animator = GetComponent<Animator>();
        m_moveId = Animator.StringToHash("Move");
        m_inputActions = new InputSystemActions();
        m_inputActions.Enable();
        m_inputActions.Player.Move.started += BeginMoving;
    }


    void Start()
    {
        m_startPosition = transform.position;
        TargetPosition = transform.position;
    }

    void OnDisable()
    {
        m_inputActions.Player.Move.started -= BeginMoving;
        m_inputActions.Disable();
    }

    void SetStartAndTargetPositions()
    {
        m_startPosition = m_startPosition.RoundWithoutY();
        TargetPosition = TargetPosition.RoundWithoutY();


        if (m_direction.x != 0)
        {
            TargetPosition += m_right * m_direction.x;
        }

        if (m_direction.y != 0)
        {
            TargetPosition += m_forward * m_direction.y;
        }
    }

    void BeginMoving(InputAction.CallbackContext input)
    {
        m_input = input;
        if (autoMove) return;
        if (transform.position != TargetPosition) return;
        m_direction = input.ReadValue<Vector2>().Round();

        m_startPosition = transform.position;
        TargetPosition = transform.position;
        SetStartAndTargetPositions();
        CheckCollide();
    }

    void CheckCollide()
    {
        var direction = m_forward * m_direction.y + m_right * m_direction.x;
        m_rotateDirection = direction;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, out var hit, RayDistance)) //   (+ Vector3.up * 0.5f) - потому что у персонажа пивот снизу.
        {
            if (hit.transform.TryGetComponent<Box>(out var box))
            {
                if (box.CanStep(direction)) return;
            }

            m_direction = Vector2.zero;
            TargetPosition = transform.position;
        }
    }

    bool HasFloor()
    {
        var pos = TargetPosition + Vector3.up * 1.5f;
        var dir = Vector3.down;

        Debug.DrawRay(pos, dir * 2, Color.red);
        
        
        if (Physics.Raycast(pos, dir, out var hit, 2))
        {
            Debug.Log(hit.transform.name);
            return true;
        }


        return false;
    }


    void Animation(bool play)
    {
        m_animator.SetBool(m_moveId, play);
    }

    void RotateAnimation()
    {
        if (m_rotateDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(m_rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed * 8.0f);
        }
    }

    public void SetAutoMove(Vector3 targetPosition)
    {
        autoMove = true;
        TargetPosition = targetPosition + Vector3.down * 0.5f;
        m_rotateDirection = Vector3.forward;
    }

    void Update()
    {
        if (autoMove)
        {
            Move(speed * Time.deltaTime);
        }
        else
        {
            // if (HasFloor())
            // {
                MoveCharacter();
            // }
        }

        Animation(transform.position != TargetPosition);
        RotateAnimation();


        // var moveDirection = m_forward * m_direction.y + m_right * m_direction.x;
        // Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDirection * RayDistance, Color.red);
    }

    void MoveCharacter()
    {
        var a = (transform.position - m_startPosition).sqrMagnitude;
        var b = (TargetPosition - m_startPosition).sqrMagnitude;

        var moveDirection = m_forward * m_direction.y + m_right * m_direction.x;
        var newPosition = transform.position + moveDirection * (speed * Time.deltaTime);

        //  если целевая позиция достигнута.
        if (a >= b)
        {
            if (!m_input.started) //  если кнопка движения отжата.
            {
                newPosition = TargetPosition;
            }
            else
            {
                //  кнопка удерживается при достижении целевой позиции ячейки, получаем новое направление, устанавливаем стартовую и целевую позиции.
                Redirect();
                m_startPosition = TargetPosition;
                SetStartAndTargetPositions();
                CheckCollide();
            }
        }

        transform.position = newPosition;
    }


    void Redirect()
    {
        var newDirection = m_input.ReadValue<Vector2>().Round();
        if (newDirection.x != 0.0f && newDirection.y != 0.0f)
        {
            if (Mathf.Approximately(newDirection.x, m_direction.x) && m_direction.x != 0.0f)
            {
                newDirection.x = 0.0f;
            }

            if (Mathf.Approximately(newDirection.y, m_direction.y) && m_direction.y != 0.0f)
            {
                newDirection.y = 0.0f;
            }

            if (newDirection.x != 0.0f && newDirection.y != 0.0f)
            {
                newDirection = Vector2.zero;
            }
        }

        m_direction = newDirection;
    }


    public void SetRightForward()
    {
        m_forward = transform.forward;
        m_right = transform.right;
        m_forward.Normalize();
        m_right.Normalize();
    }
}