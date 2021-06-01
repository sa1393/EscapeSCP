using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LifeObject
{
    private GameObject target;
    private BoxCollider2D enemyFloor;

    public int hp;
    //���ݷ�
    public int attackDamage;
    //��Ÿ�
    public int range;
    //�̵��ӵ�
    public float moveSpeed;
    //���ݼӵ�
    public float attackSpeed;

    private void Awake()
    {
        enemyFloor = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

}
