using UnityEngine;
[DefaultExecutionOrder(-100)]

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;

    private void Awake()
    {
        model.OnDiceCountChanged += view.ShowDiceCount;
        model.OnThresholdsGenerated += view.ShowThresholds;
        model.OnRollStarted += view.ShowRolling;
        model.OnRoundFinished += view.ShowResult;

        view.onRollRequested.AddListener(HandleRollRequested);
    }
    private void OnDestroy()
    {
        model.OnDiceCountChanged -= view.ShowDiceCount;
        model.OnThresholdsGenerated -= view.ShowThresholds;
        model.OnRollStarted -= view.ShowRolling;
        model.OnRoundFinished -= view.ShowResult;

        view.onRollRequested.RemoveListener(HandleRollRequested);
    }

    private void HandleRollRequested(string dice, string lose, string draw, string win)
    {
        if (int.TryParse(dice, out int diceCount))
        {
            model.SetDiceCount(diceCount);
        }

        int loseScore = 0;
        int drawScore = 0;
        int winScore = 0;

        bool allParsed =
            int.TryParse(lose, out loseScore) &&
            int.TryParse(draw, out drawScore) &&
            int.TryParse(win, out winScore);

        if (allParsed)
            model.SetThresholds(loseScore, drawScore, winScore);
        else
            model.GenerateThresholds();

        model.StartRoll();
    }
}
