// InputManager.cs
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    private string[] laneButtonNames = { "LaneButton01", "LaneButton02", "LaneButton03", "LaneButton04" };
    private GameObject[] laneButtons;

    public static event Action<int> OnLaneHit;

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

    void Start()
    {
        laneButtons = new GameObject[laneButtonNames.Length];
        for (int i = 0; i < laneButtonNames.Length; i++)
        {
            laneButtons[i] = GameObject.Find(laneButtonNames[i]);
            if (laneButtons[i] == null)
            {
                Debug.LogError($"GameObject '{laneButtonNames[i]}'를 찾을 수 없습니다. 씬에 해당 오브젝트가 있는지 확인하세요.");
            }
            else
            {
                laneButtons[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < laneKeys.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                OnLaneHit?.Invoke(i);
                if (laneButtons[i] != null)
                    laneButtons[i].SetActive(true);
            }

            if (Input.GetKeyUp(laneKeys[i]))
            {
                if (laneButtons[i] != null)
                    laneButtons[i].SetActive(false);
            }
        }
    }

    public static bool IsLaneKeyHolding(int lane)
    {
        if (Instance == null) return false;
        if (lane < 0 || lane >= Instance.laneKeys.Length) return false;
        return Input.GetKey(Instance.laneKeys[lane]);
    }
}
//github upload