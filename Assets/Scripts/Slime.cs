using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    //���� �ڽ��� �������� �ٶ󺸰� �ִ°�?
    private bool isRight = false;

    private Animator animator;

    //���ݷ�
    //��Ÿ�
    //�̵��ӵ�
    //���ݼӵ�

    private void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        enemyRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        Init();
    }

    private void FixedUpdate()
    {
        //Move();
    }

    protected override void Init()
    {
        hp = 100;
        range = 1;
        moveSpeed = 5f;
        attackDamage = 10;
        attackSpeed = 4f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "PlayerAttack")
        {
            PlayerAttackInfo tempPlayer = gameObject.GetComponent<PlayerAttackInfo>();
            
            if (tempPlayer != null)
            {
                Debug.Log(tempPlayer.attackDamage);
                Hit(tempPlayer.attackDamage);
            }
        }
    }

    protected override void Hit(int damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            Die();
        }
        Debug.Log("����");

    }

    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}