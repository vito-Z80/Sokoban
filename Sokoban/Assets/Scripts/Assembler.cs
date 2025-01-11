using System.Threading.Tasks;
using Data;
using Objects;
using Objects.Boxes;
using Objects.CollectibleObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class Assembler : MainObject
{
    public CharacterData characterData;

    [SerializeField] Transform neck;

    // InputSystemActions m_inputActions;

    Animator m_animator;

    const float RayDistance = 1.0f;

    int m_moveId;
    int m_animationLookBackId;

    [HideInInspector] public bool autoMove = true;

    Vector3 m_forward;
    Vector3 m_right;

    Vector3 m_rotateDirection;
    
    void OnEnable()
    {
        SetRightForward();
        m_animator = GetComponent<Animator>();
        m_moveId = Animator.StringToHash("Move");
        m_animationLookBackId = Animator.StringToHash("LookBack");
        // m_inputActions = new InputSystemActions();
        // m_inputActions.Enable();
    }


    void Start()
    {
        targetPosition = characterData.characterInMenuPositionOffset;
        Global.Instance.input.Player.MovesBack.started += MovesBackAction;
    }

    void OnDisable()
    {
        // m_inputActions.Disable();
    }

    public Transform GetNeck() => neck;

    bool CanMove(Vector3 direction)
    {
        var forwardStartPoint = transform.position + Vector3.up * 0.5f;
        Debug.DrawRay(forwardStartPoint, direction, Color.red);

        if (Physics.Raycast(forwardStartPoint, direction, out var forwardHit, RayDistance))
        {
            if (forwardHit.transform.TryGetComponent(out Collectible collectible))
            {
                collectible.Collect();
                return true;
            }
            return forwardHit.transform.TryGetComponent(out Box box) && box.Push(direction);
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

    public async Task LookBackAnimation()
    {
        m_animator.SetTrigger(m_animationLookBackId);
        await Task.Delay(3300); //  3.3 sec animation time
    }

    void RotateAnimation()
    {
        if (m_rotateDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(m_rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed * 8.0f);
        }
    }

    public void SetAutoMove(Vector3 targetPos, Vector3 forward)
    {
        autoMove = true;
        targetPosition = targetPos;
        m_rotateDirection = forward;
    }


    void Update()
    {
        if (!autoMove)
        {
            ControlledByPlayer();
        }
        
        // MovesBackAction();
        
        MoveAnimation(IsMoving());
        RotateAnimation();

        Move(Time.deltaTime);
    }

    void MovesBackAction(InputAction.CallbackContext callbackContext)
    {
        if (!IsMoving() && Global.Instance.levelPhase == LevelPhase.SearchSolution)
        {
            StepsController.OnPop?.Invoke();
        }
    }

    public bool IsMoving()
    {
        return targetPosition != transform.position;
    }

    void ControlledByPlayer()
    {
        if (targetPosition == transform.position)
        {
            var input = Global.Instance.input.Player.Move.ReadValue<Vector2>().Round();
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
                        targetPosition = transform.position.RoundWithoutY() + direction;
                        if (autoMove) return;
                        StepsController.OnPush?.Invoke();
                        if (Global.Instance.levelPhase == LevelPhase.SearchSolution)
                        {
                            Global.Instance.gameState.steps++;
                        }
                    }
                }
            }
        }
    }


    public void SetCharacterToLevelZero(Transform levelZero)
    {
        transform.position = levelZero.position + levelZero.rotation * characterData.characterInMenuPositionOffset;

        var rotationOffset = new Quaternion(
            characterData.characterInMenuRotationOffset.x,
            characterData.characterInMenuRotationOffset.y,
            characterData.characterInMenuRotationOffset.z,
            90.0f
        );

        transform.rotation = levelZero.rotation * rotationOffset;
    }


    public void SetRightForward()
    {
        m_forward = transform.forward;
        m_right = transform.right;
        m_forward.Normalize();
        m_right.Normalize();
    }
}