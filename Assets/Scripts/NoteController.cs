using UnityEngine;

public class NoteController : MonoBehaviour, IPooledObject
{
    public NoteData noteData;
    protected Transform startPos;
    protected Transform endPos;
    protected AudioSource audioSource;

    protected float approachTime;
    protected float globalOffset;
    protected float distance;

    public bool useFixedSpeed = false;
    public float fixedSpeed = 5f;

    protected bool reachedLine = false;

    public virtual void Initialize(NoteData data, Transform start, Transform end, AudioSource audio, float aTime, float offset, float dist)
    {
        noteData = data;
        startPos = start;
        endPos = end;
        audioSource = audio;
        approachTime = aTime;
        globalOffset = offset;
        distance = dist;
        reachedLine = false;
    }

    void Update()
    {
        if (audioSource == null || noteData == null)
            return;

        float speed = useFixedSpeed ? fixedSpeed : (distance / approachTime);
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (!reachedLine && transform.position.y <= endPos.position.y)
        {
            reachedLine = true;
        }

        if (transform.position.y < -5f)
        {
            ReturnToPool();
        }
    }

    public virtual void OnObjectSpawn()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public float GetGlobalOffset()
    {
        return globalOffset;
    }
}
//github upload