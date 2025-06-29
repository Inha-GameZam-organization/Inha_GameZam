using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmAimer : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    //private Transform armPivot; // ȸ�� �߽�(�� ������), ���� �ڽ��� Ʈ������

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (player == null) return;

        // 1. �÷��̾� ���� ���
        Vector2 dir = player.position - transform.position;
        // 2. ���� ��� (2D)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 3. z�� ȸ�� ���� (2D)
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
