using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData{
    public IDictionary<string, float> playerRecords;
    public int coins;
    public IDictionary<string, CarUpgrades> carUpgrades;

    public GameData() {
        this.coins = 0;
    }

    public float getPlayerRecord(string levelName) {
        float record = 0;
        if (playerRecords != null)
            playerRecords.TryGetValue(levelName, out record);
        return record;
    }

    public void addNewRecord(string levelName, float recordTime) {
        if (this.playerRecords == null)
            this.playerRecords = new Dictionary<string, float>();
        if (recordTime <= 0)
            this.playerRecords.Remove(levelName);
        else
            this.playerRecords[levelName] = recordTime;
    }

    public float getPlayerRecord() {
        return getPlayerRecord(SceneManager.GetActiveScene().name);
    }

    public void addNewRecord(float recordTime) {
        addNewRecord(SceneManager.GetActiveScene().name, recordTime);
    }

    public CarUpgrades getCarUpgrades(string carName) {
        if(this.carUpgrades == null)
            this.carUpgrades = new Dictionary<string, CarUpgrades>();
        CarUpgrades upgrades = null;
        this.carUpgrades.TryGetValue(carName, out upgrades);
        if (upgrades == null) {
            upgrades = new CarUpgrades();
            this.carUpgrades[carName] = upgrades;
        }
        return upgrades;
    }

    public float getMotorTorque(CarUpgrades upgrades) {
        float motorTorque = 300;
        motorTorque += 15 * upgrades.engine;
        return motorTorque;
    }

    public float getBreakForce(CarUpgrades upgrades) {
        float breakForce = 90000;
        breakForce += 2000 * upgrades.breaks;
        return breakForce;
    }

    public float getMaxAngle(CarUpgrades upgrades) {
        float maxAngle = 40;
        maxAngle += upgrades.wheels;
        return maxAngle;
    }
}
