using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    public TextMeshProUGUI comboText;
    public float bounceDuration = 0.2f; // Ƣ�� ȿ�� ���� �ð�
    public Vector3 initialScale = Vector3.one; // �⺻ ũ��
    public Vector3 bounceScale = new Vector3(1.1f, 1.1f, 1f); // Ƣ�� ȿ�� ũ��

    private int currentCombo = 0;

    private int comboCount = 0;

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
    public void ResetCombo()
    {
        currentCombo = 0;
        UpdateComboUI();
    }

    public void UpdateCombo(string judgement)
    {
        if (judgement == "Perfect" || judgement == "Great")
        {
            comboCount++;
        }
        else if (judgement == "Good")
        {
            comboCount = 1; // Good ���� �޺��� 1���� ����
        }
        else if (judgement == "Miss")
        {
            comboCount = 0; // Miss ���� �޺��� 0���� ����
        }

        UpdateComboDisplay();
        if (comboCount > 0) StartCoroutine(BounceEffect());
    }

    private void UpdateComboDisplay()
    {
        if (comboCount > 0)
        {
            comboText.text = $"Combo\n {comboCount}";
            comboText.gameObject.SetActive(true);
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }

    private System.Collections.IEnumerator BounceEffect()
    {
        float elapsed = 0f;

        while (elapsed < bounceDuration)
        {
            float t = elapsed / bounceDuration;
            comboText.rectTransform.localScale = Vector3.Lerp(initialScale, bounceScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        comboText.rectTransform.localScale = initialScale;
    }

    private void UpdateComboUI()
    {
        // UI ������Ʈ ����
        Debug.Log($"Current Combo: {currentCombo}");
        // ���� UI �ؽ�Ʈ ������Ʈ �ڵ� �߰�
    }
}
//github upload