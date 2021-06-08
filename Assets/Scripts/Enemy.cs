using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LifeObject
{
    private GameObject target;
    private BoxCollider2D enemyFloor;
    public Player player;
    protected Rigidbody2D enemyRigid;

    public int hp;
    //���ݷ�
    public int attackDamage;
    //��Ÿ�
    public int range;
    //�̵��ӵ�
    public float moveSpeed = 1;
    //���ݼӵ�
    public float attackSpeed;

    protected void Awake()
    {
        enemyFloor = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(enemyFloor, player.playerCollider);
        enemyRigid = GetComponent<Rigidbody2D>();
    }

    //������ �Ÿ� �̵�
    protected void Move()
    {
        enemyRigid.velocity = new Vector2(100 * -moveSpeed, enemyRigid.velocity.y);
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);
    protected abstract void Die();
}
