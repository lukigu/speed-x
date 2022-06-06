using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementController : MonoBehaviour {

    private const int minCost = 50;
    private const int costPerLvl = 25;

    private const int maxLvl = 10;

    private int currentLvl;

    public Image image;
    public Text currentUpgradeLvl;
    public Button upgradeButton;
    public Text upgradeButtonText;

    private GameData gameData;

    void Awake() {
        image = this.gameObject.GetComponentInChildren<Image>();
        currentUpgradeLvl = this.gameObject.GetComponentInChildren<Text>();
        upgradeButton = this.gameObject.GetComponentInChildren<Button>();
        upgradeButtonText = upgradeButton.gameObject.GetComponentInChildren<Text>();

        this.gameData = DataManager.getInstance().getGameData();

        setCurrentUpgradeLevel(0);
    }

    private void Update() {
        if (maxLvl == currentLvl)
            return;
        if(this.gameData.coins < getUpgradeCost(currentLvl + 1)) {
            upgradeButton.interactable = false;
            upgradeButtonText.color = Color.red;
        } else {
            upgradeButton.interactable = true;
            upgradeButtonText.color = Color.black;
        }
    }

    public void upgrade() {
        setCurrentUpgradeLevel(currentLvl + 1);
    }

    public void setCurrentUpgradeLevel(int lvl) {
        currentLvl = Mathf.Clamp(lvl, 0, maxLvl);
        currentUpgradeLvl.text = currentLvl + "/" + maxLvl;

        if (lvl != maxLvl) {
            upgradeButtonText.text = "UPGRADE\n -" + getUpgradeCost(currentLvl + 1) + "$";
            upgradeButton.interactable = true;
        }else {
            upgradeButtonText.text = "UPGRADE\nMAX";
            upgradeButton.interactable = false;
        }
    }

    public int getUpgradeCost() {
        return getUpgradeCost(currentLvl + 1);
    }

    public int getUpgradeCost(int lvl) {
        return minCost + (costPerLvl * lvl);
    }
    public int getUpgradeLvl() {
        return this.currentLvl;
    }
}
