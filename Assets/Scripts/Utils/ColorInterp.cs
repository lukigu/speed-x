using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInterp : Interpolator<Color>, ICalculatorInterpolation<Color>
{
    public ColorInterp(float fullTime, IOnUpdateInterpolation<Color> onUpdateListener) : base(fullTime, null, onUpdateListener)
    {
        this.calculatorInterpolation = this;
    }

    public ColorInterp(Color startingValue, Color targetValue, float fullTime, IOnUpdateInterpolation<Color> onUpdateListener) : base(startingValue, targetValue, fullTime, null, onUpdateListener)
    {
        this.calculatorInterpolation = this;
    }

    public Color calculateInterpolatedValue(Color startingValue, Color targetValue, float t)
    {
        return Color.Lerp(startingValue, targetValue, t);
    }
}
