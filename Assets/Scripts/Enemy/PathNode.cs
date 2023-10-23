using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class PathNode : MonoBehaviour
    {
        [Tooltip("Time in seconds the entity will wait at this node")]
        public float stopTime = 0f;

        [Tooltip("If true, the entity will look in the direction of the node")]
        public bool lookInDirection = false;
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        // Use this for initialization
        void Awake()
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

        }

#if UNITY_EDITOR
        // Draw the path node in the editor
        public void OnDrawGizmos()
        {
            if (Selection.activeGameObject == null || !Selection.activeGameObject.transform.IsChildOf(transform.parent.parent.transform))
            {
                return;
            }

            if (Selection.activeGameObject == gameObject)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireSphere(transform.position, 0.25f);

            // Draws an arrow pointing towards the direction of the node
            if (lookInDirection)
            {
                Vector3 from = transform.position;
                Vector3 to = transform.position + transform.forward;
                Vector3 arrowHead1 = to + Quaternion.Euler(0, 180 + 20, 0) * transform.forward * 0.5f;
                Vector3 arrowHead2 = to + Quaternion.Euler(0, 180 - 20, 0) * transform.forward * 0.5f;
                Gizmos.DrawLine(from, to);
                Gizmos.DrawLine(to, arrowHead1);
                Gizmos.DrawLine(to, arrowHead2);
            }
        }
#endif
    }
}