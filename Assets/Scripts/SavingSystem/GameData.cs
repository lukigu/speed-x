using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData{
    public IDictionary<string, float> playerRecords;

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
}
