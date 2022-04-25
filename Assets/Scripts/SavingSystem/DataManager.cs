using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance;

    public static DataManager getInstance() {
        if (instance == null)
            instance = new DataManager();
        return instance;
    }

    const string gameSaveFilePath = "playerData.json";

    public GameData gameData;

    public DataManager() {
        loadGameDataFromFile();
        Application.quitting += Application_quitting;
    }

    private void Application_quitting() {
        saveData();
    }

    private void loadGameDataFromFile() {
        string rawJsonData = null;
        try {
            rawJsonData = System.IO.File.ReadAllText(gameSaveFilePath);
        } catch (System.Exception e) {
            Debug.Log(e.StackTrace);
        }
        if (rawJsonData != null) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = HandleDeserializationError;
            this.gameData = JsonConvert.DeserializeObject<GameData>(rawJsonData, settings);
        }
        if(this.gameData == null) {
            this.gameData = new GameData();
            saveData();
        }
    }

    public void saveData() {
        string rawJsonData = JsonConvert.SerializeObject(this.gameData);
        Debug.Log("Saving: " + rawJsonData);
        try {
            System.IO.File.WriteAllText(gameSaveFilePath, rawJsonData);
        } catch (System.Exception e) {
            Debug.Log(e.StackTrace);
        }
    }

    public GameData getGameData() {
        return this.gameData;
    }
    public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs) {
        errorArgs.ErrorContext.Handled = true;
    }
}
