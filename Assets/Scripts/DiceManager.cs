using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private List<Dice> dices = new List<Dice>();
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private InputActionReference throwActionRef;

    private bool isCalculatingScore;
    private bool allStopped;
    private int totalScore;

    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Нажмите Space";
        }
    }

    private void OnEnable()
    {
        if (throwActionRef != null && throwActionRef.action != null)
        {
            throwActionRef.action.Enable();
            throwActionRef.action.performed += OnThrowPressed;
        }
    }

    private void OnDisable()
    {
        if (throwActionRef != null && throwActionRef.action != null)
        {
            throwActionRef.action.performed -= OnThrowPressed;
            throwActionRef.action.Disable();
        }
    }

    private void OnThrowPressed(InputAction.CallbackContext context)
    {
        if (isCalculatingScore) return;

        if (scoreText != null)
        {
            scoreText.text = "Кубики брошены";
        }

        foreach (Dice dice in dices)
        {
            if (dice != null) dice.Roll();
        }

        StartCoroutine(WaitAndCalculateScoreRoutine());
    }

    private IEnumerator WaitAndCalculateScoreRoutine()
    {
        isCalculatingScore = true;

        //небольшая пауза чтобы кубики успели набрать скорость
        yield return new WaitForSeconds(0.2f);

        //ждём пока все кубики остановятся
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

        //считаем сумму очков
        totalScore = 0;
        foreach (Dice dice in dices)
        {
            if (dice != null) totalScore += dice.GetUpwardValue();
        }

        if (scoreText != null)
        {
            scoreText.text = "Сумма очков: " + totalScore + "\nНажмите Space";
        }

        isCalculatingScore = false;
    }
}