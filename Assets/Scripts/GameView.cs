using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI diceCountText;

    [SerializeField] private TMP_InputField diceCountInput;
    [SerializeField] private TMP_InputField drawInput;
    [SerializeField] private TMP_InputField winInput;
    [SerializeField] private TMP_InputField loseInput;

    [SerializeField] private Button rollButton;

    public UnityEvent<string, string, string, string> onRollRequested;

    private void Awake()
    {
        rollButton.onClick.AddListener(OnRollClicked);
        totalText.text = "Ждемс бросок";
    }
    private void OnDestroy()
    {
        rollButton.onClick.RemoveListener(OnRollClicked);
    }

    private void OnRollClicked()
    {
        onRollRequested.Invoke(diceCountInput.text, loseInput.text, drawInput.text, winInput.text);
    }

    public void ShowDiceCount(int count)
    {
        diceCountText.text = "Кубиков: " + count;
    }
    public void ShowThresholds(int lose, int draw, int win)
    {
        drawInput.text = draw.ToString();
        winInput.text = win.ToString();
        loseInput.text = lose.ToString();
    }
    public void ShowRolling()
    {
        totalText.text = "Бросаем";
        rollButton.interactable = false;
    }
    public void ShowResult(int total, RoundResult result)
    {
        switch (result)
        {
            case RoundResult.Lose:
                totalText.text = "Выпало:" + total + " Поражение";
                break;
            case RoundResult.Win:
                totalText.text = "Выпало:" + total + " Победа";
                break;
            case RoundResult.Draw:
                totalText.text = "Выпало:" + total + " Ничья";
                break;
        }
        rollButton.interactable = true;
    }
}