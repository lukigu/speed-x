using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CarListContentLoader : MonoBehaviour, OnClickListener {

    private float carAlpha = 0.45f;

    private List<GameObject> cars;
    private GameObject selectedCar;

    private OnCarSelectedListener listener;

    public CarListContentLoader() {
        cars = new List<GameObject> ();
    }

    void Awake() {
        Sprite[] carSprites = Resources.LoadAll("Cars", typeof(Sprite)).Cast<Sprite>().ToArray();
        Debug.Log("Loaded " + carSprites.Length + " cars.");
        foreach(Sprite carSpriteObj in carSprites) {
            GameObject root = new GameObject(carSpriteObj.ToString());
            root.name = carSpriteObj.name;
            Image bg = root.AddComponent<Image>();
            bg.color = new Color(255,255,255);
            CanvasGroup cg = root.AddComponent<CanvasGroup>();
            cg.alpha = carAlpha;

            OnClickAction clickAction = root.AddComponent<OnClickAction>();
            clickAction.registerListener(this);

            GameObject carObject = new GameObject(carSpriteObj.ToString());
            Image carImage = carObject.AddComponent<Image>();
            carImage.sprite = carSpriteObj;

            root.GetComponent<Transform>().SetParent(this.gameObject.transform);
            root.SetActive(true);

            carObject.GetComponent<Transform>().GetComponent<Transform>().SetParent(root.transform);
            carObject.SetActive(true);

            RectTransform rootTransform = root.GetComponent<RectTransform>();
            rootTransform.anchoredPosition3D = new Vector3(0,0,0);
            rootTransform.localScale = new Vector3(1, 1, 1);
            rootTransform.sizeDelta = new Vector2(250, 100);

            RectTransform carObjectTransform = carObject.GetComponent<RectTransform>();
            carObjectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            carObjectTransform.localScale = new Vector3(1, 1, 1);
            carObjectTransform.sizeDelta = new Vector2(250, 100);

            cars.Add(root);
        }
        if (cars.Count > 0) {
            selectCar(cars[0]);
        }
    }

    private void selectCar(GameObject carElement) {
        if(selectedCar != null)
            selectedCar.GetComponent<CanvasGroup>().alpha = carAlpha;
        selectedCar = carElement;
        selectedCar.GetComponent<CanvasGroup>().alpha = 1;
        if(listener != null)
            listener.onCarSelected(selectedCar.name);
    }

    public void onClick(GameObject gameObject) {
        Debug.Log("onClick(): " + gameObject.name);
        selectCar(gameObject);
    }

    public void registerListener(OnCarSelectedListener listener) {
        this.listener = listener;
        listener.onCarSelected(selectedCar.name);
    }
}
