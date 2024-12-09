using UnityEngine;
using System.Collections.Generic;

public class OsuParser
{
    public float BPM = 120f;
    public float Offset = 0f;
    public List<NoteData> Notes { get; private set; }

    public List<NoteData> ParseFromText(string textData)
    {
        Notes = new List<NoteData>();

        if (string.IsNullOrWhiteSpace(textData))
        {
            Debug.LogError("Empty or null text data provided to ParseFromText.");
            return Notes;
        }

        Debug.Log("Parsing note data...");

        string[] lines = textData.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        ParseLines(lines);

        return Notes;
    }

    private void ParseLines(string[] lines)
    {
        bool isHitObjectsSection = false;
        bool isTimingPointsSection = false;

        foreach (var line in lines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine == "[HitObjects]")
            {
                isHitObjectsSection = true;
                isTimingPointsSection = false;
                continue;
            }

            if (trimmedLine == "[TimingPoints]")
            {
                isTimingPointsSection = true;
                isHitObjectsSection = false;
                continue;
            }

            if (isTimingPointsSection)
            {
                ParseTimingPoint(trimmedLine);
            }

            if (isHitObjectsSection)
            {
                ParseHitObject(trimmedLine);
            }
        }
    }

    private void ParseHitObject(string line)
    {
        string[] parts = line.Split(',');
        if (parts.Length >= 4)
        {
            bool isLongNote = (int.Parse(parts[3]) & 128) > 0;
            int lane = ConvertToLane(int.Parse(parts[0]));
            float time = float.Parse(parts[2]) / 1000f;

            if (isLongNote)
            {
                Notes.Add(new NoteData { Time = time, Lane = lane, IsLongNote = false });
            }
            else
            {
                Notes.Add(new NoteData { Time = time, Lane = lane, IsLongNote = false });
            }
        }
    }

    private void ParseTimingPoint(string line)
    {
        string[] parts = line.Split(',');
        if (parts.Length > 1)
        {
            float beatLengthMs = float.Parse(parts[1]);
            if (beatLengthMs > 0)
            {
                BPM = 60000f / beatLengthMs;
            }
        }
    }

    private int ConvertToLane(int x)
    {
        if (x < 128) return 0;
        if (x < 256) return 1;
        if (x < 384) return 2;
        return 3;
    }
}
//github upload