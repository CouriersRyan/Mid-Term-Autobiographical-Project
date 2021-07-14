using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private float groundRayLength = 5f;
    
    private FighterAction enemyAction;
    private bool _isMove = true;

    [SerializeField] private GameObject player;
    [SerializeField] private int randomFrames = 100;
    [SerializeField] private int framesMax = 50;
    [SerializeField] private int framesMin = 20;
    
    // Start is called before the first frame update
    private void Start()
    {
        enemyAction = GetComponent<FighterAction>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        FindPlayer();
        CheckGroundFront();
        
        var axisX = _isMove ? Mathf.Sign(player.transform.position.x - transform.position.x) : 0;
        
        var input0 = false;
        var input1 = false;

        if (randomFrames <= 0)
        {
            if (Random.Range(0, 2) == 1)
            {
                input0 = true;
            }
            else
            {
                input1 = true;
            }
            randomFrames = Random.Range(framesMin, framesMax);
        }
        else
        {
            randomFrames--;
        }
        
        enemyAction.DoActions(axisX, false, input0, input1);
    }
    private void FindPlayer()
    {
        var dir = Mathf.Sign(transform.localScale.x) * Vector2.right;
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

    private void CheckGroundFront()
    {
        var dir =  new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * 1, -1);
        var origin = new Vector2(transform.position.x, transform.position.y + 1f);
        LayerMask mask = LayerMask.GetMask("Ground");

        RaycastHit2D ray = Physics2D.Raycast(origin, dir, groundRayLength, mask);

        Debug.DrawRay(origin, dir * groundRayLength, Color.blue);

        if (ray.collider == null)
        {
            Debug.Log("Cliff Ahead");
            _isMove = false;
        }
    }
}
