using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject resultScreen; // ��� ȭ�� UI
    public JudgeManager judgeManager;

    public void EndGame()
    {
        Debug.Log("EndGame called.");
        Time.timeScale = 0; // ���� ����

        if (resultScreen != null)
        {
            resultScreen.SetActive(true); // ��� ȭ�� Ȱ��ȭ
        }
        else
        {
            Debug.LogError("ResultScreen is not assigned!");
        }

        if (judgeManager != null)
        {
            judgeManager.DisplayFinalResults(); // ��� ������ ǥ��
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // ���� �簳
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // ���� �޴��� �̵�
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
//github upload