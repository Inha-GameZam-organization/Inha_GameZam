using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBullet : MonoBehaviour
{
    //[SerializeField] ParticleSystem trailEffect;
    ObjectPool<MonsterBullet> objectPool;
    WaitForSeconds bulletLifeTime;
    Rigidbody2D rb;
    float speed = 10f; // ���� �Ѿ��� ������ ��
    int damage = 10;
    float lifeTime = 6f;
    bool isReleased = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletLifeTime = new WaitForSeconds(lifeTime);
    }

    private void OnEnable()
    {
       // trailEffect?.Play();
    }

    private void OnDisable()
    {
        //trailEffect?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Init(ObjectPool<MonsterBullet> objectPool)
    {
        this.objectPool = objectPool;
    }

    public void Fire(Vector2 dir)
    {
        isReleased = false;
        rb.velocity = dir.normalized * speed;
        StartCoroutine(ReturnBullet());
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        objectPool.Release(this);
    }

    IEnumerator ReturnBullet()
    {
        yield return bulletLifeTime;
        rb.velocity = Vector2.zero;
        objectPool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ���߰� ó��
        if (collision.CompareTag("Player"))
        {
            // ������ ó��
            if (collision.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
            }

            rb.velocity = Vector2.zero;
            if (!isReleased)
            {
                isReleased = true;
                objectPool.Release(this);
            }
        }
        // ��, �ٴ� �� ��Ÿ ��� ���� ������Ʈ���� ����
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            if (!isReleased)
            {
                isReleased = true;
                objectPool.Release(this);
            }
        }
        // �� �ܿ� ���� (������ �浹 X, �ٸ� �Ѿ� ��)
    }
}