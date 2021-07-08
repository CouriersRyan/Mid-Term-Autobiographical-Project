using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpd = 3;
    [SerializeField] private float rayLength = 10f;
    
    private FighterAction enemyAction;
    private bool _isMove = true;

    [SerializeField] private GameObject player;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite hurt;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    
    private int _stunned;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_stunned <= 0)
        {
            FindPlayer();
            _sr.sprite = normal;

            var axisX = _isMove ? Mathf.Sign(player.transform.position.x - transform.position.x) : 0;

            var input0 = false;
            var input1 = false;

            if (axisX != 0)
            {
                _rb.velocity = new Vector2(axisX * moveSpd, _rb.velocity.y);
                transform.localScale = new Vector3(-Mathf.Sign(axisX), 1, 1);
            }
        }
        else
        {
            _sr.sprite = hurt;
            _stunned--;
        }
    }
    private void FindPlayer()
    {
        var dir = -Mathf.Sign(transform.localScale.x) * Vector2.right;
        var origin = new Vector2(transform.position.x, transform.position.y + 1f);
        LayerMask mask = LayerMask.GetMask("Player");

        RaycastHit2D ray = Physics2D.Raycast(origin, dir, rayLength, mask);
        
        Debug.DrawRay(origin, dir * rayLength, Color.red);

        if (ray.collider != null)
        {
            Debug.Log("Range Reached!");
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                _isMove = false;
            }
        }
        else
        {
            _isMove = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_stunned <= 0)
        {
            if (other.CompareTag("PlayerAttack"))
            {
                Debug.Log("Hit.");
                var attackData = other.GetComponentInParent<AttackData>();
                _stunned = attackData.stunFrames;
                _rb.AddForce(-Mathf.Sign(other.transform.position.x - transform.position.x) * Vector2.right * 
                             attackData.enemyKnockback,
                    ForceMode2D.Impulse);
            }
        }
    }
}
