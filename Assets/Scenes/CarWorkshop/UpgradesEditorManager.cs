using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesEditorManager : MonoBehaviour, OnClickListener, OnCarSelectedListener
{
    [SerializeField]
    private UpgradeElementController upgradeEngine;
    [SerializeField]
    private UpgradeElementController upgradeWheels;
    [SerializeField]
    private UpgradeElementController upgradeBreaks;
    [SerializeField]
    private Text coinsText;
    [SerializeField]
    private CarListContentLoader carListContent;

    private GameData data;
    private CarUpgrades selectedCarUpgrades;

    void Start()
    {
        upgradeEngine.upgradeButton.gameObject.AddComponent<OnClickAction>().registerListener(this);
        upgradeWheels.upgradeButton.gameObject.AddComponent<OnClickAction>().registerListener(this);
        upgradeBreaks.upgradeButton.gameObject.AddComponent<OnClickAction>().registerListener(this);

        this.data = DataManager.getInstance().getGameData();

        carListContent.registerListener(this);
    }

    void Update() {
        this.coinsText.text = data.coins + "$";
    }

    public void onClick(GameObject gameObject) {
        if (!gameObject.GetComponent<Button>().interactable)
            return;
        if (gameObject == upgradeEngine.upgradeButton.gameObject) {
            upgrade(upgradeEngine);
            return;
        }
        if (gameObject == upgradeWheels.upgradeButton.gameObject) {
            upgrade(upgradeWheels);
            return;
        }
        if (gameObject == upgradeBreaks.upgradeButton.gameObject) {
            upgrade(upgradeBreaks);
            return;
        }
    }

    private void upgrade(UpgradeElementController controller) {
        if(controller.gameObject.name.Equals("Engine")) {
            selectedCarUpgrades.engine++;
        }else if (controller.gameObject.name.Equals("Wheels")) {
            selectedCarUpgrades.wheels++;
        }else if (controller.gameObject.name.Equals("Breaks")) {
            selectedCarUpgrades.breaks++;
        }
        this.data.coins -= controller.getUpgradeCost();
        controller.upgrade();
    }

    public void onCarSelected(string name) {
        selectedCarUpgrades = this.data.getCarUpgrades(name);
        upgradeEngine.setCurrentUpgradeLevel(selectedCarUpgrades.engine);
        upgradeWheels.setCurrentUpgradeLevel(selectedCarUpgrades.wheels);
        upgradeBreaks.setCurrentUpgradeLevel(selectedCarUpgrades.breaks);
    }

    public void backToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
