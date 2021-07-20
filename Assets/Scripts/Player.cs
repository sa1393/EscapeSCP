using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LifeObject
{
    float moveSpeed = 1000f;
    float currentMoveSpeed = 1000f;

    float jumpForce = 2000f;

    public bool timeFast = false;
    public float timeFastNumber = 1.5f;

    bool canAttack = true;

    float currentAttackDelay = 0;
    float attackDelay = 0.5f;
    //ü��
    public int hp;
    //���ݷ�
    public int attackDamage;

    private Rigidbody2D rigid;
    private Animator animator;
    public Collider2D playerCollider;

    public PlayerAttackEffect effect;

    //������
    private bool isJump = false;
    //���� Ƚ��
    private int jumpCount = 0;
    //���� �÷��̾ �������� �ٶ󺸰� �ִ°�?
    private bool isRight = true;
    //�÷��̾ ���� ���� ��� �ִ°�?
    private bool isGround = true;
    //�÷��̾ ���� ��ų�� ����� �� �ִ°�?
    private bool skillEnable = true;

    private bool attacking = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Init();
        effect = transform.GetChild(0).GetComponent<PlayerAttackEffect>();
    }
    private void Init() {
        hp = 100;
        attackDamage = 1;
    }

    void Update()
    {
        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
            }
        }

        if (Input.GetButton("Jump") && !isJump)
        {
            Jump();
        }
        //����
        if (Input.GetMouseButton(0) && canAttack && !attacking)
        {
            PlayerAttack();
        }

        if(skillEnable)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                skillEnable = false;
                StartCoroutine(TimeStop());
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                skillEnable = false;
                StartCoroutine(TimeSlow());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                skillEnable = false;
                StartCoroutine(TimeFast());
            }
        }
    }

    private void FixedUpdate()
    {
        animator.SetBool("isGround", isGround);
        GroundCheck();
        PlayerMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.transform.parent.GetChild(0).GetComponent<Enemy>();
        if (collision.gameObject.tag == "EnemyAttack")
        {

            OnDamaged(enemy.attackDamage);
        }
    }

    //�� üũ
    void GroundCheck()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 110f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 110f, LayerMask.GetMask("Floor"));

        if (rayHit.collider != null)
        {
            isJump = false;
            isGround = true;
        } else
        {
            isGround = false;
        }
    }

    IEnumerator TimeStop()
    {
        GameManager.Instance.TimeStop();

        yield return new WaitForSeconds(3f);

        GameManager.Instance.TimeStop();
        skillEnable = true;
    }

    IEnumerator TimeSlow()
    {
        GameManager.Instance.TimeSlow();

        yield return new WaitForSeconds(5f);

        GameManager.Instance.TimeSlow();
        skillEnable = true;
    }

    IEnumerator TimeFast()
    {
        currentMoveSpeed = currentMoveSpeed * timeFastNumber;
        animator.speed = animator.speed * timeFastNumber;

        yield return new WaitForSeconds(5f);

        currentMoveSpeed = currentMoveSpeed / timeFastNumber;
        animator.speed = animator.speed / timeFastNumber;

        skillEnable = true;
    }

    //�÷��̾� �̵�
    void PlayerMove()
    {

        float axis_X = Input.GetAxisRaw("Horizontal");

        rigid.velocity = new Vector2(axis_X * currentMoveSpeed, rigid.velocity.y);

        if(!attacking )
        {
            if (isRight && axis_X == -1)
            {
                Flip();
            }

            if (!isRight && axis_X == 1)
            {
                Flip();
            }

        }

        //�ִϸ��̼� �Ķ���� ����
        animator.SetFloat("axis_X", Mathf.Abs(axis_X));
    }

    //�÷��̾� ����
    void Jump()
    {
        isJump = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //�ִϸ��̼� �Ķ���� ����
        animator.SetTrigger("isJump");
    }

    //�÷��̾� �ǰ� �̺�Ʈ
    void OnDamaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            StartCoroutine("Die");
        }else
        {
            animator.SetTrigger("isHit");

        }


    }

    void PlayerAttack()
    {
        if (!canAttack) return;
        attacking = true;
        animator.SetTrigger("isAttack");
        Debug.Log("�÷��̾� ����");
    }

    //�÷��̾� ��������Ʈ ��ȯ
    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator Die()
    {
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }


    //�ִϸ��̼� �̺�Ʈ �Լ���

    void PlayerAttackOff()
    {
        attacking = false;
        canAttack = false;
    }

    public void PlayerAttackEffect()
    {
        effect.damage = attackDamage;
        effect.animator.SetTrigger("isAttack");
    }

}
