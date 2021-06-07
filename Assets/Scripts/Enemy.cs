using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LifeObject
{
    private GameObject target;
    private BoxCollider2D enemyFloor;
    public Player player;

    public int hp;
    //���ݷ�
    public int attackDamage;
    //��Ÿ�
    public int range;
    //�̵��ӵ�
    public float moveSpeed;
    //���ݼӵ�
    public float attackSpeed;

    protected void Awake()
    {
        enemyFloor = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(enemyFloor, player.playerCollider);
    }

    protected abstract void Init();

    protected abstract void Hit();
}
