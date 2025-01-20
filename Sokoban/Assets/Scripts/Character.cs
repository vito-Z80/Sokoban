using Interfaces;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MainObject, IMovable
{
    Vector3 m_targetPosition;
    Vector3 m_rotateDirection;
    Vector3 m_forward;
    Vector3 m_right;

    bool m_freezed;

    int m_sideLayerMask;
    int m_bottomLayerMask;

    Vector2 m_direction;

    Vector2[] m_path = new Vector2[] { Vector2.up, Vector2.up, Vector2.left, Vector2.left };

    InputAction m_input;

    public Transform GetTransform => transform;

    public Vector3 TargetPosition
    {
        get => m_targetPosition;
        set => m_targetPosition = value;
    }


    public bool Freezed
    {
        get => m_freezed;
        set => m_freezed = value;
    }


    void Start()
    {
        m_input = Global.Instance.input.Player.Move;
        m_targetPosition = transform.position;
        SetRightForward();

        m_sideLayerMask = LayerMask.GetMask(
            "Box",
            "Portal",
            "Door",
            "Wall",
            "Collectible"
        );
        m_bottomLayerMask = LayerMask.GetMask(
            "Floor",
            "Swich",
            "Point",
            "Box"
        );
    }


    void Update()
    {
        Move();

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * Global.Instance.gameSpeed);
        // return transform.position != m_targetPosition;
    }

    public bool CanMove(Vector3 direction)
    {
        var position = transform.position + Vector3.up * 0.5f;
        if (Raycast(position, direction, out var hit, 1.0f, m_sideLayerMask))
        {
            return false;
        }

        return true;
    }


    void Move()
    {
        if (m_targetPosition == transform.position)
        {
#if !UNITY_ANDROID

            m_direction = m_input.ReadValue<Vector2>().Round();

#endif
            if (m_direction != Vector2.zero)
            {
                var direction = CorrectInput(m_direction);

                m_rotateDirection = direction;
                if (CanMove(direction))
                {
                    if (direction.x != 0.0f && direction.z == 0.0f || direction.z != 0.0f && direction.x == 0.0f)
                    {
                        m_targetPosition = transform.position.RoundWithoutY() + direction;
                        // if (m_autoMove) return;
                        // StepsController.OnPush?.Invoke();
                        // if (Global.Instance.levelPhase == LevelPhase.SearchSolution)
                        // {
                        //     Global.Instance.gameState.steps++;
                        // }
                    }
                }
            }
        }
    }

    Vector3 CorrectInput(Vector2 input)
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

        return direction;
    }

    public void ToLeft()
    {
        m_direction = Vector2.left;
    }

    public void ToRight()
    {
        m_direction = Vector2.right;
    }

    public void Forward()
    {
        m_direction = Vector2.up;
    }

    public void Backward()
    {
        m_direction = Vector2.down;
    }

    public void SetRightForward()
    {
        m_forward = transform.forward;
        m_right = transform.right;
        m_forward.Normalize();
        m_right.Normalize();
    }
}