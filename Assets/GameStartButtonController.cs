using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameStartButtonController : MonoBehaviour
{
    [Header("���� ����")]
    public AudioSource bgmSource;       // BGM_Player�� AudioSource
    public AudioSource sfxSource;       // ȿ������ AudioSource (Play On Awake ��)
    public AudioClip clickClip;         // �˵��η� Ŭ��

    [Header("�� ����")]
    public string nextSceneName;        // ���� ���ÿ� ��ϵ� ���� �� �̸�

    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnStartClicked);
    }

    void OnStartClicked()
    {
        // Ŭ�����ڸ��� ��ư ��Ȱ��ȭ�ؼ� �ߺ� Ŭ�� ����
        btn.interactable = false;
        StartCoroutine(PlaySfxAndLoad());
    }

    IEnumerator PlaySfxAndLoad()
    {
        // 1) BGM ����
        if (bgmSource.isPlaying)
            bgmSource.Stop();

        // 2) SFX ���
        sfxSource.PlayOneShot(clickClip);

        // 3) SFX ���̸�ŭ ���
        yield return new WaitForSeconds(clickClip.length);

        // 4) �� ��ȯ
        SceneManager.LoadScene("Stage");
    }
}