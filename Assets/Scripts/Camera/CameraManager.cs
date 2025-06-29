using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private Coroutine currentShakeCoroutine;

    protected override void OnAwakeWork()
    {
        DontDestroyOnLoad(this.gameObject);
        noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    protected override void OnDestroyedWork()
    {
        base.OnDestroyedWork();
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        if (currentShakeCoroutine != null)
        {
            StopCoroutine(currentShakeCoroutine);
        }

        currentShakeCoroutine = StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
    {
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Lerp ����: 1���� 0���� ���� �پ���
            noise.AmplitudeGain = Mathf.Lerp(amplitude, 0f, t);
            noise.FrequencyGain = Mathf.Lerp(frequency, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ȯ���� 0���� ����
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
        currentShakeCoroutine = null;
    }
}
