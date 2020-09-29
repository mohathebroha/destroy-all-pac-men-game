using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimationBehavior))]
public class CharacterNodeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D characterRigidBody;
    private Node currentNode, nextNode;
    private Vector2 movement, nextMovement;
    private CharacterAnimationBehavior characterAnimator;

    private void Awake()
    {
        characterRigidBody = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<CharacterAnimationBehavior>();
    }

    void Start()
    {
        Node node = GetNodeAtPosition(transform.position);
        if (node != null)
        {
            currentNode = node;
            currentNode.previous = null;
        }
    }

    private void Update()
    {
        characterAnimator.SetMoveAnimation(movement);
    }
    private void FixedUpdate()
    {
        HandleMove();
    }

    #region MOVEMENT_LOGIC
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
                    characterRigidBody.MovePosition(nextNode.transform.position);
                    currentNode = nextNode;
                }

                if (!ChangeDirection(nextMovement) && !ChangeDirection(movement))
                {
                    movement = Vector2.zero;
                }

            }
            else
            {
                characterRigidBody.MovePosition(characterRigidBody.position + (movement * moveSpeed * Time.fixedDeltaTime));
            }
        }
    }

    public bool ChangeDirection(Vector2 direction)
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

    private bool CheckTeleport()
    {
        if (nextNode.isTeleport)
        {
            characterRigidBody.MovePosition(nextNode.teleportReceiver.transform.position);
            currentNode = nextNode.teleportReceiver;
            return true;
        }
        return false;
    }
    #endregion

    #region NODE_LOGIC
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
    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject nodeTile = GameObject.Find("GameGraph").GetComponent<GameGraph>().graph[(int)pos.x, (int)pos.y];
        if (nodeTile == null) return null;
        return nodeTile.GetComponent<Node>();

    }
    #endregion
}
