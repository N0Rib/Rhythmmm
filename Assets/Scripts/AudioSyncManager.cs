using UnityEngine;

public class AudioSyncManager : MonoBehaviour
{
    public AudioSource audioSource; // ���� ��� AudioSource
    public float audioOffset = 0f; // ��ũ ���� ��

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned. Creating one dynamically.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public float GetSyncedTime()
    {
        return audioSource != null ? audioSource.time + audioOffset : 0f;
    }
}
//github upload
