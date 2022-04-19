public interface ICalculatorInterpolation<T>
{
    T calculateInterpolatedValue(T startingValue, T targetValue, float t);
}