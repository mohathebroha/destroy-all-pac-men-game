using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D playerRigidBody;
    public Animator animator;

    private Node currentNode, nextNode;
    private Vector2 movement;
    private Vector2 nextMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
