using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class NoteManager : MonoBehaviour
{
    public ObjectPool objectPool;
    public Transform[] lanes;
    public Transform judgementLine;
    public AudioSyncManager audioSyncManager;
    public JudgeManager judgeManager;

    public TMP_Text countdownText;
    public float BPM = 120f;
    public bool useFixedSpeed = false;

    [Range(1f, 25f)]
    public float fixedSpeed = 5f;
    public float approachTime = 2f;

    private Queue<NoteData> noteQueue = new Queue<NoteData>();
    private bool isMusicStarted = false;
    private bool isCountdownFinished = false;
    private float countdownDuration = 3f;

    void Start()
    {
        if (objectPool == null)
        {
            Debug.LogError("ObjectPool is not assigned in the NoteManager.");
        }

        if (audioSyncManager == null || audioSyncManager.audioSource == null)
        {
            Debug.LogError("AudioSyncManager or its AudioSource is not assigned!");
        }

        // StageSelectManager에서 선택된 파일 경로 가져오기
        if (string.IsNullOrEmpty(StageSelectManager.selectedMapPath) || string.IsNullOrEmpty(StageSelectManager.selectedAudioPath))
        {
            Debug.LogError("No stage selected. Returning to Stage Select.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
            return;
        }

        StartCoroutine(LoadStage(StageSelectManager.selectedMapPath, StageSelectManager.selectedAudioPath));
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator LoadStage(string notePath, string audioPath)
    {
        // 노트 파일 로드
        UnityWebRequest noteRequest = UnityWebRequest.Get(notePath);
        yield return noteRequest.SendWebRequest();

        if (noteRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Successfully loaded note file: {notePath}");
            OsuParser parser = new OsuParser();
            List<NoteData> notes = parser.ParseFromText(noteRequest.downloadHandler.text);

            if (notes != null && notes.Count > 0)
            {
                LoadNotes(notes, parser.BPM, parser.Offset);
            }
            else
            {
                Debug.LogError($"No notes found in note file: {notePath}");
            }
        }
        else
        {
            Debug.LogError($"Failed to load note file from: {notePath}. Error: {noteRequest.error}");
        }

        // 오디오 파일 로드
        UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG);
        yield return audioRequest.SendWebRequest();

        if (audioRequest.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(audioRequest);
            audioSyncManager.audioSource.clip = audioClip;
            Debug.Log($"Audio loaded successfully: {audioPath}");
        }
        else
        {
            Debug.LogError($"Failed to load audio file from: {audioPath}. Error: {audioRequest.error}");
        }
    }

    public void LoadNotes(List<NoteData> notes, float parsedBPM, float parsedOffset)
    {
        BPM = parsedBPM;

        foreach (var note in notes)
        {
            noteQueue.Enqueue(note);
        }

        Debug.Log($"Loaded {notes.Count} notes.");
    }

    private IEnumerator CountdownCoroutine()
    {
        float remainingTime = countdownDuration;

        while (remainingTime > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            }
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        if (countdownText != null)
        {
            countdownText.text = "Start!";
        }

        yield return new WaitForSeconds(1f);

        if (countdownText != null)
        {
            countdownText.text = "";
        }

        isCountdownFinished = true; // 카운트다운 종료 플래그 설정
        Debug.Log("Countdown finished. Game starts now!");

        // 카운트다운 종료 후 음악 재생
        if (audioSyncManager.audioSource != null && audioSyncManager.audioSource.clip != null)
        {
            audioSyncManager.audioSource.Play(); // 음악 시작
            isMusicStarted = true;
        }
    }

    void Update()
    {
        if (!isCountdownFinished) return; // 카운트다운이 끝나지 않으면 실행되지 않음

        if (noteQueue.Count > 0 && objectPool != null && audioSyncManager != null && isMusicStarted)
        {
            float currentTime = audioSyncManager.GetSyncedTime();
            NoteData nextNote = noteQueue.Peek();
            float targetTime = nextNote.Time;

            float distance = Mathf.Abs(lanes[nextNote.Lane].position.y - judgementLine.position.y);
            float actualApproachTime = useFixedSpeed ? (distance / fixedSpeed) : approachTime;
            float spawnTime = targetTime - actualApproachTime;

            if (currentTime >= spawnTime)
            {
                noteQueue.Dequeue();
                CreateNote(nextNote, distance, actualApproachTime);
            }
        }
    }

    void CreateNote(NoteData noteData, float distance, float actualApproachTime)
    {
        Transform spawnPoint = lanes[noteData.Lane];
        Vector3 spawnPos = spawnPoint.position;

        GameObject noteObj = noteData.IsLongNote
            ? objectPool.SpawnLongNote(spawnPos, Quaternion.identity)
            : objectPool.SpawnShortNote(spawnPos, Quaternion.identity);

        if (noteObj == null)
        {
            Debug.LogError($"Failed to spawn note.");
            return;
        }

        NoteController controller = noteObj.GetComponent<NoteController>();
        if (controller == null)
        {
            Debug.LogError("NoteController is missing on the prefab!");
            noteObj.SetActive(false);
            return;
        }

        controller.useFixedSpeed = useFixedSpeed;
        controller.fixedSpeed = fixedSpeed;
        controller.Initialize(noteData, spawnPoint, judgementLine, audioSyncManager.audioSource, actualApproachTime, 0, distance);

        judgeManager.AddNoteToLane(noteData.Lane, controller);
    }
}
//github upload