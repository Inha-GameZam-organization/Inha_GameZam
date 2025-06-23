using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    Debug.LogError(typeof(T) + " �̱����� ���� �����ϴ�! ���� �̱����� ��ġ�ϼ���.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance.gameObject != this.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        OnAwakeWork();
    }

    private void OnDestroy()
    {
        OnDestroyedWork();
    }

    protected virtual void OnAwakeWork() { }

    protected virtual void OnDestroyedWork() { }
}
