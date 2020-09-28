using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> adjacentNodes = new List<Node>();
    public Vector2[] validDirections;
    public Node previous = null;
    public string label;
    public bool isTeleport = false;
    public Node teleportReceiver;

    private void Start()
    {
        validDirections = new Vector2[adjacentNodes.Count];
        for (int i = 0; i < adjacentNodes.Count; ++i)
        {
            Node adjacent = adjacentNodes[i];
            Vector2 tempVector = adjacent.transform.localPosition - transform.localPosition;
            tempVector.x = (float)Math.Round(tempVector.x);
            tempVector.y = (float)Math.Round(tempVector.y);
            validDirections[i] = tempVector.normalized;
            //Debug.Log(tempVector.normalized);
        }
        
    }

    public void Clear()
    {
        previous = null;
    }

}
