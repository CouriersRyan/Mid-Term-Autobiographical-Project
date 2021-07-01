using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum Fight
    {
        Idle,
        Jab,
        Kick
    }
    
    //Inputs:
    [SerializeField] private KeyCode key0 = KeyCode.J;
    [SerializeField] private KeyCode key1 = KeyCode.K;
    [SerializeField] private KeyCode keyJump = KeyCode.Space;

    [SerializeField] private float moveSpd = 2;
    [SerializeField] private float jumpForce = 5;

    private bool _isInAction = false;
    private bool _isGrounded = false;
    
    private Rigidbody2D _rb;

    private GameObject _frame = null;

    [SerializeField] private Fight action = Fight.Idle;
    [SerializeField] private int frameCount = 0;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var axisX = Input.GetAxisRaw("Horizontal");
        var input0 = Input.GetKey(key0);
        var input1 = Input.GetKey(key1);
        var inputJump = Input.GetKey(keyJump);
        
        PlayerMove(axisX, inputJump);
        
        PlayerFight(input0, input1);

        if (_isInAction)
        {
            if(_frame) _frame.SetActive(false);
            //TODO: Find and store frames in start so that this is more efficient.
            var temp = transform.Find(GetStringFromFight(action)).Find("Frame" + frameCount);
            _frame = temp ? temp.gameObject : null;

            if (_frame)
            {
                _frame.SetActive(true);
                frameCount++;
            }
            else
            {
                _isInAction = false;
                frameCount = 0;
            }
        }
    }

    private void PlayerMove(float axisX, bool jump)
    {
        if (axisX != 0 && !_isInAction)
        {
            _rb.velocity = new Vector2(axisX * moveSpd, _rb.velocity.y);
        }

        if (jump && !_isInAction && _isGrounded)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void PlayerFight(bool input0, bool input1)
    {
        if (input0 && !_isInAction)
        {
            _isInAction = true;
            action = Fight.Jab;
        }
        
        if (input1 && !_isInAction)
        {
            _isInAction = true;
            action = Fight.Kick;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    private string GetStringFromFight(Fight action)
    {
        switch (action)
        {
            case Fight.Idle:
                return "Idle";
            
            case Fight.Jab:
                return "Jab";
            
            case Fight.Kick:
                return "Kick";              
            default:
                return null;
        }
    }
}
