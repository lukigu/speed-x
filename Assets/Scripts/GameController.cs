using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject completestagescreen;

    [SerializeField]
    //Wartoœæ startowa odliczania(domyœlnie 3s)
    private int countdown = 3;

    //Czas startu odliczania
    private float startTime;
    //Czas startu wyœcigu(wymagany od obliczania czasu) - czas systemowy
    private float raceStartTime;
    //Czas zakoñczenia wyœcigu(tylko informacyjnie/na przysz³oœæ) - czas systemowy
    private float raceFinishTime;

    //Aktualna wartoœæ odliczania
    private int countdownValue;
    //Czas zmniejszenia licznika odliczania(licznik zostaje zmniejszony o 1 gdy roznica aktualnego czasu i tej wartosci > 1s)
    private float previousCountdownTime;

    //Aktualny czas wyœcigu (wyswietlany w interfejsie), po zakonczeniu wyscigu = raceFinishTime - startFinishTime
    private float raceTime;
    //Suma zdobtych monent za wyscig wyswietlana na ekranie koncowym (Wartoœæ domyœlna + bonus w przypadku nowego rekordu)
    public int coins;
    //Po zakonczeniu przejazdu: czy jest nowym rekordem (informacyjnie)
    public bool record;

    //Lista sluchaczy, ktorzy zostaj¹ poinformowani o zdarzeniach: rozpoczecie odliczania, rozpoczecie i zakonczenie wyscigu
    private List<GameTimeListener> gameTimeListeners;

    //Aktualny status: ODLICZANIE, WYSCIG, ZAKONCZONY
    private Mode mode;

    //Ustawienia monet: domyslna wartoœæ i wartoœci bonusowe za rekord
    private CoinsConfiguration coinsConfiguration;

    //Menadzer danych - odpowiedzialny za dostep danych w aplikacji oraz zapis i odczyt z dysku
    private DataManager dataManager;

    public GameController() {
        this.gameTimeListeners = new List<GameTimeListener>();
        this.mode = Mode.NONE;
        this.coinsConfiguration = new CoinsConfiguration();
        this.dataManager = DataManager.getInstance();
    }

    public void Awake() {
        start();
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
        Debug.Log("finishRace");
        if (mode == Mode.NONE || mode == Mode.FINISHED)
            return;
        this.mode = Mode.FINISHED;
        this.raceFinishTime = getCurrentTime();
        this.raceTime = this.raceFinishTime - this.raceStartTime;
        float currentRecord = DataManager.getInstance().gameData.getPlayerRecord();
        Debug.Log("Record: " + currentRecord);
        this.coins = this.coinsConfiguration.calcCoins(SceneManager.GetActiveScene().name, currentRecord, this.raceTime);
        this.dataManager.getGameData().coins += coins;
        if (currentRecord == 0 || currentRecord > this.raceTime) {
            this.record = true;
            this.dataManager.getGameData().addNewRecord(SceneManager.GetActiveScene().name, this.raceTime);
        }
        this.dataManager.saveData();
        foreach (GameTimeListener listener in this.gameTimeListeners)
            listener.onRaceFinished();
    }

    private void startRace() {
        if (mode == Mode.RACE)
            throw new Exception("Race is already running!");
        this.mode = Mode.RACE;
        this.raceStartTime = getCurrentTime();
        foreach (GameTimeListener listener in this.gameTimeListeners) {
            listener.onRaceStarted();
        }
    }

    public void reset() {
        this.countdownValue = this.countdown;
        this.startTime = getCurrentTime();
        this.raceStartTime = 0;
        this.previousCountdownTime = this.startTime;
        this.record = false;
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

    public int getCoins() {
        return this.coins;
    }

    public enum Mode {
        NONE, COUNTINGDOWN, RACE, FINISHED
    }

    public interface GameTimeListener {
        void onCountDown();
        void onRaceStarted();
        void onRaceFinished();
    }

    public void completelevel()
    {
        Debug.Log("Stage Completed");
        //finishRace();
    }

}
