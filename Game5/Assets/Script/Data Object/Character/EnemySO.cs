using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    Bat,
    Boar,
    Boss,
    FireTotem,
    FlyingMelee,
    LittleRange,
    LittleMelee,
    Skeleton,
}
[CreateAssetMenu]
public class EnemySO: ScriptableObject 
{
    [Header("Stats Enemy")]
    public float heath;
    public float damage;
    public float attackTimer;
    public float alertRange;
    public float range;
    public float builetSpeed;
    [Header("Mood Enemy")]
    [SerializeField] public TypeEnemy Type;
    [SerializeField] public bool isDead;
    [SerializeField] public bool isAlert;
    [SerializeField] public bool isAttack;

    [Header("Reward")]
    [SerializeField] public int goldReward;
    [SerializeField] public int expReward;
    
}
