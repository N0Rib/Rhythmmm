using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject resultScreen; // 결과 화면 UI
    public JudgeManager judgeManager;

    public void EndGame()
    {
        Debug.Log("EndGame called.");
        Time.timeScale = 0; // 게임 정지

        if (resultScreen != null)
        {
            resultScreen.SetActive(true); // 결과 화면 활성화
        }
        else
        {
            Debug.LogError("ResultScreen is not assigned!");
        }

        if (judgeManager != null)
        {
            judgeManager.DisplayFinalResults(); // 결과 데이터 표시
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // 게임 재개
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // 메인 메뉴로 이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
//github upload