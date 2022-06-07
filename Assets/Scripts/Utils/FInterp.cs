using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInterp : Interpolator<float>, ICalculatorInterpolation<float> {
    public FInterp(float fullTime, IOnUpdateInterpolation<float> onUpdateListener) : base(fullTime, null, onUpdateListener) {
        this.calculatorInterpolation = this;
    }

    public FInterp(float startingValue, float targetValue, float fullTime, IOnUpdateInterpolation<float> onUpdateListener) : base(startingValue, targetValue, fullTime, null, onUpdateListener) {
        this.calculatorInterpolation = this;
    }

    public float calculateInterpolatedValue(float startingValue, float targetValue, float t) {
        return Mathf.Lerp(startingValue, targetValue, t);
    }
}
