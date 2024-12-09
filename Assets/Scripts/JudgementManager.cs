// JudgementManager.cs
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public static JudgementManager Instance { get; private set; }

    public GameObject judgementDisplayPrefab; // JudgementDisplay 프리팹
    public Transform canvasTransform; // Canvas의 Transform
    public Vector2 fixedDisplayPosition = new Vector2(0, 200); // 고정된 화면 위치 (예: 상단 중앙)

    private JudgementDisplay currentDisplay;

    void Awake()
    {
        // 싱글턴 패턴 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 판정 결과를 화면에 표시하는 메서드
    /// </summary>
    /// <param name="result">판정 결과 ("Perfect", "Great", "Good", "Miss")</param>
    public void ShowJudgement(string result)
    {
        if (judgementDisplayPrefab == null || canvasTransform == null)
        {
            Debug.LogError("JudgementDisplayPrefab 또는 CanvasTransform이 설정되지 않았습니다.");
            return;
        }

        // 기존 인스턴스가 있다면 제거
        if (currentDisplay != null)
        {
            Destroy(currentDisplay.gameObject);
        }

        // JudgementDisplay 프리팹 인스턴스화
        GameObject judgementObj = Instantiate(judgementDisplayPrefab, canvasTransform);
        currentDisplay = judgementObj.GetComponent<JudgementDisplay>();
        if (currentDisplay != null)
        {
            // 판정 결과 설정
            currentDisplay.SetJudgement(result);

            // 고정된 화면 위치로 설정
            RectTransform rectTransform = judgementObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = fixedDisplayPosition;
            }
            else
            {
                Debug.LogError("JudgementDisplay의 RectTransform을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("JudgementDisplay 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
//github upload