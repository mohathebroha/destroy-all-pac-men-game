using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterNodeMovement))]
public class PlayerInputController : MonoBehaviour
{
    private CharacterNodeMovement nodeMovementLogic;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;

    private void Awake()
    {
        nodeMovementLogic = GetComponent<CharacterNodeMovement>();
    }

    public void CheckInputChange()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0f && !xAxisInUse)
        {
            xAxisInUse = true;
            nodeMovementLogic.ChangeDirection(new Vector2(xInput, 0));
        }
        else if (xInput == 0)
        {
            xAxisInUse = false;
        }
        if (yInput != 0f && !yAxisInUse)
        {
            yAxisInUse = true;
            nodeMovementLogic.ChangeDirection(new Vector2(0, yInput));
        }
        else if (yInput == 0)
        {
            yAxisInUse = false;
        }

    }
}
