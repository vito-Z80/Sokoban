using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Объект класса может как растворяться, так и материализоваться.
    /// Материализация - объект принимает свой вид. Когда объект полностью примет свой вид, он должен поменять материал на статический и стать статичным.
    /// Растворение - может быть произведено только после материализации. Объект падает и растворяется, когда Y меньше -2 = объект уничтожается при определенном условии.
    /// </summary>
    public class Mutagen : MonoBehaviour
    {
        [SerializeField] Material material;
        [SerializeField] Material dissolveMaterial;

        Renderer m_renderer;

        Vector3 m_mainPosition;

        public bool isMaterialized;
        public bool isDissolved;

        void Start()
        {
            m_mainPosition = transform.position;
            
            m_renderer = GetComponentInChildren<Renderer>();
        }


        /// <summary>
        /// Растворить объект.<br/>
        /// Объект полностью раствориться когда Y меньше -2.
        /// </summary>
        /// <param name="destroy">Уничтожить объект после растворения.</param>
        public void Dissolve(bool destroy = true)
        {
            gameObject.isStatic = false;
            m_renderer.material = dissolveMaterial;
            //  down Y
            if (destroy)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Материализовать объект.<br/>
        /// Объект полностью материализуется когда Y больше или равен 0 <br/>
        /// </summary>
        public void Materialize()
        {
            if (transform.position.y >= 0.0f) return;
            var matPos = m_mainPosition;
            matPos.y = -2.0f;
            transform.position = matPos;
            _ = MaterializeObject();
        }


        async Task MaterializeObject()
        {
            gameObject.isStatic = false;
            m_renderer.material.enableInstancing = false;
            m_renderer.material = dissolveMaterial;

            while (Vector3.Distance(transform.position, m_mainPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_mainPosition, Time.deltaTime * 1.5f);
                await Task.Yield();
            }

            transform.position = m_mainPosition;

            m_renderer.material = material;
            gameObject.isStatic = true;
            m_renderer.material.enableInstancing = true;
            isMaterialized = true;
        }

        async Task DissolveObject(bool destroy = true)
        {
        }
    }
}