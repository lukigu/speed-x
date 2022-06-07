using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTime : MonoBehaviour, GameController.GameTimeListener, IOnUpdateInterpolation<float> {

    private GameController gameController;

    [Header("UI")]
    public Text timerLabel;
    public Text countdownLabel;

    private FInterp countdownScale;
    private FInterp countdownAlpha;

    void Awake() {
        this.gameController = FindObjectOfType<GameController>();
        this.gameController.registerGameTimeListener(this);

        float record = DataManager.getInstance().gameData.getPlayerRecord();
        if (record <= 0)
            Debug.Log("Brak rekordu na tej mapie!");
        else
            Debug.Log("Aktualny rekord na tej mapie: " + time2String(record));

        this.timerLabel.enabled = false;
        this.countdownLabel.enabled = true;
        this.countdownScale = new FInterp(1, 0.6f, 1f, this);
        this.countdownAlpha = new FInterp(1, 0.0f, 0.7f, this);
        this.countdownAlpha.setDelay(0.2f);
    }

    void Update() {
        this.countdownScale.update();
        this.countdownAlpha.update();
        timerLabel.text = time2String(this.gameController.getRaceTime(), 1);
    }

    public void onCountDown() {
        this.countdownScale.reset();
        this.countdownAlpha.reset();
        Debug.Log("Odliczanie: " + this.gameController.getCountdownValue()) ;
        this.countdownLabel.text = this.gameController.getCountdownValue().ToString();
    }

    public void onRaceFinished() {
        Debug.Log("Wyscig zakonczony : " + time2String(this.gameController.getRaceTime()));
    }

    public void onRaceStarted() {
        this.timerLabel.enabled = true;
        this.countdownLabel.enabled = false;
        Debug.Log("Wyscig rozpoczety");
    }

    public static string time2String(float time) {
        return time2String(time, 3);
    }

    public static string time2String(float time, int a) {
        int b = (int) Math.Pow(10, a);
        int timeInt = (int)(time * b);
        int seconds = timeInt / b;
        int minutes = seconds/60;
        if (minutes > 0)
            seconds = seconds % 60;
        int milis = timeInt % b;
        string result = seconds + ":";
        for (int i = 0; i < a - milis.ToString().Length; i++)
            result += "0";
        if (seconds < 10) {
            result = "0" + result;
        }
        result += milis + "";
        result = minutes + ":" + result;
        if (minutes < 10)
            result = "0" + result;
        return result;
    }

    public void onUpdateInterpolation(Interpolator<float> interpolator, float currentValue) {
        if (interpolator == this.countdownScale) {
            this.countdownLabel.transform.localScale = new Vector3(currentValue, currentValue, 1);
            return;
        }
        this.countdownLabel.color = new Color(255, 255, 255, currentValue);
    }
}
