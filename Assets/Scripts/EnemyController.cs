using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    enum Fight
    {
        Idle,
        Jab,
        Kick
    }
    
    [SerializeField] private float moveSpd = 2;
    [SerializeField] private float jumpForce = 5;

    private bool _isInAction = false;
    private bool _isGrounded = false;
    private bool _isMove = true;
    private int _Stunned = 0;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;

    private int _isWalkingID;
    private int _jumpID;

    private GameObject _frame = null;

    [SerializeField] private GameObject player;

    [SerializeField] private Fight action = Fight.Idle;
    [SerializeField] private int frameCount = 0;
    [SerializeField] private int randomFrames = 100;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _isWalkingID = Animator.StringToHash("isWalking");
        _jumpID = Animator.StringToHash("Jump");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_Stunned <= 0)
        {
            _anim.speed = 1;
            var axisX = Mathf.Sign(player.transform.position.x - transform.position.x);

            var input0 = false;
            var input1 = false;
            if (Random.Range(0, 2) == 1)
            {
                input0 = true;
            }
            else
            {
                input1 = true;
            }

            PlayerMove(axisX, false);

            if (randomFrames <= 0)
            {
                PlayerFight(input0, input1);
                randomFrames = Random.Range(50, 80);
            }
            else
            {
                randomFrames--;
            }

            if (_isInAction)
            {
                if (_frame) _frame.SetActive(false);
                //TODO: Find and store frames in start so that this is more efficient.
                var temp = transform.Find(GetStringFromFight(action)).Find("Frame" + frameCount);
                _frame = temp ? temp.gameObject : null;

                if (_frame)
                {
                    _frame.SetActive(true);
                    frameCount++;
                    _spriteRenderer.enabled = false;
                }
                else
                {
                    _isInAction = false;
                    frameCount = 0;
                    _spriteRenderer.enabled = true;
                }
            }
        }
        else
        {
            _isInAction = false;
            frameCount = 0;
            _spriteRenderer.enabled = true;
            if (_frame) _frame.SetActive(false);
            _Stunned--;
            randomFrames--;
            _anim.speed = 0;
        }
    }

    private void PlayerMove(float axisX, bool jump)
    {
        if (axisX != 0 && !_isInAction && _isMove)
        {
            _rb.velocity = new Vector2(axisX * moveSpd, _rb.velocity.y);
            _anim.SetBool(_isWalkingID, true);
            transform.localScale = new Vector3(Mathf.Sign(axisX), 1, 1);
        }
        else
        {
            _anim.SetBool(_isWalkingID, false);
        }

        if (jump && !_isInAction && _isGrounded)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _anim.SetBool(_jumpID, true);
        }
    }

    private void PlayerFight(bool input0, bool input1)
    {
        if (_isGrounded)
        {
            if (input0 && !_isInAction)
            {
                _isInAction = true;
                action = Fight.Jab;
                _rb.AddForce(-0.1f * Mathf.Sign(transform.localScale.x) * Vector2.right, ForceMode2D.Impulse);
            }

            if (input1 && !_isInAction)
            {
                _isInAction = true;
                action = Fight.Kick;
                _rb.AddForce(-0.1f * Mathf.Sign(transform.localScale.x) * Vector2.right, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _anim.SetBool(_jumpID, false);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            _isMove = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _anim.SetBool(_jumpID, true);
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            _isMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_Stunned <= 0)
        {
            if (other.CompareTag("PlayerJab"))
            {
                _Stunned = 15;
                _rb.AddForce(-Mathf.Sign(other.transform.position.x - transform.position.x) * Vector2.right * 5,
                    ForceMode2D.Impulse);
                //Get Stunned
            }

            if (other.CompareTag("PlayerKick"))
            {
                _Stunned = 20;
                _rb.AddForce(-Mathf.Sign(other.transform.position.x - transform.position.x) * Vector2.right * 7.5f,
                    ForceMode2D.Impulse);
                //Get Stunned
            }
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
