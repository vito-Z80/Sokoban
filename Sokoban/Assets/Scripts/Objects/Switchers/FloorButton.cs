using UnityEngine;

namespace Objects.Switchers
{
    public class FloorButton : Switch
    {

        [SerializeField] Mesh up;
        [SerializeField] Mesh down;
        
        MeshFilter m_meshFilter;
        
        // Vector3 m_positionOn;
        // Vector3 m_positionOff;
        // Vector3 m_targetPosition;
        //
        // Material m_material;
        // Color m_colorBase;
        // Color m_colorTarget;
        // int m_colorPropId;
        // int m_colorEmissionId;

        void Start()
        {
            m_meshFilter = GetComponent<MeshFilter>();
            m_meshFilter.mesh = up;
            
            
            // m_positionOff = transform.position;
            // m_positionOn =  transform.position + Vector3.down * 0.035f;
            // m_targetPosition = m_positionOff;
            // m_material = GetComponent<Renderer>().material;
            // m_colorPropId = Shader.PropertyToID("_BaseColor");
            // m_colorEmissionId = Shader.PropertyToID("_EmissionColor");
            // m_colorBase = Color.white;
            // m_colorTarget = m_colorBase;
            // m_material.SetColor(m_colorEmissionId, GetColor());
        }

        // void Update()
        // {
        //     if (transform.position == m_targetPosition) return;
        //  
        //     var color = Color.LerpUnclamped(m_material.GetColor(m_colorPropId), m_colorTarget, Time.deltaTime * 4.0f);
        //     m_material.SetColor(m_colorPropId, color);
        //     
        //     transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime/8.0f * Global.Instance.gameSpeed);
        // }

        protected override void Touch()
        {
            m_meshFilter.mesh = down;
        }

        protected override void UnTouch()
        {
            m_meshFilter.mesh = up;
        }
    }
}