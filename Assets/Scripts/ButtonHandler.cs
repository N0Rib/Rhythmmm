using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static string selectedMapPath; // ���õ� �� ���
    public static string selectedAudioPath; // ���õ� ���� ���

    public void OnButtonClick()
    {
        Debug.Log($"Selected Map: {StageSelectManager.selectedMapPath}");
        Debug.Log($"Selected Audio: {StageSelectManager.selectedAudioPath}");

        // �� �ε�
        SceneManager.LoadScene("Gameplay");
    }
}
//github upload