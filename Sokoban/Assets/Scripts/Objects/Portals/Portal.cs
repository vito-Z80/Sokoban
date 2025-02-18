using System.Collections;
using Interfaces;
using UnityEngine;

namespace Objects.Portals
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] Portal otherSidePortal;
        [SerializeField] ParticleSystem teleportEffect;
        [SerializeField] ParticleSystem errorEffect;

        [SerializeField] AudioSource audioSource;

        readonly WaitForSeconds m_waitSecond = new(1.0f);
        readonly WaitForSeconds m_waitHalfSecond = new(0.5f);

        IMovable m_inside;
        readonly Collider[] m_colliders = new Collider[1];
        int m_maskLayers;

        int m_texId;
        Material m_material;
        Vector2 m_texOffset = Vector2.zero;


        void Start()
        {
            m_maskLayers = LayerMask.GetMask("Box", "Assembler");
            m_texId = Shader.PropertyToID("_BaseMap");
            m_material = GetComponent<Renderer>().sharedMaterial;
        }

        void Update()
        {
            m_material.SetTextureOffset(m_texId, m_texOffset);
            m_texOffset.x += Time.deltaTime * 3.0f;
        }

        IEnumerator PushObjectFromPortal()
        {
            var moveable = m_inside;
            m_inside = null;

            if (moveable is Assembler)
            {
                //  гг не выталкивается из портала принятия так как он сам ходить умеет.
                yield return m_waitHalfSecond;
                moveable.Freezed = false;
                yield break;
            }

            moveable.Freezed = false;
            //  портал принятия чекает можно ли вытолкнуть объект из портала ?
            while (true)
            {
                if (moveable.GetTransform.position != moveable.TargetPosition)
                {
                    yield break;
                }

                //  TODO сделать проверку OverlapSphereNonAlloc и только потом сдвигать если нет пересечений с другими коллайдерами.
                //   проблема в том что портал может выталкивать короб и в это же время гг может идти на эту коробку = чел оказывается в коробке.
                //   с другой стороны... ни кто не может толкнуть объект с обратной стороны что бы с ним столкнулся гг.

                if (moveable.CanMove(transform.forward))
                {
                    // выталкиваем
                    while (moveable.GetTransform.position != moveable.TargetPosition)
                    {
                        yield return null;
                    }

                    yield break;
                }

                yield return m_waitSecond;
            }
        }

        IEnumerator PullObjectToPortal()
        {
            //  затягиваем объект в портал
            m_inside.TargetPosition = new Vector3(
                transform.position.x,
                m_inside.GetTransform.position.y,
                transform.position.z
            );
            while (m_inside.GetTransform.position != m_inside.TargetPosition)
            {
                yield return null;
            }

            yield return null;
            //  проверяем можно ли переслать объект в другой портал
            if (Physics.OverlapSphereNonAlloc(otherSidePortal.transform.position, 0.49f, m_colliders, m_maskLayers) == 0)
            {
                audioSource.clip = Global.Instance.teleportSound;
                audioSource.Play();
                //  можно переслать объект: эффект отправки, перенос объекта, эффект принятия, отключаем коллайдер портала отправки.
                teleportEffect.Play();
                yield return m_waitHalfSecond;
                m_inside.GetTransform.gameObject.SetActive(false);

                m_inside.TargetPosition = new Vector3(
                    otherSidePortal.transform.position.x,
                    m_inside.GetTransform.position.y,
                    otherSidePortal.transform.position.z
                );
                m_inside.GetTransform.position = m_inside.TargetPosition;

                otherSidePortal.m_inside = m_inside;
                m_inside = null;
                audioSource.Play();
                otherSidePortal.teleportEffect.Play();
                yield return m_waitHalfSecond;
                otherSidePortal.m_inside.GetTransform.gameObject.SetActive(true);
            }
            else
            {
                //  нельзя переслать объект: эффект сломанного портала.
                audioSource.clip = Global.Instance.teleportErrorSound;
                audioSource.Play();
                errorEffect.Play();
                yield return m_waitHalfSecond;
                m_inside.Freezed = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IMovable movable))
            {
                if (movable.Freezed)
                {
                    StartCoroutine(PushObjectFromPortal());
                }
                else
                {
                    movable.Freezed = true;
                    m_inside = movable;
                    StartCoroutine(PullObjectToPortal());
                }
            }
        }
    }
}