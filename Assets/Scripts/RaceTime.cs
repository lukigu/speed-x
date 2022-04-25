using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTime : MonoBehaviour, GameController.GameTimeListener {

    private GameController gameController;

    void Awake() {
        this.gameController = FindObjectOfType<GameController>();
        this.gameController.registerGameTimeListener(this);

        float record = DataManager.getInstance().gameData.getPlayerRecord();
        if (record <= 0)
            Debug.Log("Brak rekordu na tej mapie!");
        else
            Debug.Log("Aktualny rekord na tej mapie: " + time2String(record));
    }

    void Update() {
        if(this.gameController.getMode() == GameController.Mode.RACE && this.gameController.getRaceTime() > 10) {
            this.gameController.finishRace();
        }
    }

    public void onCountDown() {
        Debug.Log("Odliczanie: " + this.gameController.getCountdownValue()) ;
    }

    public void onRaceFinish() {
        Debug.Log("Wyscig zakonczony : " + time2String(this.gameController.getRaceTime()));
    }

    public void onRaceStart() {
        Debug.Log("Wyscig rozpoczety");
    }

    public string time2String(float time) {
        int timeInt = (int)(time * 1000);
        int seconds = timeInt / 1000;
        int milis = timeInt % 1000;
        string result = seconds + ":";
        for (int i = 0; i < 3 - milis.ToString().Length; i++)
            result += "0";
        result += milis + "";
        return result;
    }


}