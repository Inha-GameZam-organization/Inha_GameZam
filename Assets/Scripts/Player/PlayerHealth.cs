using System.Collections;
using UnityEngine;
public class PlayerHealth : MonoBehaviour, IHealth
{
    int maxHealth = 100;
    int currentHealth;
    float unbeatableTime = 1f;
    float knockbackForce = 10f;
    float blinkRate = 0.1f;
    bool unbeatable = false;
    WaitForSeconds GetWaitForSeconds;
    WaitForSeconds GetBlinkSeconds;
    
    Coroutine blinkCoroutine;
    Rigidbody2D rb;

    MeshRenderer[] meshRenderers;
    [SerializeField] HealthBar healthBar;
    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        GetWaitForSeconds = new WaitForSeconds(unbeatableTime);
        GetBlinkSeconds = new WaitForSeconds(blinkRate);
        rb = GetComponent<Rigidbody2D>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // ü�� ���� �Լ� => IHealth �������̽��� ���� ȣ��
    public void TakeDamage(int damage)
    {
        // �����̶�� �Ѿ��
        if (unbeatable)
        {
            return;
        }
        

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth);

        KnockBack();
        SoundManager.Instance.PlaySound(SoundType.GetHit);


        StartCoroutine(SetUnbeatable());
    }

    // �������� �Ծ��� �� �з����� �Լ�
    void KnockBack()
    {
        rb.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
    }

    // ���� ���� �Լ�
    IEnumerator SetUnbeatable()
    {
        unbeatable = true;
        blinkCoroutine = StartCoroutine(Blink());
        Physics2D.IgnoreLayerCollision(9, 13, true);
        yield return GetWaitForSeconds;
        StopCoroutine(blinkCoroutine);
        Physics2D.IgnoreLayerCollision(9, 13, false);
        foreach(var mesh in meshRenderers)        
            mesh.material.color = Color.white;
        unbeatable = false;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            foreach (var mesh in meshRenderers)
                mesh.material.color = Color.gray;
            yield return GetBlinkSeconds;
            foreach (var mesh in meshRenderers)
                mesh.material.color = Color.white;
            yield return GetBlinkSeconds;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Item item))
        {
            item.Use(this.gameObject);
        }
    }
}
