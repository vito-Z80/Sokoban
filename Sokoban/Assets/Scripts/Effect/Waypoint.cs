using System;
using UnityEngine;

namespace Effect
{
    public class Waypoint : MonoBehaviour
    {

        [SerializeField] Transform startTransform;
        public Vector3[] points;
        bool m_isShowing;
        
        enum Direction
        {
            Forward,Backward
        }
        
        int m_pointIndex;
        Direction m_direction;


        void Start()
        {
            if (points != null )
            {
                for (var i = 0; i < points.Length; i++)
                {
                    var newPos = startTransform.TransformPoint(points[i]);
                    points[i].y = -0.49f;
                    points[i] = newPos;
                }
                
            }
        }


        void Update()
        {
            if (!m_isShowing || points == null || points.Length == 0) return;
            
            var target = points[m_pointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 16f);
            
            
            if (transform.position == target)
            {
                
                switch (m_direction)
                {
                    case Direction.Forward:
                        m_pointIndex++;
                        break;
                    case Direction.Backward:
                        m_pointIndex--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                if (m_pointIndex >= points.Length)
                {
                    m_direction = Direction.Backward;
                    m_pointIndex = points.Length - 2;
                } else if (m_pointIndex < 0)
                {
                    m_direction = Direction.Forward;
                    m_pointIndex = 0;
                }
            }
        }


        public void Show(bool show, Color color)
        {
            m_isShowing = show;
        }
        

    }
}