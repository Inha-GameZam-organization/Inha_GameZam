using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    public enum MonsterState    //���� ����
    {
        Patrol,    // ���� ���� 
        Watch,     // ���� ����
        Chase,     // ���� ����
    }

    public float speed = 2f; // Speed of the monster
    public Transform groundCheck;     //�ٴ� ����
    public Transform wallCheck;       //�� ����
    public LayerMask groundLayer;     // Layer for ground detection
    

    // ���� ������ ����
    public float watchRange = 5f; // �÷��̾� ���� ����(���ÿ�)
    public float chaseRange = 3f; // �÷��̾� ���� ����(������)
    public float chaseSpeed = 3f; // ���� �ӵ�
    
    public float damagecooldown = 1f; // ���� ��Ÿ��

    private MonsterState state = MonsterState.Patrol; // ���� ����
    private bool movingDirectionRight = true;

    private Rigidbody2D rb;
    private Vector2 lastPosition;

    private float moveDistance = 0f;  // �̵� �Ÿ� ������
    private float maxDistance = 5f;    // �󸶳� ���� ���Ƽ��� (���� ����)

    private Transform player;         // �÷��̾� ��ġ (������)
    private float lastAttackTime = -999f; // ������ ���� �ð� (���� ��Ÿ�ӿ�)
    private MonsterStats stats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        stats = GetComponent<MonsterStats>(); // ���� ���� ������Ʈ ��������
        lastPosition = transform.position; // ���� ��ġ ����
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ��ġ ��������
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.y - player.position.y) > 1.0f)
        {
            Patrol(); // �÷��̾ �ʹ� ���ų� ������ ���� ���� ����
            return; // �÷��̾ �ʹ� ���ų� ������ ����
        }
        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x); // �÷��̾���� �Ÿ� ���
        switch (state)
        {
            case MonsterState.Patrol:
                Patrol(); // ���� ������ �� Patrol �Լ� ȣ��
                if (distanceToPlayer < watchRange) // �÷��̾ ���� ������ ������ ���� ���·� ��ȯ
                {
                    ChangeState(MonsterState.Watch);
                }
                break;
            case MonsterState.Watch:
                Watch(); // ���� ������ �� Watch �Լ� ȣ��
                if (distanceToPlayer < chaseRange)
                {
                    ChangeState(MonsterState.Chase); // �÷��̾ ���� ������ ������ ���� ���·� ��ȯ
                }
                else if (distanceToPlayer >= watchRange) // �÷��̾ ���� ������ ����� ���� ���·� ���ư�
                {
                    ChangeState(MonsterState.Patrol);
                }
                break;
            case MonsterState.Chase:
                Chase(); // ���� ������ �� Chase �Լ� ȣ��
                if (distanceToPlayer >= watchRange) // �÷��̾ ���� ������ ����� ���� ���·� ��ȯ
                {
                    ChangeState(MonsterState.Watch);
                }
                break;
        }

    }
    void Patrol()
    {
        //�̵����� ����
        float dir = movingDirectionRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        //�ٴ� üũ
        Vector2 groundCheckPos = groundCheck.position + Vector3.right * (movingDirectionRight ? 0.3f : -0.3f);
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckPos, Vector2.down, 0.1f, groundLayer);
        bool isGround = groundHit.collider != null;

        //�� üũ
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * (movingDirectionRight ? 1f : -1f), 0.1f, groundLayer);
        bool isWall = wallHit.collider != null;

        //�̵� �Ÿ� üũ

        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        moveDistance += distanceMoved;
        lastPosition = transform.position;

        if (!isGround || isWall || moveDistance >= maxDistance)
        {
            Flip();
            moveDistance = 0f;
        }

        // ����׿� ���� �ð�ȭ
        Debug.DrawRay(groundCheckPos, Vector2.down * 0.1f, Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * (movingDirectionRight ? 0.1f : -0.1f), Color.blue);
    }
    void ChangeState(MonsterState newState)
    {
        state = newState;
    }
    void Watch()
    {
        if (player.position.x < transform.position.x && movingDirectionRight)
            Flip();
        else if (player.position.x > transform.position.x && !movingDirectionRight)
            Flip();
    }

    void Chase()
    {
        if (player.position.x < transform.position.x && movingDirectionRight)
            Flip();
        else if (player.position.x > transform.position.x && !movingDirectionRight)
            Flip();

        Vector2 groundCheckPos = groundCheck.position + Vector3.right * (movingDirectionRight ? 0.3f : -0.3f);
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckPos, Vector2.down, 0.1f, groundLayer);
        bool isGround = groundHit.collider != null;

        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * (movingDirectionRight ? 1f : -1f), 0.1f, groundLayer);
        bool isWall = wallHit.collider != null;

        if (!isGround || isWall)
        {
            rb.velocity = Vector2.zero; // ���߱�
            return; // �� �̻� ���� ����
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * chaseSpeed, rb.velocity.y);
    }

    void Flip()
    {
        movingDirectionRight = !movingDirectionRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // x�� ����
        transform.localScale = scale;

    }
}
