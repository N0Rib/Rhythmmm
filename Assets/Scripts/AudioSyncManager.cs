using UnityEngine;

public class AudioSyncManager : MonoBehaviour
{
    public AudioSource audioSource; // À½¾Ç Àç»ý AudioSource
    public float audioOffset = 0f; // ½ÌÅ© Á¶Àý °ª

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
