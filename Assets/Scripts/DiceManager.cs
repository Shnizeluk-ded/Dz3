using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiceManager : MonoBehaviour
{
    private List<Dice> dices = new List<Dice>();

    private bool allStopped;
    private int totalScore;

    [SerializeField] private GameModel model; 
    [SerializeField] private Dice dicePrefab;

    private void Awake()
    {
        model.OnRollStarted += StartRoll;
    }

    private void OnDestroy()
    {
        model.OnRollStarted -= StartRoll;
    }

    private IEnumerator WaitAndCalculateScoreRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        allStopped = false;
        while (!allStopped)
        {
            allStopped = true;
            foreach (Dice dice in dices)
            {
                if (dice != null && dice.isRolling)
                {
                    allStopped = false;
                    break;
                }
            }
            yield return null;
        }

        totalScore = 0;
        foreach (Dice dice in dices)
        {
            if (dice != null) totalScore += dice.GetUpwardValue();
        }

        model.Finish(totalScore);
    }

    private void StartRoll()
    {
        foreach (Dice dice in dices)
        {
            Destroy(dice.gameObject);
        }
        dices.Clear();

        for (int i = 0; i < model.DiceCount; i++)
        {
            var dice = Instantiate(dicePrefab, new Vector3(i * 1.5f, 1f, 0f), Quaternion.identity);
            dices.Add(dice);
            dice.Roll();
        }
        StartCoroutine(WaitAndCalculateScoreRoutine());
    }
}