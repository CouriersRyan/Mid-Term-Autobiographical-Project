using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //TODO: Make audio as a separate script and use events to have sounds play when player takes an action.
    // 'SampleEvent?.Invoke()'
    
    /*TODO: Break up existing code for better architecture. Split up Code so that the inputs are separate from the movement.
      That way, we can have one movement code, with enemy and player having their own inputs.
     */

    private FighterAction playerAction;

    //Inputs:
    [SerializeField] private KeyCode key0 = KeyCode.J;
    [SerializeField] private KeyCode key1 = KeyCode.K;
    [SerializeField] private KeyCode keyJump = KeyCode.Space;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerAction = GetComponent<FighterAction>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var axisX = Input.GetAxisRaw("Horizontal");
        var input0 = Input.GetKey(key0);
        var input1 = Input.GetKey(key1);
        var inputJump = Input.GetKey(keyJump);
        
        playerAction.DoActions(axisX, inputJump, input0, input1);
    }
    
}
