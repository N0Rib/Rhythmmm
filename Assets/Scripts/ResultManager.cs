using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI additionalRankText;

    public void DisplayResults(int perfect, int great, int good, int miss, int score)
    {
        perfectText.text = $"Perfect: {perfect}";
        greatText.text = $"Great: {great}";
        goodText.text = $"Good: {good}";
        missText.text = $"Miss: {miss}";
        scoreText.text = $"Score: {score:D7}";

        rankText.text = CalculateRank(score);
        additionalRankText.text = CalculateAdditionalRank(perfect, great, good, miss);
    }

    private string CalculateRank(int score)
    {
        if (score >= 900000) return "S";
        if (score >= 800000) return "A";
        if (score >= 700000) return "B";
        return "C";
    }

    private string CalculateAdditionalRank(int perfect, int great, int good, int miss)
    {
        if (good == 0 && miss == 0)
        {
            if (great == 0) return "AP"; // All Perfect
            return "FC"; // Full Combo
        }
        return "";
    }
}
//github upload