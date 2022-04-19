using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Interpolator<T>
{
    float fullTime;
    float currentTime;

    T startingValue;
    T targetValue;
    T currentValue;

    bool paused = false;

    protected ICalculatorInterpolation<T> calculatorInterpolation;
    IOnUpdateInterpolation<T> onUpdateListener;

    public Interpolator(T startingValue, T targetValue, float fullTime, ICalculatorInterpolation<T> calculatorInterpolation, IOnUpdateInterpolation<T> onUpdateListener) : this(fullTime, calculatorInterpolation, onUpdateListener)
    {
        this.startingValue = startingValue;
        this.targetValue = targetValue;
    }

    public Interpolator(float fullTime, ICalculatorInterpolation<T> calculatorInterpolation, IOnUpdateInterpolation<T> onUpdateListener)
    {
        this.fullTime = fullTime;
        this.calculatorInterpolation = calculatorInterpolation;
        this.onUpdateListener = onUpdateListener;
        this.currentTime = 0;
        this.paused = true;
    }

    public void update()
    {
        if (!this.paused)
        {
            this.currentTime += Time.deltaTime;
            if (this.currentTime < this.fullTime)
            {
                this.currentValue = this.calculatorInterpolation.calculateInterpolatedValue(this.startingValue, this.targetValue, this.currentTime / this.fullTime); ;
            }
            else
            {
                this.currentValue = this.calculatorInterpolation.calculateInterpolatedValue(this.startingValue, this.targetValue, 1);
                this.paused = true;
            }
            this.onUpdateListener.onUpdateInterpolation(this, this.currentValue);
        }
    }

    public void start()
    {
        this.paused = false;
    }

    public void pause()
    {
        this.paused = true;
    }

    public void reset()
    {
        this.currentTime = 0;
        this.paused = false;
    }

    public void setValues(T startingValue, T targetValue)
    {
        this.startingValue = startingValue;
        this.targetValue = targetValue;
    }

    public void reset(T startingValue, T targetValue)
    {
        setValues(startingValue, targetValue);
        this.currentTime = 0;
        this.paused = false;
    }

    public T getValue()
    {
        return this.currentValue;
    }
}
