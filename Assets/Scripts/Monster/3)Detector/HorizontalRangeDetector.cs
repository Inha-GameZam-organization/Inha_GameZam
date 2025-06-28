using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRangeDetector : IPlayerDetector
{
    private float rangeX;
    private float rangeY;
    private Transform monsterTransform;
    public HorizontalRangeDetector(float rangeX, Transform monsterTransform, float rangeY = 1.0f)
    {
        this.rangeX = rangeX;
        this.rangeY = rangeY;
        this.monsterTransform = monsterTransform;
    }

    public bool IsPlayerDetected(Transform monster, Transform player)
    {
        float dx = player.position.x - monster.position.x;
        float dy = Mathf.Abs(player.position.y - monster.position.y);
        bool playerInFront =
            (dx > 0 && monsterTransform.localScale.x > 0) || // ������ �ٶ󺸰� �����ʿ� ����
            (dx < 0 && monsterTransform.localScale.x < 0);   // ���� �ٶ󺸰� ���ʿ� ����
        
        return Mathf.Abs(dx) < rangeX && dy < rangeY && playerInFront;
    }
}