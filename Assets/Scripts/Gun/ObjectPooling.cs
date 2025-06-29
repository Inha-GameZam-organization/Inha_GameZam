using UnityEngine;
using UnityEngine.Pool;


// ������Ʈ Ǯ���� ���� ��ũ��Ʈ
public class ObjectPooling : MonoBehaviour
{

    //�⺻ ���� 
    int defaultCapacity = 40;   //�⺻ ť ������
    int maxSize = 100;          //�ִ� ť ������
    
    //�÷��̾�� 
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Missile missilePrefab;
    public ObjectPool<Bullet> bulletPool;
    public ObjectPool<Missile> missilePool;
    

    //���Ϳ�
    [SerializeField] MonsterBullet monsterBulletPrefab;
    public ObjectPool<MonsterBullet> monsterBulletPool;

    //������
    [SerializeField] BossBullet bossBulletPrefab;
    public ObjectPool<BossBullet> bossBulletPool;
    [SerializeField] BossBullet2 bossBullet2Prefab;
    public ObjectPool<BossBullet2> bossBullet2Pool;

    void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            OnGetBullet,
            OnReleaseBullet,
            OnDestroyBullet,
            true, // Make sure to set this to false if you want to control the pool size manually
            defaultCapacity, // Initial size of the pool
            maxSize // Maximum size of the pool
        );
        missilePool = new ObjectPool<Missile>(
            CreateMissile,
            OnGetMissile,
            OnReleaseMissile,
            OnDestroyMissile,
            true,
            defaultCapacity,
            maxSize
            );
        monsterBulletPool = new ObjectPool<MonsterBullet>(
            CreateMonsterBullet,
            OnGetMonsterBullet,
            OnReleaseMonsterBullet,
            OnDestroyMonsterBullet,
            true, 
            defaultCapacity, 
            maxSize
            );
        bossBulletPool = new ObjectPool<BossBullet>(
            CreateBossBullet,
            OnGetBossBullet,
            OnReleaseBossBullet,
            OnDestroyBossBullet,
            true,
            defaultCapacity,
            maxSize
        );
        bossBullet2Pool = new ObjectPool<BossBullet2>(
            CreateBossBullet2,
            OnGetBossBullet2,
            OnReleaseBossBullet2,
            OnDestroyBossBullet2,
            true,
            defaultCapacity,
            maxSize
        );
    }


    //�÷��̾� �Ѿ�(��Ŭ��)
    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.Init(bulletPool);
        bullet.transform.SetParent(transform);
        return bullet;
    }
    void OnGetBullet(Bullet bullet)
    {
        if (bullet != null)
        {
            bullet.gameObject.SetActive(true);
        }
    }
    void OnReleaseBullet(Bullet bullet)
    {
        if (bullet!=null)
        {
            bullet.gameObject.SetActive(false);
        }
    }
    void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }


    //�÷��̾� �̻���(��Ŭ��)
    Missile CreateMissile()
    {
        Missile missile = Instantiate(missilePrefab);
        missile.Init(missilePool);
        missile.transform.SetParent(transform);
        return missile;
    }
    void OnGetMissile(Missile missile)
    {
        if (missile != null)
        {
            missile.gameObject.SetActive(true);
        }
    }
    void OnReleaseMissile(Missile missile)
    {
        if (missile != null)
        {
            missile.gameObject.SetActive(false);
        }
    }
    void OnDestroyMissile(Missile missile)
    {
        Destroy(missile.gameObject);
    }


    //���� �Ѿ�
    MonsterBullet CreateMonsterBullet()
    {
        MonsterBullet mbullet = Instantiate(monsterBulletPrefab);
        mbullet.Init(monsterBulletPool);
        mbullet.transform.SetParent(transform);
        return mbullet;
    }
    void OnGetMonsterBullet(MonsterBullet mbullet)
    {
        if (mbullet != null)
        {
            mbullet.gameObject.SetActive(true);
        }
    }
    void OnReleaseMonsterBullet(MonsterBullet mbullet)
    {
        if (mbullet != null)
        {
            mbullet.gameObject.SetActive(false);
        }
    }
    void OnDestroyMonsterBullet(MonsterBullet mbullet)
    {
        Destroy(mbullet.gameObject);
    }

    //���� �Ѿ�
    BossBullet CreateBossBullet()
    {
        BossBullet bossbullet = Instantiate(bossBulletPrefab);
        bossbullet.Init(bossBulletPool);
        bossbullet.transform.SetParent(transform);
        return bossbullet;
    }
    void OnGetBossBullet(BossBullet bossbullet)
    {
        if (bossbullet != null)
        {
            bossbullet.gameObject.SetActive(true);
        }
    }
    void OnReleaseBossBullet(BossBullet bossbullet)
    {
        if (bossbullet != null)
        {
            bossbullet.gameObject.SetActive(false);
        }
    }
    void OnDestroyBossBullet(BossBullet bossbullet)
    {
        Destroy(bossbullet.gameObject);
    }

    //���� �Ѿ�2
    BossBullet2 CreateBossBullet2()
    {
        BossBullet2 bossbullet2 = Instantiate(bossBullet2Prefab);
        bossbullet2.Init(bossBullet2Pool);
        bossbullet2.transform.SetParent(transform);
        return bossbullet2;
    }
    void OnGetBossBullet2(BossBullet2 bossbullet)
    {
        if (bossbullet != null)
        {
            bossbullet.gameObject.SetActive(true);
        }
    }
    void OnReleaseBossBullet2(BossBullet2 bossbullet2)
    {
        if (bossbullet2 != null)
        {
            bossbullet2.gameObject.SetActive(false);
        }
    }
    void OnDestroyBossBullet2(BossBullet2 bossbullet2)
    {
        Destroy(bossbullet2.gameObject);
    }
}
