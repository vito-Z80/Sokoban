using Effect;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Waypoint))]
    public class WaypointEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active)]
        static void DrawGizmos(Waypoint waypoint, GizmoType gizmoType)
        {
            Gizmos.color = Color.yellow;
            var points = waypoint.points;
            if (points == null || points.Length == 0)
            {
                Gizmos.color = Color.yellow;
                var position = waypoint.transform.TransformPoint(Vector3.zero);
                Gizmos.DrawSphere(position, 0.3f);
                return;
            }

            var pointsCount = waypoint.points.Length;

            for (var i = 0; i < pointsCount - 1; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(points[i], points[i + 1]);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(points[i], 0.3f);
            }

            Gizmos.DrawSphere(points[^1], 0.3f);
        }
    }
}