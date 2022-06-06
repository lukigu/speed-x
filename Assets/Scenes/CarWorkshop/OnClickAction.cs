using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickAction : MonoBehaviour, IPointerClickHandler {

    private OnClickListener onClickListener;

    public void OnPointerClick(PointerEventData eventData) {
        if (onClickListener == null)
            return;
        onClickListener.onClick(this.gameObject);
    }

    public void registerListener(OnClickListener listener) {
        this.onClickListener = listener;
    }
}
