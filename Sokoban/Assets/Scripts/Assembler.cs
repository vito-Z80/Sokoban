using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class Assembler : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    Vector3 m_startPosition;
    Vector3 m_targetPosition;


    InputSystemActions m_inputActions;
    InputAction.CallbackContext m_input;
    Animator m_animator;

    const float RayDistance = 1.0f;

    int m_moveId;
    bool m_canMove;


    Vector2 m_direction;


    [SerializeField] Vector3 m_forward;
    [SerializeField]Vector3 m_right;

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
        m_targetPosition = transform.position;
        m_canMove = true;
        var a = new Vector3(123.23f,1.2010222102f,1.9999102f);
        Debug.Log(a);
        Debug.Log(a.normalized);
        a.Normalize();
        Debug.Log(a);
    }

    void OnDisable()
    {
        m_inputActions.Player.Move.started -= BeginMoving;
        m_inputActions.Disable();
    }

    public void SetMove(bool canMove)
    {
        m_canMove = canMove;
    }

    void SetStartAndTargetPositions()
    {

        m_startPosition = m_startPosition.RoundWithoutY();
        m_targetPosition = m_targetPosition.RoundWithoutY();
        
        
        if (m_direction.x != 0)
        {
            m_targetPosition += m_right * m_direction.x;
        }

        if (m_direction.y != 0)
        {
            m_targetPosition += m_forward * m_direction.y;
        }
    }

    void BeginMoving(InputAction.CallbackContext input)
    {
        m_input = input;
        if (!m_canMove) return;
        if (transform.position != m_targetPosition) return;
        m_direction = input.ReadValue<Vector2>().Round();

        m_startPosition = transform.position;
        m_targetPosition = transform.position;
        SetStartAndTargetPositions();
        CheckCollide();
    }

    void CheckCollide()
    {
        var direction = m_forward * m_direction.y + m_right * m_direction.x;
        m_rotateDirection = direction;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, out var hit, RayDistance))
        {
            if (hit.transform.TryGetComponent<Box>(out var box))
            {
                if (box.CanStep(direction)) return;
            }

            m_direction = Vector2.zero;
            m_targetPosition = transform.position;
        }
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


    void Update()
    {
        MoveCharacter();
        Animation(transform.position != m_targetPosition);
        RotateAnimation();

        // var moveDirection = m_forward * m_direction.y + m_right * m_direction.x;
        // Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDirection * RayDistance, Color.red);
    }

    void MoveCharacter()
    {
        var a = (transform.position - m_startPosition).sqrMagnitude;
        var b = (m_targetPosition - m_startPosition).sqrMagnitude;

        var moveDirection = m_forward * m_direction.y + m_right * m_direction.x;
        var newPosition = transform.position + moveDirection * (speed * Time.deltaTime);

        //  если целевая позиция достигнута.
        if (a >= b)
        {
            if (!m_input.started || !m_canMove) //  если кнопка движения отжата.
            {
                newPosition = m_targetPosition;
            }
            else
            {
                //  кнопка удерживается при достижении целевой позиции ячейки, получаем новое направление, устанавливаем стартовую и целевую позиции.
                Redirect();
                m_startPosition = m_targetPosition;
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


    void SetRightForward()
    {
        m_forward = transform.forward;
        m_right = transform.right;
        m_forward.Normalize();
        m_right.Normalize();
    }
}