using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    public float speed = 2f; // Speed of the monster
    public Transform groundCheck;     //�ٴ� ����
    public Transform wallCheck;       //�� ����
    public LayerMask groundLayer;     // Layer for ground detection
    
    private Rigidbody2D rb; 
    private bool movingDirectionRight = true;

    private float moveDistance = 0f;  // �̵� �Ÿ� ������
    private Vector2 lastPosition;
    public float maxDistance = 5f;    // �󸶳� ���� ���Ƽ��� (���� ����)
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        lastPosition = transform.position; // ���� ��ġ ����
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
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
    void Flip()
    {
        movingDirectionRight = !movingDirectionRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // x�� ����
        transform.localScale = scale;
    }
}
