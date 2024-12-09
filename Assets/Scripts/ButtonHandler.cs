using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static string selectedMapPath; // 선택된 맵 경로
    public static string selectedAudioPath; // 선택된 음악 경로

    public void OnButtonClick()
    {
        Debug.Log($"Selected Map: {StageSelectManager.selectedMapPath}");
        Debug.Log($"Selected Audio: {StageSelectManager.selectedAudioPath}");

        // 씬 로드
        SceneManager.LoadScene("Gameplay");
    }
}
//github upload