using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveMaxSpeed = 30000f;
    public float moveSpeed = 45000f;
    public float jumpForce = 2000f;

    //ü��
    public int hp;
    //���ݷ�
    public int attackDamage;

    private Rigidbody2D rigid;
    private Animator animator;
    public Collider2D playerCollider;

    //������
    private bool isJump = false;
    //���� Ƚ��
    private int jumpCount = 0;
    //���� �÷��̾ �������� �ٶ󺸰� �ִ°�?
    private bool isRight = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Init() {
        hp = 100;
        attackDamage = 10;
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        PlayerMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
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
        }
    }

    //�÷��̾� �̵�
    void PlayerMove()
    {
        float axis_X = Input.GetAxis("Horizontal");

        if(axis_X != 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.AddForce(Vector2.right * axis_X * moveSpeed);
        }

        if(axis_X < 0 && isRight || axis_X > 0 && !isRight)
        {
            Flip();
        }

        //�ӵ� ����
        if (rigid.velocity.x >= moveMaxSpeed)
        {
            rigid.velocity = new Vector2(2.5f, rigid.velocity.y);
        }
        else if (rigid.velocity.x <= moveMaxSpeed * -1)
        {
            rigid.velocity = new Vector2(-2.5f, rigid.velocity.y);
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
    void OnDamaged(Vector2 targetPos)
    {
        //���̾� ����
        //Debug.Log("gg");
        //rigid.velocity = Vector2.zero;
        //int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        //Debug.Log(dirc);
        //rigid.AddForce(new Vector2(dirc, 1)*20, ForceMode2D.Impulse);

        //�ִϸ��̼� �Ķ���� ����
        animator.SetTrigger("isHit");
    }

    //�÷��̾� ��������Ʈ ��ȯ
    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
