using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    //TODO: Put in all of the attacks.
    [SerializeField] public float enemyKnockback = 5.0f;
    [SerializeField] public float selfKnockback = 0.1f;
    [SerializeField] public int stunFrames = 15;
}
