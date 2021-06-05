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
    public Collider2D playerCollider;



    //������
    private bool isJump = false;
    //���� Ƚ��
    private int jumpCount = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
    }

    private void Init() {
        hp = 100;
        attackDamage = 10;
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJump)
        {
            jump();

        }

    }

    private void FixedUpdate()
    {
        groundCheck();
        playerMove();
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    //�� üũ
    void groundCheck()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 110f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 110f, LayerMask.GetMask("Floor"));

        if (rayHit.collider != null)
        {
            isJump = false;
        }
    }

    //�÷��̾� �̵�
    void playerMove()
    {
        float axis_X = Input.GetAxis("Horizontal");
        if(axis_X != 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.AddForce(Vector2.right * axis_X * moveSpeed);

        }


        // �ӵ� ����
        if (rigid.velocity.x >= moveMaxSpeed)
        {
            rigid.velocity = new Vector2(2.5f, rigid.velocity.y);

        }
        else if (rigid.velocity.x <= moveMaxSpeed * -1)
        {
            rigid.velocity = new Vector2(-2.5f, rigid.velocity.y);
        }
    }

    //�÷��̾� ����
    void jump()
    {
        isJump = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
    }


}
