using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TestParser : MonoBehaviour
{
    public NoteManager noteManager;

    void Start()
    {
        if (string.IsNullOrEmpty(StageSelectManager.selectedMapPath) || string.IsNullOrEmpty(StageSelectManager.selectedAudioPath))
        {
            Debug.LogError("No stage selected. Returning to Stage Select.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
            return;
        }

        StartCoroutine(LoadStage());
    }

    private IEnumerator LoadStage()
    {
        string noteFilePath = "file://" + StageSelectManager.selectedMapPath.Replace("\\", "/");
        string audioFilePath = "file://" + StageSelectManager.selectedAudioPath.Replace("\\", "/");

        // 노트 파일 로드
        UnityWebRequest noteRequest = UnityWebRequest.Get(noteFilePath);
        yield return noteRequest.SendWebRequest();

        if (noteRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Successfully loaded note file: {noteFilePath}");
            OsuParser parser = new OsuParser();
            var notes = parser.ParseFromText(noteRequest.downloadHandler.text);
            noteManager.LoadNotes(notes, parser.BPM, parser.Offset);
        }
        else
        {
            Debug.LogError($"Failed to load note file from: {noteFilePath}. Error: {noteRequest.error}");
        }

        // 오디오 파일 로드
        UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, AudioType.MPEG);
        yield return audioRequest.SendWebRequest();

        if (audioRequest.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(audioRequest);
            if (noteManager.audioSyncManager.audioSource != null)
            {
                noteManager.audioSyncManager.audioSource.clip = audioClip;
                noteManager.audioSyncManager.audioSource.Play();
                Debug.Log($"Successfully loaded and playing audio: {audioFilePath}");
            }
        }
        else
        {
            Debug.LogError($"Failed to load audio file from: {audioFilePath}. Error: {audioRequest.error}");
        }
    }
}
//github upload