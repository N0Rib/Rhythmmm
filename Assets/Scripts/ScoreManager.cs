using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int newScore)
    {
        currentScore = newScore;

        // 7자리 숫자로 포맷팅 (빈 자리는 0으로 채움)
        scoreText.text = $"{currentScore.ToString("D7")}";
    }
}
//github upload