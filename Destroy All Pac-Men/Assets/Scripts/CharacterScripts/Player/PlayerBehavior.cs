using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInputController))]
public class PlayerBehavior : MonoBehaviour
{
    private PlayerInputController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        controller.CheckInputChange();
    }


}
