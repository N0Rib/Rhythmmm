// Metronome.cs
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public float bpm = 120f;
    public bool isPlaying = false;

    private float interval;
    private float nextBeatTime;

    void Start()
    {
        CalculateInterval();
    }

    void Update()
    {
        if (isPlaying && Time.time >= nextBeatTime)
        {
            TriggerBeat();
            nextBeatTime += interval;
        }
    }

    public void StartMetronome()
    {
        isPlaying = true;
        nextBeatTime = Time.time + interval;
    }

    public void StopMetronome()
    {
        isPlaying = false;
    }

    public void SetBPM(float newBpm)
    {
        bpm = newBpm;
        CalculateInterval();
    }

    private void CalculateInterval()
    {
        interval = 60f / bpm;
    }

    private void TriggerBeat()
    {
        Debug.Log($"Beat triggered at: {Time.time}");
    }
}
//github upload