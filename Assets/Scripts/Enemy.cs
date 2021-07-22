using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : LifeObject
{
    const string animBaseLayer = "Base Layer";

    private GameObject target;
    public Collider2D enemyCollider;
    private Collider2D enemyFloor;
    public Player player;
    protected GameObject effect;
    protected Rigidbody2D enemyRigid;
    protected Animator animator;
    protected Animator effectAnimator;

    //�ӽ�
    protected SpriteRenderer sr;

    public float timeSlowNumber = 0.2f;

    //���� �ڽ��� �������� �ٶ󺸰� �ִ°�?
    protected bool isRight = false;

    public int hp;
    //���ݷ�
    public int attackDamage;
    //��Ÿ�
    public int range;
    //�̵��ӵ�
    public float moveSpeed = 1;
    public float moveCurrentSpeed = 1;
    //���ݼӵ�
    public float attackSpeed;
    public bool isDead = false;

    public bool canHit = true;
    public float hitDelay = 1.0f;
    public float currentHitDelay = 0;

    public bool canAttack = true;
    public float attackDelay = 5.0f;
    public float currentAttackDelay = 0;

    public bool attacking = false;
    public bool hitting = false;

    protected void Awake()
    {
        enemyRigid = transform.parent.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<PolygonCollider2D>();
        enemyFloor = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();


        effect = transform.parent.GetChild(2).gameObject;
        effectAnimator = effect.GetComponent<Animator>();

        GameManager.Instance.enemies.Add(this);
        
    }

    protected void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();

        PolygonCollider2D[] colliders = player.transform.GetChild(0).GetComponent<PlayerAttackEffect>().colliders;
        Debug.Log(colliders[0]);
        if(enemyFloor != null)
        {
            Debug.Log("hihi");
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            Physics2D.IgnoreCollision(enemyFloor, colliders[i]);
        }
    }



    protected void Update()
    {
        if (!canHit)
        {
            currentHitDelay += Time.deltaTime;

            if (currentHitDelay >= hitDelay)
            {
                currentHitDelay = 0;
                canHit = true;
                //�ӽ�
                sr.color = new Color(1, 1, 1, 1f);
            }
        }
        if (GameManager.Instance.timeStop) return;

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
            }
        }
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("isAttack");
        gameObject.tag = "EnemyAttack";
        enemyCollider.isTrigger = true;
        gameObject.layer = 9;
        enemyRigid.gravityScale = 0;

    }

    protected virtual void OffAttack()
    {
        gameObject.tag = "Enemy";
        gameObject.layer = 7;
        enemyCollider.isTrigger = false;
        enemyRigid.gravityScale = 1;
    }
    //���� ����
    IEnumerator StartBaseAttack(float time)
    {
        if (canAttack && !hitting && !attacking)
        {
            float exitTime = 0.8f;
            attacking = true;
            yield return new WaitForSeconds(time);
            Attack();

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("slime_attack"))
            {
                //��ȯ ���� �� ����Ǵ� �κ�
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                //�ִϸ��̼� ��� �� ����Ǵ� �κ�
                yield return null;
            }   

            canAttack = false;
            attacking = false;
        }
    }

    public void TimeStop()
    {
        if (GameManager.Instance.timeStop)
        {
            animator.speed = 0;
            effectAnimator.speed = 0;
        }
        else
        {
            if (GameManager.Instance.timeSlow)
            {
                animator.speed = timeSlowNumber;
                effectAnimator.speed = timeSlowNumber;
            }
            else
            {
                animator.speed = 1;
                effectAnimator.speed = 1;
            }


        }
    }

    public void TimeSlow()
    {
        if (GameManager.Instance.timeStop)
        {
            if (GameManager.Instance.timeSlow)
            {
                moveCurrentSpeed = moveCurrentSpeed * timeSlowNumber;
            }
            else
            {
                moveCurrentSpeed = moveCurrentSpeed / timeSlowNumber;
            }
        }
        else
        {
            if (GameManager.Instance.timeSlow)
            {
                animator.speed = timeSlowNumber;
                effectAnimator.speed = timeSlowNumber;
                moveCurrentSpeed = moveCurrentSpeed * timeSlowNumber;
                Debug.Log("test");
            }
            else
            {
                animator.speed = 1;
                effectAnimator.speed = 1;
                moveCurrentSpeed = moveCurrentSpeed / timeSlowNumber;
            }
        }
        

    }


    protected void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.parent.localScale;
        scale.x *= -1;
        transform.parent.localScale = scale;
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);

    protected abstract IEnumerator Die(float second);
}
