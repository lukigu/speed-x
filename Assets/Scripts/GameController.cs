using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [SerializeField]
    private int countdown;

    private float startTime;
    private float raceStartTime;
    private float raceFinishTime;

    private float raceTime;
    private int countdownValue;

    private float previousCountdownTime;

    private List<GameTimeListener> gameTimeListeners;

    private Mode mode;


    public GameController() {
        this.gameTimeListeners = new List<GameTimeListener>();
        this.mode = Mode.NONE;
    }

    public void Start() {
        start();
    }

    void Update()
    {
        float currentFrameTime = getCurrentTime();
        switch(mode) {
            case Mode.COUNTINGDOWN:
                if(currentFrameTime - this.previousCountdownTime >= 1) {
                    this.countdownValue--;
                    this.previousCountdownTime += 1;
                    foreach (GameTimeListener listener in this.gameTimeListeners)
                        listener.onCountDown();
                }
                if(this.countdownValue <= 0) {
                    startRace();
                }
                break;
            case Mode.RACE:
                this.raceTime = getCurrentTime() - this.raceStartTime;
                break;
        }
    }

    public void start() {
        reset();
        this.mode = Mode.COUNTINGDOWN;
        foreach (GameTimeListener listener in this.gameTimeListeners)
            listener.onCountDown();
    }

    public void finishRace() {
        if (mode == Mode.NONE || mode == Mode.FINISHED)
            return;
        this.mode = Mode.FINISHED;
        this.raceFinishTime = getCurrentTime();
        this.raceTime = this.raceFinishTime - this.raceStartTime;
        float currentRecord = DataManager.getInstance().gameData.getPlayerRecord();
        if (currentRecord == 0 || currentRecord > this.raceTime)
            DataManager.getInstance().getGameData().addNewRecord(SceneManager.GetActiveScene().name, this.raceTime);
        foreach (GameTimeListener listener in this.gameTimeListeners)
            listener.onRaceFinish();

    }

    private void startRace() {
        if (mode == Mode.RACE)
            throw new Exception("Race is already running!");
        this.mode = Mode.RACE;
        this.raceStartTime = getCurrentTime();
        foreach (GameTimeListener listener in this.gameTimeListeners)
            listener.onRaceStart();
    }

    public void reset() {
        this.countdownValue = this.countdown;
        this.startTime = getCurrentTime();
        this.previousCountdownTime = this.startTime;
    }

    private float getCurrentTime() {
        return Time.fixedTime;
    }

    public void registerGameTimeListener(GameTimeListener listener) {
        this.gameTimeListeners.Add(listener);
    }

    public void unregisterGameTimeListener(GameTimeListener listener) {
        this.gameTimeListeners.Remove(listener);
    }

    public float getRaceTime() {
        return this.raceTime;
    }

    public int getCountdownValue() {
        return this.countdownValue;
    }

    public Mode getMode() {
        return this.mode;
    }

    public enum Mode {
        NONE, COUNTINGDOWN, RACE, FINISHED
    }

    public interface GameTimeListener {
        void onCountDown();
        void onRaceStart();
        void onRaceFinish();
    }
}
