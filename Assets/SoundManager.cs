using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//���� Ÿ�� ����
public enum SoundType
{
    //main ȭ�� ����
    MainBackgroundMusic,
    MainButtonClick,
    MainGameStart,

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
public class SoundManager : Singleton<SoundManager>
{
    //public static SoundManager Instance { get; private set; }

    //���� ����
    [SerializeField] private List<SoundEntry> soundTable;

    private Dictionary<SoundType, AudioClip> clipMap;
    private Queue<AudioClip> playQueue = new Queue<AudioClip>();

    private AudioSource audioSource;
    private AudioSource bgmSource;


    protected override void OnAwakeWork()
    {
        DontDestroyOnLoad(gameObject);

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

        // �� ��ȯ �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
        // ù ��(Startup)�� �ٷ� ���
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

    }

    protected override void OnDestroyedWork()
    {
        // �ݵ�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ٲ� ������ ȣ��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� BGM ����
        if (bgmSource.isPlaying) bgmSource.Stop();

        // ����� Ÿ�� ����
        SoundType typeToPlay;
        if (scene.name == "Main") typeToPlay = SoundType.MainBackgroundMusic;
        else if (scene.name == "Stage") typeToPlay = SoundType.BackgroundMusic;
        else return; // �� �� ���� BGM ����

        // Ŭ���� ������ ���
        if (clipMap.TryGetValue(typeToPlay, out var clip) && clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySound(SoundType type, float volume = 0.3f)
    {
        if (type == SoundType.BackgroundMusic || type == SoundType.MainBackgroundMusic)
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