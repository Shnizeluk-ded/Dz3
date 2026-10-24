using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class GameModel : MonoBehaviour
{
    public int DiceCount { get; private set; } = 3;
    public int WinScore { get; private set; }
    public int DrawScore { get; private set; }
    public int LoseScore { get; private set; }
    public int Total { get; private set; }

    public event Action<int> OnDiceCountChanged;
    public event Action<int, int, int> OnThresholdsGenerated; 
    public event Action OnRollStarted;
    public event Action<int, RoundResult> OnRoundFinished;

    private void Awake()
    {
        SetDiceCount(3);
        GenerateThresholds();
    }

    public void SetDiceCount(int n)
    {
        DiceCount = n;
        OnDiceCountChanged?.Invoke(DiceCount);
    }
    public void SetThresholds(int lose, int draw, int win)
    {
        int[] values = { lose, draw, win };
        Array.Sort(values);
        LoseScore = values[0];
        DrawScore = values[1];
        WinScore = values[2];
        OnThresholdsGenerated?.Invoke(LoseScore, DrawScore, WinScore);
    }
    public void GenerateThresholds()
    {
        int[] values = { Random.Range(DiceCount, DiceCount*6 + 1),
            Random.Range(DiceCount, DiceCount * 6 + 1),
            Random.Range(DiceCount, DiceCount * 6 + 1)};

        Array.Sort(values);
        SetThresholds(values[0], values[1], values[2]);
    }

    public void StartRoll()
    {
        OnRollStarted?.Invoke();
    }
    public void Finish(int total)
    {
        RoundResult roundResult;

        Total = total;

        if (total >= WinScore)
            roundResult = RoundResult.Win;
        else if(total >= DrawScore)
            roundResult = RoundResult.Draw;
        else 
            roundResult = RoundResult.Lose;

        OnRoundFinished?.Invoke(total, roundResult);
    }
}