using UnityEngine;
using TMPro;
using System.IO;

public class StageSelectManager : MonoBehaviour
{
    public Transform stageListParent;
    public GameObject stageButtonPrefab;

    public static string selectedMapPath;
    public static string selectedAudioPath;

    private string mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Maps");

    void Start()
    {
        LoadStages();
    }

    private string ExtractOverallDifficulty(string txtFilePath)
    {
        try
        {
            // 텍스트 파일 읽기
            string[] lines = File.ReadAllLines(txtFilePath);

            // OverallDifficulty 찾기
            foreach (string line in lines)
            {
                if (line.StartsWith("OverallDifficulty"))
                {
                    string[] splitLine = line.Split(':');
                    if (splitLine.Length > 1)
                    {
                        return splitLine[1].Trim(); // 값 반환
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error reading OverallDifficulty from {txtFilePath}: {ex.Message}");
        }

        return "Unknown"; // 실패 시 기본값 반환
    }

    private void LoadStages()
    {
        if (!Directory.Exists(mapsFolderPath))
        {
            Debug.LogError($"Maps folder not found at: {mapsFolderPath}");
            return;
        }

        foreach (string folderPath in Directory.GetDirectories(mapsFolderPath))
        {
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
            string[] audioFiles = Directory.GetFiles(folderPath, "*.mp3");

            if (txtFiles.Length == 0)
            {
                Debug.LogWarning($"No .txt files found in folder: {folderPath}");
                continue;
            }

            foreach (string txtFilePath in txtFiles)
            {
                GameObject button = Instantiate(stageButtonPrefab, stageListParent);
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

                if (buttonText != null)
                {
                    string mapName = Path.GetFileName(folderPath);
                    string overallDifficulty = ExtractOverallDifficulty(txtFilePath);

                    // 버튼 텍스트 설정 (폴더 이름과 OverallDifficulty 표시)
                    buttonText.text = $"{mapName}_OverallDifficulty:{overallDifficulty}";
                }

                string audioPath = audioFiles.Length > 0 ? audioFiles[0] : null;

                button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    selectedMapPath = txtFilePath;
                    selectedAudioPath = audioPath;

                    Debug.Log($"Selected Map: {selectedMapPath}");
                    Debug.Log($"Selected Audio: {selectedAudioPath}");
                });
            }
        }
    }
}
//github upload
