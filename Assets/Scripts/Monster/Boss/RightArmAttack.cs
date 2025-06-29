using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmAttack : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] ObjectPooling objectPool;   // ������Ʈ Ǯ �Ҵ�
    [SerializeField] Transform firePoint;        // �ѱ� ��ġ �� ������Ʈ
    public int bulletCount = 3;
    public float shootInterval = 3f;           // �Ѿ� ����
=======
    [SerializeField] ObjectPooling objectPool;   // ������Ʈ Ǯ �Ҵ�
    [SerializeField] Transform firePoint;        // �ѱ� ��ġ �� ������Ʈ
    public int bulletCount = 3;
    public float shootInterval = 3f;           // �Ѿ� ����
>>>>>>> Stashed changes


    [SerializeField] Transform player;

<<<<<<< Updated upstream

    void Update()
    {
        if (player != null)
        {
            Debug.Log(player.position);
            Vector2 dir = player.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }


=======
>>>>>>> Stashed changes
    public void FireBullets()
    {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            BossBullet2 bullet = objectPool.bossBullet2Pool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
<<<<<<< Updated upstream
            // �÷��̾� ���� ���
            Vector2 dir = (player.position - firePoint.position).normalized;
            bullet.Fire(dir);  // �� �÷��̾� ������ �߻�
=======
            // �÷��̾� ���� ���
            Vector2 dir = (player.position - firePoint.position).normalized;
            bullet.Fire(dir);  // �� �÷��̾� ������ �߻�
>>>>>>> Stashed changes
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
