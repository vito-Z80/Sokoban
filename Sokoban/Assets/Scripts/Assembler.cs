using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Interfaces;
using Objects;
using Objects.Boxes;
using Objects.CollectibleObjects;
using Objects.Portals;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Assembler : MainObject, IMovable, IUndo
{
    public CharacterData characterData;

    [SerializeField] Transform neck;

    Animator m_animator;
    InputAction m_input;

    const float RayDistance = 1.0f;

    int m_moveId;
    int m_animationSpeedId;
    int m_animationLookBackId;
    int m_sideLayerMask;
    int m_bottomLayerMask;

    Vector3 m_forward;
    Vector3 m_right;
    Vector3 m_targetPosition;
    Vector3 m_rotateDirection;

    Vector2 m_direction;

    bool m_freezed;


    public Transform GetNeck() => neck;

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

    public List<BackStepTransform> Stack { get; } = new();

    void OnEnable()
    {
        m_freezed = true;
        SetRightForward();
        m_animator = GetComponent<Animator>();
        m_moveId = Animator.StringToHash("Move");
        m_animationSpeedId = Animator.StringToHash("Speed");
        m_animationLookBackId = Animator.StringToHash("LookBack");

        m_sideLayerMask = LayerMask.GetMask(
            "Box",
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


    void Start()
    {
        m_input = Global.Instance.input.Player.Move;
        m_targetPosition = characterData.characterInMenuPositionOffset;
        Global.Instance.input.Player.MovesBack.started += MovesBackAction;
    }

    void OnDisable()
    {
        // m_inputActions.Disable();
    }

    public bool CanMove(Vector3 direction)
    {
        var position = transform.position + Vector3.up * 0.5f;
        if (Raycast(position, direction, out var hit, 1.0f, m_sideLayerMask))
        {
            if (hit.transform.TryGetComponent<IMovable>(out var movable))
            {
                return movable.CanMove(direction);
            }

            if (hit.transform.TryGetComponent<Collectible>(out var collectible))
            {
                return collectible.Collect();
            }

            return false;
        }

        if (!Raycast(position + direction, Vector3.down, out hit, 0.6f, m_bottomLayerMask))
        {
            return false;
        }

        return true;
    }

    // bool CanMove(Vector3 direction)
    // {
    //     var forwardStartPoint = transform.position + Vector3.up * 0.5f;
    //     Debug.DrawRay(forwardStartPoint, direction, Color.red);
    //
    //     if (Raycast(forwardStartPoint, direction, out var forwardHit, RayDistance, m_sideLayerMask))
    //     {
    //         if (forwardHit.transform.TryGetComponent(out Collectible collectible))
    //         {
    //             collectible.Collect();
    //             return true;
    //         }
    //
    //         if (EnterPortal(forwardHit.transform)) return true;
    //
    //         return MoveBoxTo(forwardHit.transform, direction);
    //     }
    //
    //     var belowStartPoint = forwardStartPoint + direction;
    //     Debug.DrawRay(belowStartPoint, Vector3.down, Color.red);
    //     if (Raycast(belowStartPoint, Vector3.down, out var belowHit, RayDistance, m_bottomLayerMask))
    //     {
    //         return belowHit.transform is not null;
    //     }
    //
    //     return false;
    // }

    bool MoveBoxTo(Transform t, Vector3 direction)
    {
        return t.TryGetComponent(out Box box) && box.CanMove(direction);
    }

    bool EnterPortal(Transform t)
    {
        if (t.TryGetComponent<Portal>(out var portal))
        {
            if (portal.GetState() == Portal.State.Inactive)
            {
                m_freezed = true;
                return true;
            }
        }

        return false;
    }


    void MoveAnimation(bool play)
    {
        m_animator.SetFloat(m_animationSpeedId, Global.Instance.gameSpeed);
        m_animator.SetBool(m_moveId, play /*|| m_inputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero*/);
    }

    public async Task LookBackAnimation()
    {
        m_animator.SetTrigger(m_animationLookBackId);
        await Task.Delay(500);

        while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            await Task.Yield();
        }
    }

    void RotateAnimation()
    {
        if (m_rotateDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(m_rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * Global.Instance.gameSpeed * 8.0f);
        }
    }

    public void SetAutoMove(Vector3 targetPos, Vector3 forward)
    {
        m_freezed = true;
        m_targetPosition = targetPos;
        m_rotateDirection = forward;
    }


    void Update()
    {
        if (!m_freezed)
        {
            Move();
        }

        MoveAnimation(IsMoving());
        RotateAnimation();

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * Global.Instance.gameSpeed);
    }

    void MovesBackAction(InputAction.CallbackContext callbackContext)
    {
        if (!IsMoving() && Global.Instance.levelPhase == LevelPhase.SearchSolution)
        {
            UndoController.Pop();
        }
    }

    public bool IsMoving()
    {
        return m_targetPosition != transform.position;
    }


    //  android


    public void ToLeft()
    {
        m_direction = Vector2.left;
    }

    public void ToRight()
    {
        m_direction = Vector2.right;
    }

    public void ToUp()
    {
        m_direction = Vector2.up;
    }

    public void ToDown()
    {
        m_direction = Vector2.down;
    }

    // Vector2 m_mobileInput;

    // void MobileControlledByPlayer()
    // {
    //     if (m_targetPosition == transform.position)
    //     {
    //         if (m_mobileInput != Vector2.zero)
    //         {
    //             var direction = Vector3.zero;
    //
    //             if (m_mobileInput.x != 0 || m_mobileInput.y == 0)
    //             {
    //                 direction = (m_right * m_mobileInput.x).Round();
    //             }
    //             else if (m_mobileInput.y != 0 || m_mobileInput.x == 0)
    //             {
    //                 direction = (m_forward * m_mobileInput.y).Round();
    //             }
    //
    //             m_rotateDirection = direction;
    //             if (CanMove(direction))
    //             {
    //                 if (direction.x != 0.0f && direction.z == 0.0f || direction.z != 0.0f && direction.x == 0.0f)
    //                 {
    //                     m_targetPosition = transform.position.RoundWithoutY() + direction;
    //                     if (autoMove) return;
    //                     StepsController.OnPush?.Invoke();
    //                     if (Global.Instance.levelPhase == LevelPhase.SearchSolution)
    //                     {
    //                         Global.Instance.gameState.steps++;
    //                     }
    //                 }
    //             }
    //         }
    //
    //         m_mobileInput = Vector2.zero;
    //     }
    // }
    //
    //
    // //
    //
    //
    // void ControlledByPlayer()
    // {
    //     if (m_targetPosition == transform.position)
    //     {
    //         var input = Global.Instance.input.Player.Move.ReadValue<Vector2>().Round();
    //         if (input != Vector2.zero)
    //         {
    //             var direction = Vector3.zero;
    //
    //             if (input.x != 0 || input.y == 0)
    //             {
    //                 direction = (m_right * input.x).Round();
    //             }
    //             else if (input.y != 0 || input.x == 0)
    //             {
    //                 direction = (m_forward * input.y).Round();
    //             }
    //
    //             m_rotateDirection = direction;
    //             if (CanMove(direction))
    //             {
    //                 if (direction.x != 0.0f && direction.z == 0.0f || direction.z != 0.0f && direction.x == 0.0f)
    //                 {
    //                     m_targetPosition = transform.position.RoundWithoutY() + direction;
    //                     if (autoMove) return;
    //                     StepsController.OnPush?.Invoke();
    //                     if (Global.Instance.levelPhase == LevelPhase.SearchSolution)
    //                     {
    //                         Global.Instance.gameState.steps++;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }


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
                        // if (m_freezed) return;
                        UndoController.Push();
                        if (Global.Instance.levelPhase == LevelPhase.SearchSolution)
                        {
                            Global.Instance.gameState.steps++;
                        }
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

    public void Push()
    {
        Stack.Add(new BackStepTransform(transform));
    }

    public void Pop()
    {
        if (Stack.Count == 0) return;
        var data = Stack.Last();
        transform.rotation = data.Rotation;
        transform.localScale = data.Scale;
        m_targetPosition = data.Position;
        transform.position = data.Position;
        Stack.RemoveAt(Stack.Count - 1);
    }
    
    void OnDestroy()
    {
        Debug.Log(gameObject.name + " is destroyed");
    }
}