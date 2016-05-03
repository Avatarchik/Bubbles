using System;
using UnityEngine;
using System.Collections;
using System.Reflection.Emit;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Action playGame;
    public Action<string> connectGame;

    public Text timer;
    public Text score;

    public Button connect;
    public Button play;

    public InputField field;

    private int scoreValue;

    private TimeSpan timeSpan = new TimeSpan();

    void Awake()
    {
        play.onClick.AddListener(() =>
        {
            HideButtons();
            if (playGame != null)
            {
                playGame();
            }

            MonoTimer.Timer.onTick += Timer_onTick;
            scoreValue = 0;
        });
        connect.onClick.AddListener(() =>
        {
            if (connectGame != null)
            {
                connectGame(field.text);
            }
        });
    }

    private void HideButtons()
    {
        connect.gameObject.SetActive(false);
        play.gameObject.SetActive(false);
        field.gameObject.SetActive(false);

    }

    private void Timer_onTick()
    {
        timeSpan = timeSpan.Add(new TimeSpan(0, 0, 0, 1));
        timer.text = string.Format("{0:hh\\:mm\\:ss}", timeSpan);
    }

    public void AddScore(int score)
    {
        scoreValue += score;
        this.score.text = scoreValue.ToString();
    }
}
