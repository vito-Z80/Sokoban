using System;
using Data;
using Interfaces;
using UnityEngine;

namespace Objects
{
    public class ColoredDoor : MainObject, IInteracting
    {
        [SerializeField] DoorColor color;
        Vector3 m_closedDoorPosition;
        Vector3 m_openDoorPosition;
        Vector3 m_targetPosition;
        
        void Start()
        {
            m_targetPosition = transform.position;
            m_closedDoorPosition = transform.position;
            m_openDoorPosition = transform.position + Vector3.down * 0.98f;

            var material = GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", GetColor());
            material.EnableKeyword("_EMISSION");
        }

        void Update()
        {
            if (m_targetPosition == transform.position) return;
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * Global.Instance.gameSpeed);
        }


        Color GetColor()
        {
            return color switch
            {
                DoorColor.White => Color.white,
                DoorColor.Red => Color.red,
                DoorColor.Green => Color.green,
                DoorColor.Blue => Color.blue,
                DoorColor.Yellow => Color.yellow,
                DoorColor.Magenta => Color.magenta,
                DoorColor.Cyan => Color.cyan,
                DoorColor.Black => Color.black,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Affect(bool affect)
        {
            m_targetPosition = affect ? m_openDoorPosition : m_closedDoorPosition;
        }
    }
}