using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveWatch : IWatchBehavior
{
    private float chaseRange;

    public PassiveWatch(float chaseRange)
    {
        this.chaseRange = chaseRange;
    }

    public void Watch(Transform monster, Transform player, Rigidbody2D rb)
    {
        // �ٶ󺸱⸸ �ϰ� �� ������
        if (player.position.x < monster.position.x && monster.localScale.x > 0)
            monster.localScale = new Vector3(-1, 1, 1);
        else if (player.position.x > monster.position.x && monster.localScale.x < 0)
            monster.localScale = new Vector3(1, 1, 1);

        rb.velocity = Vector2.zero;
    }

    public bool ShouldChase(Transform monster, Transform player)
    {
        return Mathf.Abs(player.position.x - monster.position.x) < chaseRange;
    }

    public void OnEnterWatch()
    {
        // �ƹ� �ʱ�ȭ �ʿ� ����
    }
}