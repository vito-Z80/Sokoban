﻿using Data;
using Effect;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Switchers
{
    public class FloorButton : Switcher
    {
        

        [SerializeField] Waypoint waypoint;
        
        Vector3 m_positionOn;
        Vector3 m_positionOff;

        Material m_material;
        Color m_colorBase;
        Color m_colorTarget;
        int m_colorPropId;

        void Start()
        {
            m_positionOff = transform.position;
            m_positionOn = m_positionOff + Vector3.down * 0.04f;
            m_material = GetComponent<Renderer>().material;
            m_colorPropId = Shader.PropertyToID("_BaseColor");
            m_colorBase = m_material.GetColor(m_colorPropId);
        }

        void Update()
        {
            var color = Color.LerpUnclamped(m_material.GetColor(m_colorPropId), m_colorTarget, Time.deltaTime * 4.0f);
            m_material.SetColor(m_colorPropId, color);
            
            if (transform.position == targetPosition) return;
            Move(Time.deltaTime);
        }

        protected override void Touch()
        {
            targetPosition = m_positionOn;
            isOn = true;
            m_colorTarget = GetColor();
            waypoint.Show(isOn, m_colorTarget);
        }

        protected override void UnTouch()
        {
            targetPosition = m_positionOff;
            isOn = false;
            m_colorTarget = m_colorBase;
            waypoint.Show(isOn, Color.black);
        }
    }
}