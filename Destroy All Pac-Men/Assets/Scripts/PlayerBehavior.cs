using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerBehavior : MonoBehaviour
{
    #region PROPERTIES

    public float moveSpeed = 5f;
    public Rigidbody2D playerRigidBody;
    public Animator animator;

    private Node currentNode, nextNode;
    private Vector2 movement;
    private Vector2 nextMovement;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;
    #endregion

    #region MONOBEHAVIOUR_METHODS
    // Start is called before the first frame update
    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
            currentNode.previous = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        CheckInputChange();
        SetAnimation();
    }

    //Called every fixed framerate frame
    private void FixedUpdate()
    {
        HandleMove();

    }

    #endregion

    #region HELPERS

    
    private void SetAnimation()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    #region Movement_Logic

    private void CheckInputChange()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0f && !xAxisInUse)
        {
            xAxisInUse = true;
            ChangeDirection(new Vector2(xInput, 0));
        }
        else if (xInput == 0)
        {
            xAxisInUse = false;
        }
        if (yInput != 0f && !yAxisInUse)
        {
            yAxisInUse = true;
            ChangeDirection(new Vector2(0, yInput));
        }
        else if (yInput == 0)
        {
            yAxisInUse = false;
        }

    }

    private bool ChangeDirection(Vector2 direction)
    {
        if (direction != movement)
        {
            nextMovement = direction; 
        }
        if (currentNode != null)
        {
            Node desiredNode = CanMove(direction);
            if (desiredNode != null)
            {
                movement = direction;
                nextNode = desiredNode;
                nextNode.previous = currentNode;
                currentNode = null;
                return true;
            }
        }
        return false;
    }

    private Node CanMove(Vector2 direction)
    {
        for (int i = 0; i < currentNode.adjacentNodes.Count; ++i)
        {
            if (currentNode.validDirections[i].ToString() == direction.normalized.ToString())
            {
                return currentNode.adjacentNodes[i];
            }
        }
        return null;
    }

    private void HandleMove()
    {
        if (nextNode != currentNode && nextNode != null)
        {
            if (nextMovement == movement * -1)
            {
                movement *= -1;
                Node tempNode = nextNode;
                nextNode = nextNode.previous;
                nextNode.previous = tempNode;
            }
            if (OverShotNextNode())
            {
                if (!CheckTeleport())
                {
                    playerRigidBody.MovePosition(nextNode.transform.position);
                    currentNode = nextNode;
                }

                if (!ChangeDirection(nextMovement))
                {
                    if (!ChangeDirection(movement))
                    {
                        movement = Vector2.zero;
                    }
                }

            }
            else
            {
                playerRigidBody.MovePosition(playerRigidBody.position + (movement * moveSpeed * Time.fixedDeltaTime));
            }
        }
    }
    #endregion

    #region Node_Logic

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject nodeTile = GameObject.Find("GameGraph").GetComponent<GameGraph>().graph[(int)pos.x, (int)pos.y];
        if (nodeTile == null) return null;
        return nodeTile.GetComponent<Node>();

    }

    private bool CheckTeleport()
    {
        if (nextNode.isTeleport)
        {
            playerRigidBody.MovePosition(nextNode.teleportReceiver.transform.position);
            currentNode = nextNode.teleportReceiver;
            return true;
        }
        return false;
    }

    bool OverShotNextNode()
    {
        float distanceFromTarget = DistanceToNode(nextNode.transform.position);
        float distanceFromSelf = DistanceToNode(transform.position);
        return distanceFromSelf > distanceFromTarget;
    }

    float DistanceToNode(Vector2 target)
    {
        Vector2 distanceVector = target - (Vector2)nextNode.previous.transform.position;
        return distanceVector.sqrMagnitude;
    }
    #endregion

    #endregion

}
