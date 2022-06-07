using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageCompletedController : MonoBehaviour, GameController.GameTimeListener {
    private GameController gameController;

    [SerializeField]
    private GameObject timerUI;
    [SerializeField]
    private GameObject stageCompletedUI;
    [SerializeField]
    private Text timeLabel;
    [SerializeField]
    private Text cointsLabel;

    void Start() {
        this.gameController = FindObjectOfType<GameController>();
        this.gameController.registerGameTimeListener(this);
    }

    void Update() {

    }

    public void onRaceFinished() {
        Debug.Log("Wyscig zakonczony : " + RaceTime.time2String(this.gameController.getRaceTime()));
        this.timeLabel.text = RaceTime.time2String(this.gameController.getRaceTime());
        this.cointsLabel.text = "+" + this.gameController.getCoins() + "$";
        this.stageCompletedUI.SetActive(true);
        this.timerUI.SetActive(false);
    }

    public void onCountDown() {}

    public void onRaceStarted() {}
}