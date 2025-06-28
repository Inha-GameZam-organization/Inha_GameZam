using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperHalfCircleDetector : IPlayerDetector
{
    private float radius;

    public UpperHalfCircleDetector(float radius)
    {
        this.radius = radius;
    }

    public bool IsPlayerDetected(Transform monster, Transform player)
    {
        Vector2 dir = player.position - monster.position;

        // ��ü �� �ȿ� ������
        if (dir.magnitude < radius)
        {
            // ����(��ݱ�)�� ���� ���� ����
            return dir.y >= 0;
        }

        return false;
    }
}