// JudgementDisplay.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class JudgementDisplay : MonoBehaviour
{
    public TextMeshProUGUI judgementText; // 판정 텍스트 UI
    public float displayDuration = 1f; // 텍스트가 표시되는 시간
    public float fadeDuration = 0.5f; // 텍스트가 서서히 사라지는 시간
    public float scaleUpDuration = 0.2f; // 텍스트가 커지는 시간
    public Vector3 initialScale = new Vector3(0.8f, 0.8f, 1f); // 초기 스케일
    public Vector3 finalScale = new Vector3(1f, 1f, 1f); // 최종 스케일

    void Awake()
    {
        // 초기 설정 (필요시)
    }

    /// <summary>
    /// 판정 결과를 설정하고, 애니메이션을 시작하는 메서드
    /// </summary>
    /// <param name="result">판정 결과 ("Perfect", "Great", "Good", "Miss")</param>
    public void SetJudgement(string result)
    {
        if (judgementText != null)
        {
            judgementText.text = result;
            switch (result)
            {
                case "Perfect":
                    judgementText.color = Color.yellow;
                    break;
                case "Great":
                    judgementText.color = Color.green;
                    break;
                case "Good":
                    judgementText.color = Color.cyan;
                    break;
                case "Miss":
                    judgementText.color = Color.white;
                    break;
                default:
                    judgementText.color = Color.white;
                    break;
            }

            // 애니메이션 시작
            StartCoroutine(DisplayAndFade());
        }
    }

    /// <summary>
    /// 텍스트를 표시하고 애니메이션을 적용한 후 사라지게 하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayAndFade()
    {
        // 초기 스케일 설정
        transform.localScale = initialScale;

        // 스케일 업 애니메이션
        float elapsed = 0f;
        while (elapsed < scaleUpDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / scaleUpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;

        // 텍스트 표시 시간 대기
        yield return new WaitForSeconds(displayDuration);

        // 페이드 아웃 애니메이션
        if (judgementText != null)
        {
            Color originalColor = judgementText.color;
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                judgementText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }
            judgementText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

        // 텍스트 비활성화
        gameObject.SetActive(false);
    }
}
//github upload