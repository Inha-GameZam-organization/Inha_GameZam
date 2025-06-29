using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAttack : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] ObjectPooling objectPool;   // ������Ʈ Ǯ �Ҵ�
    [SerializeField] Transform firePoint;        // �ѱ� ��ġ �� ������Ʈ
    [SerializeField] Transform player;        // �ѱ� ��ġ �� ������Ʈ
    public int bulletCount = 20;
    public float shootInterval = 1f;           // �Ѿ� ����



    void Update()
    {
        if (player != null)
        {
            // 1. ��(LeftArm)�� �÷��̾ ���ϰ� ȸ��
            Debug.Log(player.position);
            Vector2 dir = player.position - transform.position;
            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    
    }
=======
    [SerializeField] ObjectPooling objectPool;   // ������Ʈ Ǯ �Ҵ�
    [SerializeField] Transform firePoint;        // �ѱ� ��ġ �� ������Ʈ
    public int bulletCount = 20;
    public float shootInterval = 1f;           // �Ѿ� ����

    [SerializeField] Transform player; 
>>>>>>> Stashed changes

    public void FireBullets()
    {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            BossBullet bullet = objectPool.bossBulletPool.Get();
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
