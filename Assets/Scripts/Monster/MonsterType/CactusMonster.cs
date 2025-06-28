using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMonster : MonoBehaviour
{
    [Header("��ġ ����")]
    public float detectRange = 5f;

    private Rigidbody2D rb;
    private Transform player;

    private IPlayerDetector detector;
    private IWatchBehavior watcher;

    private enum MonsterState { Patrol, Watch }
    private MonsterState state = MonsterState.Patrol;

    private bool facingRight = true; // ���� �ٶ󺸴� ���� ������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        detector = new UpperHalfCircleDetector(detectRange, transform);
        //watcher = new PassiveWatch();  // �� �ʰ� ������ Watcher Ŭ���� �־���
    }

    void Update()
    {
        if (player == null) return;

        bool detected = detector.IsPlayerDetected(transform, player);

        switch (state)
        {
            case MonsterState.Patrol:
                FaceOriginalDirection();
                if (detected)
                {
                    state = MonsterState.Watch;
                }
                break;

            case MonsterState.Watch:
                watcher.Watch(transform, player, rb);
                if (!detected)
                {
                    watcher.ResetWatch();
                    state = MonsterState.Patrol;
                }
                break;
        }
    }

    void FaceOriginalDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}