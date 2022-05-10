using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsConfiguration {
    private const string fileName = "coinsConfig.json";

    private CoinsConfig coinsConfig;

    public CoinsConfiguration() {
        string rawJsonData = null;
        try {
            rawJsonData = System.IO.File.ReadAllText(fileName);
        } catch (System.Exception e) {
            Debug.Log(e.StackTrace);
        }
        if (rawJsonData != null) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = HandleDeserializationError;
            this.coinsConfig = JsonConvert.DeserializeObject<CoinsConfig>(rawJsonData, settings);
        }
        if (this.coinsConfig == null) {
            this.coinsConfig = new CoinsConfig();
        }

        foreach (KeyValuePair<string, LevelCoinsConfig> level in this.coinsConfig.levels) {
            foreach (TimeBonus timeBonus in level.Value.timeBonuses) {
                timeBonus.time = string2time(timeBonus.tTime);
            }
            level.Value.timeBonuses.Sort();
        }
    }

    private void HandleDeserializationError(object sender, ErrorEventArgs e) {
        e.ErrorContext.Handled = true;
    }

    public int calcCoins(string levelName, float previousTime, float newTime) {
        if (previousTime <= 0)
            previousTime = Int32.MaxValue;
        int coins = 0;
        LevelCoinsConfig levelConfig;
        this.coinsConfig.levels.TryGetValue(levelName, out levelConfig);
        if(levelConfig == null) {
            coins += coinsConfig.defaultCoins;
            if (newTime < previousTime)
                coins += coinsConfig.defaultRecordBonus;
            return coins;
        }
        coins += levelConfig.defaultCoins;
        if (newTime < previousTime)
            coins += levelConfig.recordBonus;
        foreach (TimeBonus timeBonus in levelConfig.timeBonuses) {
            if (newTime > timeBonus.time) {
                break;
            }
            coins += timeBonus.defaultCoins;
            if (previousTime > timeBonus.time) {
                coins += timeBonus.first;
            }
        }
        return coins;
    }

    public float string2time(string value) {
        float time = 0;
        string[] a = value.Split(':');
        time += parseInt(a[1]);
        time += parseInt(a[0]) * 60;
        return time;
    }

    public int parseInt(string value) {
        try {
            return Int32.Parse(value);
        }catch(Exception e) {
            Debug.Log(e.StackTrace);
            return 0;
        }
    }

    public class CoinsConfig{
        public int defaultCoins;
        public int defaultRecordBonus;
        public IDictionary<string, LevelCoinsConfig> levels;
    }

    public class LevelCoinsConfig {
        public int defaultCoins;
        public int recordBonus;
        public List<TimeBonus> timeBonuses;
    }

    public class TimeBonus : IComparable<TimeBonus> {
        public string tTime;
        public float time;
        public int first;
        public int defaultCoins;

        public int CompareTo(TimeBonus other) {
            if (other.time < time)
                return -1;
            return -1;
        }
    }
}
