// JudgementDisplay.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class JudgementDisplay : MonoBehaviour
{
    public TextMeshProUGUI judgementText; // ���� �ؽ�Ʈ UI
    public float displayDuration = 1f; // �ؽ�Ʈ�� ǥ�õǴ� �ð�
    public float fadeDuration = 0.5f; // �ؽ�Ʈ�� ������ ������� �ð�
    public float scaleUpDuration = 0.2f; // �ؽ�Ʈ�� Ŀ���� �ð�
    public Vector3 initialScale = new Vector3(0.8f, 0.8f, 1f); // �ʱ� ������
    public Vector3 finalScale = new Vector3(1f, 1f, 1f); // ���� ������

    void Awake()
    {
        // �ʱ� ���� (�ʿ��)
    }

    /// <summary>
    /// ���� ����� �����ϰ�, �ִϸ��̼��� �����ϴ� �޼���
    /// </summary>
    /// <param name="result">���� ��� ("Perfect", "Great", "Good", "Miss")</param>
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

            // �ִϸ��̼� ����
            StartCoroutine(DisplayAndFade());
        }
    }

    /// <summary>
    /// �ؽ�Ʈ�� ǥ���ϰ� �ִϸ��̼��� ������ �� ������� �ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayAndFade()
    {
        // �ʱ� ������ ����
        transform.localScale = initialScale;

        // ������ �� �ִϸ��̼�
        float elapsed = 0f;
        while (elapsed < scaleUpDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / scaleUpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;

        // �ؽ�Ʈ ǥ�� �ð� ���
        yield return new WaitForSeconds(displayDuration);

        // ���̵� �ƿ� �ִϸ��̼�
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

        // �ؽ�Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
//github upload