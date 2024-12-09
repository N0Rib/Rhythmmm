// LongNoteController.cs
using UnityEngine;

public class LongNoteController : NoteController, IPooledObject
{
    public Transform head;
    public Transform body;
    public Transform tail;

    private float holdDuration;
    private bool startJudged = false;
    private bool holdFailed = false;
    private bool endJudged = false;

    private float originalBodyHeight;

    public enum LongNoteState { NotStarted, Started, Ended, Missed }
    public LongNoteState currentState = LongNoteState.NotStarted;

    private SpriteRenderer spriteRenderer;

    private float judgementInterval = 0.2f;
    private float nextJudgementTime = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
    }

    public override void Initialize(NoteData data, Transform start, Transform end, AudioSource audio, float aTime, float offset, float dist)
    {
        base.Initialize(data, start, end, audio, aTime, offset, dist);

        if (noteData == null || audioSource == null || body == null || tail == null || head == null)
            return;

        holdDuration = noteData.EndTime - noteData.Time;
        float speed = useFixedSpeed ? fixedSpeed : (distance / approachTime);
        float longNoteLength = holdDuration * speed;

        SpriteRenderer bodyRenderer = body.GetComponent<SpriteRenderer>();
        if (bodyRenderer == null)
            return;

        originalBodyHeight = bodyRenderer.sprite.bounds.size.y;
        float scaleY = longNoteLength / originalBodyHeight;
        body.localScale = new Vector3(1, scaleY, 1);
        head.localPosition = new Vector3(0, originalBodyHeight * scaleY, 0);

        currentState = LongNoteState.NotStarted;
    }

    void Update()
    {
        if (audioSource == null || noteData == null || endPos == null)
            return;

        float currentTime = audioSource.time;
        float speed = useFixedSpeed ? fixedSpeed : (distance / approachTime);
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (!reachedLine && transform.position.y <= endPos.position.y)
        {
            reachedLine = true;
            if (currentState == LongNoteState.NotStarted)
            {
                OnMissed();
                ReturnToPool();
            }
        }

        if (currentState == LongNoteState.Started && !holdFailed)
        {
            if (currentTime >= nextJudgementTime)
            {
                nextJudgementTime += judgementInterval;
                // Hold judgement handled by JudgeManager
            }
        }

        if (currentTime > noteData.EndTime + GetGlobalOffset() + 1f)
        {
            ReturnToPool();
        }
    }

    public void StartHit()
    {
        if (currentState == LongNoteState.NotStarted)
        {
            currentState = LongNoteState.Started;
            startJudged = true;
            nextJudgementTime = audioSource.time + judgementInterval;
            Debug.Log("LongNote Start Hit!");
        }
    }

    public void SetHoldFailed()
    {
        if (currentState == LongNoteState.Started)
        {
            holdFailed = true;
            currentState = LongNoteState.Missed;
            ReduceTransparency();
            Debug.Log("LongNote Hold Failed!");
        }
    }

    public bool IsHoldFailed()
    {
        return holdFailed;
    }

    public void EndHit()
    {
        if (currentState == LongNoteState.Started)
        {
            endJudged = true;
            currentState = LongNoteState.Ended;
            Debug.Log("LongNote End Hit!");
            ReturnToPool();
        }
    }

    public bool IsStartJudged() { return startJudged; }
    public bool IsEndJudged() { return endJudged; }

    public float GetStartTime() { return noteData.Time + GetGlobalOffset(); }
    public float GetEndTime() { return noteData.EndTime + GetGlobalOffset(); }

    void OnMissed()
    {
        Debug.Log("LongNote missed!");
    }

    public void ReduceTransparency()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0.8f;
            spriteRenderer.color = color;
        }
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        currentState = LongNoteState.NotStarted;
        holdFailed = false;
        startJudged = false;
        endJudged = false;
    }
}
//github upload