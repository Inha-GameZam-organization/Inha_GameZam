using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� Ÿ�� ����
public enum SoundType
{
    //main ȭ�� ����





    //stage ����
    Shoot,
    Missile,
    Dash,
    Jump,
    GetHit,
    BackgroundMusic,

    // ���߰� ����
}

//�ν����Ϳ��� ���� Ÿ�� �ֱ� ���� ����ü
[System.Serializable]
public struct SoundEntry
{
    public SoundType type;
    public AudioClip clip;
}

//SoundManager �̱���
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    //���� ����
    [SerializeField] private List<SoundEntry> soundTable;

    private Dictionary<SoundType, AudioClip> clipMap;
    private Queue<AudioClip> playQueue = new Queue<AudioClip>();

    private AudioSource audioSource;
    private AudioSource bgmSource;


    void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource ������Ʈ ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;

        // ������ǿ� AudioSource ����
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false; 
        bgmSource.loop = true;
        bgmSource.spatialBlend = 0;
        bgmSource.volume = 0.1f; // ������� ���� ���� (�ʿ�� ����)

        // Dictionary�� ���� �ʱ�ȭ
        clipMap = new Dictionary<SoundType, AudioClip>();
        foreach (var entry in soundTable)
            clipMap[entry.type] = entry.clip;

        if (clipMap.TryGetValue(SoundType.BackgroundMusic, out var bgmClip) && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }

    }
    public void PlaySound(SoundType type, float volume = 0.3f)
    {
        if (type == SoundType.BackgroundMusic)
            return; // ��������� ������ ó��

        if (clipMap.TryGetValue(type, out var clip) && clip != null)
        {
            playQueue.Enqueue(clip);

            var next = playQueue.Dequeue();
            audioSource.PlayOneShot(next, volume);
        }
        else
        {
            Debug.LogWarning($"SoundManager: '{type}' Ŭ���� �����ϴ�.");
        }
    }
}