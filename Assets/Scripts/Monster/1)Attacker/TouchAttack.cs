using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAttack : IAttackBehavior
{
    private float cooldown;
    private float lastAttackTime = -999f;
    private int damage;

    public TouchAttack(int damage, float cooldown)
    {
        this.damage = damage;
        this.cooldown = cooldown;
    }

    public void Attack(Transform monster, Transform player)
    {
        if (Time.time - lastAttackTime < cooldown)
            return;

        float dist = Vector2.Distance(monster.position, player.position);
        if (dist < 0.5f) // ������ ������
        {
            Debug.Log("�����ġ�� ����!");
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}