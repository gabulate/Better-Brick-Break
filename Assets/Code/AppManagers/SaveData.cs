using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public uint gamesPlayed = 0;
    public uint maxScore = 0;
    public uint lastScore = 0;
    public uint maxHitsBall = 0; //Ball with the most block hits in a single turn
    public uint maxTurns = 0; //Max turns played in a single match
    public uint maxBalls = 0;

    public List<KeyValuePair> maxScores = new List<KeyValuePair>();

    public string theme = "Default";
    public float volume = 1f;
    public bool showTrails = true;
    public bool showParticles = true;
}

[System.Serializable]
public class KeyValuePair
{
    public string key;
    public uint value;

    public KeyValuePair(string key, uint value)
    {
        this.key = key;
        this.value = value;
    }
}

public static class KeyValuePairExtensions
{
    public static bool Contains(this List<KeyValuePair> list, string key)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].key == key)
            {
                return true;
            }
        }

        return false;
    }

    public static uint Get(this List<KeyValuePair> list, string key)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].key == key)
            {
                return list[i].value;
            }
        }

        return 0;
    }

    public static void UpdateValue(this List<KeyValuePair> list, string key, uint value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].key == key)
            {
                list[i].value = value;
                return;
            }
        }

        Debug.LogError("Key " + key + " wasn't found.");
    }

    public static void Add(this List<KeyValuePair> list, string key, uint value)
    {
        list.Add(new KeyValuePair(key, value));
    }
}
