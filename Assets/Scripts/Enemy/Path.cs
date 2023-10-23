using Assets.Scripts.Enemy;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    [Tooltip("If true, the entity will go to the first node after reaching the last one, if false the entity will follow the path backward")]
    public Boolean loop = true;

    private List<PathNode> nodes = new List<PathNode>();

    // Direction of the path, 1 = forward, -1 = backward
    private int direction = 1;

    // Start is called before the first frame update
    void Awake()
    {
        // Get all the nodes of the path
        foreach (Transform child in transform)
        {
            nodes.Add(child.GetComponent<PathNode>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PathNode GetNextNode(PathNode currentNode)
    {
        // If there is no node, return the first one
        int index = nodes.IndexOf(currentNode);
        if (index < 0)
        {
            return nodes[0];
        }

        index += direction;

        // If we reached the end of the path or the beginning, return the first or last node depending on the loop parameter
        if (index < 0)
        {
            direction = 1;
            return nodes[0];
        }
        else if (index >= nodes.Count)
        {
            if (loop)
            {
                return nodes[0];
            }
            else
            {
                direction = -1;
                return nodes[nodes.Count - 1];
            }
        }

        return nodes[index];
    }

#if UNITY_EDITOR
    // Draw the path in the editor
    public void OnDrawGizmos()
    {
        if (Selection.activeGameObject == null || !Selection.activeGameObject.transform.IsChildOf(transform.parent.transform))
        {
            return;
        }

        Vector3 previousNode = Vector3.negativeInfinity;

        foreach (Transform child in transform)
        {
            PathNode node = child.GetComponent<PathNode>();
            if (node != null)
            {
                if (previousNode != Vector3.negativeInfinity)
                {
                    DrawGizmosArrow(previousNode, child.position);
                }
                previousNode = child.position;
            }
        }

        if (loop && previousNode != Vector3.negativeInfinity)
        {
            DrawGizmosArrow(previousNode, transform.GetChild(0).position);
        }

    }

    // Draw an arrow between two points
    public void DrawGizmosArrow(Vector3 from, Vector3 to)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawRay((from + to) / 2, Quaternion.Euler(0, 135, 0) * (to - from).normalized * 0.5f);
        Gizmos.DrawRay((from + to) / 2, Quaternion.Euler(0, -135, 0) * (to - from).normalized * 0.5f);
    }
#endif
}
