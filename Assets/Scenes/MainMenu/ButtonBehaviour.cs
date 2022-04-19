using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IOnUpdateInterpolation<Color>
{
    [SerializeField]
    private Color highlightedTextColor;
    private Color normalTextColor;
    [SerializeField]
    private float textColorAnimationTime;
    [SerializeField]
    private Color highlightedButtonColor;
    private Color normalButtonColor;
    [SerializeField]
    private float buttonColorAnimationTime;

    private Text text;
    private Image buttonColor;

    private ColorInterp textColorInterp;
    private ColorInterp buttonColorInterp;

    private IOnUpdateInterpolation<Color> onUpdateTextColor;
    private IOnUpdateInterpolation<Color> onUpdateButtonColor;

    void Start()
    {
        this.text = GetComponentInChildren<Text>();
        this.normalTextColor = this.text.color;
        this.textColorInterp = new ColorInterp(this.textColorAnimationTime, this);

        this.buttonColor = this.transform.GetChild(0).GetComponent<Image>();
        this.normalButtonColor = this.buttonColor.color;
        this.buttonColorInterp = new ColorInterp(this.buttonColorAnimationTime, this);
    }

    void Update()
    {
        this.textColorInterp.update();
        this.buttonColorInterp.update();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.textColorInterp.reset(this.text.color, this.highlightedTextColor);
        this.buttonColorInterp.reset(this.buttonColor.color, this.highlightedButtonColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        this.textColorInterp.reset(this.text.color, this.normalTextColor);
        this.buttonColorInterp.reset(this.buttonColor.color, this.normalButtonColor);
    }

    public void onUpdateInterpolation(Interpolator<Color> interpolator, Color currentValue)
    {
        if(interpolator == this.textColorInterp) {
            this.text.color = currentValue;
        }else if(interpolator == this.buttonColorInterp) {
            this.buttonColor.color = currentValue;
        }
    }
}
