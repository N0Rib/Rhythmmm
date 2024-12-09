using UnityEngine;
using System.Collections.Generic;

public class JudgeManager : MonoBehaviour
{
    public AudioSource audioSource;

    public float perfectWindow = 0.05f;
    public float greatWindow = 0.1f;
    public float goodWindow = 0.2f;

    private List<NoteController>[] laneNotes;
    private LongNoteController[] holdingLongNotes;

    public int totalNotes;
    public int score;
    private int perfectScore;
    private int greatScore;
    private int goodScore;

    public ResultManager resultManager; // Inspector에서 연결

    private int perfectCount = 0;
    private int greatCount = 0;
    private int goodCount = 0;
    private int missCount = 0;

    void Start()
    {
        int laneCount = 4;
        laneNotes = new List<NoteController>[laneCount];
        for (int i = 0; i < laneCount; i++)
        {
            laneNotes[i] = new List<NoteController>();
        }
        holdingLongNotes = new LongNoteController[laneCount];

        InputManager.OnLaneHit += OnLaneHitReceived;

        // 점수 초기화
        score = 0;
    }

    public void SetTotalNotes(int totalNotes)
    {
        this.totalNotes = Mathf.Max(1, totalNotes); // totalNotes가 0이 되지 않도록 보장

        // 판정별 점수 계산
        perfectScore = Mathf.FloorToInt(1000000f / totalNotes);
        greatScore = Mathf.FloorToInt(perfectScore * 0.7f);
        goodScore = Mathf.FloorToInt(perfectScore * 0.4f);

        Debug.Log($"SetTotalNotes: PerfectScore={perfectScore}, GreatScore={greatScore}, GoodScore={goodScore}");
    }

    void OnDestroy()
    {
        InputManager.OnLaneHit -= OnLaneHitReceived;
    }

    public void AddNoteToLane(int lane, NoteController note)
    {
        laneNotes[lane].Add(note);
    }

    void Update()
    {
        float currentTime = audioSource.time;

        for (int lane = 0; lane < holdingLongNotes.Length; lane++)
        {
            var ln = holdingLongNotes[lane];
            if (ln == null) continue;

            if (!InputManager.IsLaneKeyHolding(lane) && !ln.IsHoldFailed())
            {
                ln.SetHoldFailed();
                ShowJudgement("Miss", 0);
            }

            if (ln.currentState == LongNoteController.LongNoteState.Missed ||
                ln.currentState == LongNoteController.LongNoteState.Ended)
            {
                laneNotes[lane].Remove(ln);
                holdingLongNotes[lane] = null;
            }
        }

        CheckForMissedNotes(currentTime);
        CheckForEndGameCondition();
    }

    void OnLaneHitReceived(int lane)
    {
        float currentTime = audioSource.time;

        // 판정 가능한 노트를 찾음
        NoteController targetNote = FindJudgeableNote(lane, currentTime);

        if (targetNote == null)
        {
            Debug.LogWarning($"No judgeable note found in lane {lane} at time {currentTime}");
            return;
        }

        // 판정 계산
        float timeDiff = Mathf.Abs(targetNote.noteData.Time - currentTime);

        string result;
        int gainedScore;

        if (timeDiff <= perfectWindow)
        {
            result = "Perfect";
            gainedScore = perfectScore;
        }
        else if (timeDiff <= greatWindow)
        {
            result = "Great";
            gainedScore = greatScore;
        }
        else if (timeDiff <= goodWindow)
        {
            result = "Good";
            gainedScore = goodScore;
        }
        else
        {
            result = "Miss";
            gainedScore = 0;
        }

        // 판정 반영
        ShowJudgement(result, gainedScore);

        // 판정된 노트는 제거
        laneNotes[lane].Remove(targetNote);
        targetNote.ReturnToPool();
    }

    void ShowJudgement(string result, int gainedScore)
    {
        Debug.Log($"Result: {result}, GainedScore: {gainedScore}");

        // 점수 증가
        score += gainedScore;
        score = Mathf.Clamp(score, 0, 1000000); // 점수가 음수로 내려가거나 초과되지 않도록 제한

        Debug.Log($"Before Adding Score: {score}");

        // 판정 카운트 업데이트
        switch (result)
        {
            case "Perfect":
                perfectCount++;
                break;
            case "Great":
                greatCount++;
                break;
            case "Good":
                goodCount++;
                break;
            case "Miss":
                missCount++;
                break;
        }

        ScoreManager.Instance?.UpdateScore(score);
        ComboManager.Instance?.UpdateCombo(result);
        JudgementManager.Instance?.ShowJudgement(result);
    }

    void CheckForMissedNotes(float currentTime)
    {
        for (int lane = 0; lane < laneNotes.Length; lane++)
        {
            for (int i = laneNotes[lane].Count - 1; i >= 0; i--)
            {
                NoteController note = laneNotes[lane][i];

                // 판정선 타이밍을 지나친 노트는 Miss 처리
                if (currentTime > note.noteData.Time + goodWindow)
                {
                    ShowJudgement("Miss", 0);
                    laneNotes[lane].RemoveAt(i);
                    note.ReturnToPool();
                }
            }
        }
    }

    private NoteController FindJudgeableNote(int lane, float currentTime)
    {
        NoteController candidate = null;
        float minDiff = float.MaxValue;

        foreach (var note in laneNotes[lane])
        {
            float targetTime = note.noteData.Time;
            float diff = Mathf.Abs(targetTime - currentTime);
            if (diff < minDiff && diff <= goodWindow)
            {
                minDiff = diff;
                candidate = note;
            }
        }

        return candidate;
    }

    public void DisplayFinalResults()
    {
        if (resultManager != null)
        {
            resultManager.DisplayResults(perfectCount, greatCount, goodCount, missCount, score);
        }
        Debug.Log($"Final Score: {score} | Perfect: {perfectCount}, Great: {greatCount}, Good: {goodCount}, Miss: {missCount}");
    }

    void CheckForEndGameCondition()
    {
        if (IsSongFinished())
        {
            GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.EndGame(); // 결과 화면 출력
            }
        }
    }

    bool IsSongFinished()
    {
        if (audioSource == null || audioSource.clip == null)
            return false;

        return audioSource.time >= audioSource.clip.length;
    }
}
//github upload