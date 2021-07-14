using System;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    enum Fight
    {
        Jab = 0,
        Kick = 1,
        Dash = 2,
    }

    [SerializeField] private float moveSpd = 2;
    [SerializeField] private float jumpForce = 5;

    private bool _isInAction;
    private bool _isGrounded;
    private int _stunned;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;

    private FighterAudio _fighterAudio;

    private int _isWalkingID;
    private int _jumpID;

    private string _attackTypeReceived;
    private GameObject _frame;
    private GameObject[][] _allActionFrames;
    private AttackData[] _attackData;

    [SerializeField] private Fight action = Fight.Jab;
    [SerializeField] private int frameCount;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _fighterAudio = GetComponent<FighterAudio>();

        _isWalkingID = Animator.StringToHash("isWalking");
        _jumpID = Animator.StringToHash("Jump");
        
        SetAttackTypeReceived(gameObject.tag);
        
        //Creates an array of child objects that correspond to the existing enum of actions
        var numberOfActions = Enum.GetNames(typeof(Fight)).Length;
        Debug.Log(numberOfActions);
        _allActionFrames = new GameObject[numberOfActions][];
        _attackData = new AttackData[numberOfActions];
        
        for (int i = 0; i < numberOfActions; i++)
        {
            action = (Fight)i;
            var actionObject = transform.Find(GetStringFromFight(action));
            _attackData[i] = actionObject.GetComponent<AttackData>();
            if (actionObject)
            {
                var count = actionObject.childCount;
                _allActionFrames[i] = new GameObject[count];
                for (int j = 0; j < count; j++)
                {
                    _allActionFrames[i][j] = actionObject.Find("Frame" + j).gameObject;
                    Debug.Log("Frame: " + j);
                }
            }
        }
    }
    
    public void DoActions(float axisX, bool inputJump, bool input0, bool input1)
    {
        if (_stunned <= 0)
        {
            _anim.speed = 1;

            PlayerMove(axisX, inputJump);
            
            PlayerFight(input0, input1);

            if (_isInAction)
            {
                if(_frame) _frame.SetActive(false);
                if (frameCount < _allActionFrames[(int) (action)].Length)
                {
                    var temp = _allActionFrames[(int) (action)][frameCount];
                    _frame = temp ? temp : null;

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
            _stunned--;
            _anim.speed = 0;
        }
    }

    private void PlayerMove(float axisX, bool jump)
    {
        if (axisX != 0 && !_isInAction)
        {
            _rb.velocity = new Vector2(axisX * moveSpd, _rb.velocity.y);
            _anim.SetBool(_isWalkingID, true);
            var localScale = transform.localScale;
            localScale = new Vector3(Mathf.Sign(axisX) * Mathf.Sign(localScale.x) * localScale.x, localScale.y, localScale.z);
            transform.localScale = localScale;
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
            if (input0 && input1 && !_isInAction)
            {
                _isInAction = true;
                action = Fight.Dash;
                _rb.AddForce(Vector2.right * (Mathf.Sign(transform.localScale.x) * 7.5f), ForceMode2D.Impulse);
                
            }
            if (input0 && !_isInAction)
            {
                _isInAction = true;
                action = Fight.Jab;
                _rb.AddForce(-_attackData[(int)action].selfKnockback * Mathf.Sign(transform.localScale.x) * 
                             Vector2.right, ForceMode2D.Impulse);
                _fighterAudio.PlayAudio("punch");
            }

            if (input1 && !_isInAction)
            {
                _isInAction = true;
                action = Fight.Kick;
                _rb.AddForce(-_attackData[(int)action].selfKnockback  * Mathf.Sign(transform.localScale.x) * 
                             Vector2.right, ForceMode2D.Impulse);
                _fighterAudio.PlayAudio("kick");
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
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _anim.SetBool(_jumpID, true);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_stunned <= 5)
        {
            if (other.CompareTag(_attackTypeReceived))
            {
                Debug.Log("Hit.");
                var attackData = other.transform.GetComponentInParent<AttackData>();
                _stunned = attackData.stunFrames;
                _rb.velocity = Vector2.zero;
                _rb.AddForce(-Mathf.Sign(other.transform.position.x - transform.position.x) * Vector2.right * 
                             attackData.enemyKnockback,
                    ForceMode2D.Impulse);
            }
        }
    }

    private void SetAttackTypeReceived(string tag)
    {
        if (tag.Equals("Player"))
        {
            _attackTypeReceived = "EnemyAttack";
        }
        
        if (tag.Equals("Enemy"))
        {
            _attackTypeReceived = "PlayerAttack";
        }
    }

    private string GetStringFromFight(Fight action)
    {
        switch (action)
        {
            case Fight.Jab:
                return "Jab";
            
            case Fight.Kick:
                return "Kick";
            case Fight.Dash:
                return "Dash";
            default:
                return null;
        }
    }
}
