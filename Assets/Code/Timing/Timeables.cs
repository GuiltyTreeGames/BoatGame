using System;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Performs an event after a certain time period.
/// </summary>
public class Countdown : ITimeable
{
    /// <summary>
    /// <see cref="Countdown"/>
    /// </summary>
    /// <param name="length">The time (in seconds) before the action is performed</param>
    /// <param name="onFinish">The action to be performed</param>
    public Countdown(float length, Action onFinish)
    {
        _onFinish = onFinish;
        _finishTime = Time.time + length;
    }

    private readonly Action _onFinish;
    private readonly float _finishTime;

    public bool ShouldBeRemoved { get; private set; } = false;
    public bool IsPermanent => false;

    public void OnUpdate()
    {
        if (Time.time < _finishTime)
            return;

        ShouldBeRemoved = true;
        _onFinish();
    }
}


/// <summary>
/// Performs an event after a random delay, uniformly distributed between min and max.
/// </summary>
public class RandomCountdown : ITimeable
{
    /// <summary>
    /// <see cref="RandomCountdown"/>
    /// </summary>
    /// <param name="timeBounds">The possible min and max of the delay</param>
    /// <param name="onFinish">The action performed when the timer ends</param>
    public RandomCountdown(Vector2 timeBounds, Action onFinish)
    {
        _onFinish = onFinish;
        _finishTime = Time.time + UnityEngine.Random.Range(timeBounds.x, timeBounds.y);
    }

    private readonly Action _onFinish;
    private readonly float _finishTime;

    public bool ShouldBeRemoved { get; private set; } = false;
    public bool IsPermanent => false;

    public void OnUpdate()
    {
        if (Time.time < _finishTime)
            return;

        ShouldBeRemoved = true;
        _onFinish();
    }
}

/// <summary>
/// Repeatedly perform an event on a regular interval.
/// </summary>
public class Ticker : ITimeable
{
    /// <summary>
    /// <see cref="Ticker"/>
    /// </summary>
    /// <param name="length">The interval (in seconds) between each execution of the action</param>
    /// <param name="permanent"></param>
    /// <param name="onTick">The action performed when the timer ends</param>
    public Ticker(float length, bool permanent, Action onTick)
    {
        _onTick = onTick;
        _length = length;
        _nextActivation = Time.time + length;
        _isPermanent = permanent;
    }

    private readonly Action _onTick;
    private readonly float _length;

    private bool _isPermanent;
    private float _nextActivation;

    public bool ShouldBeRemoved => false;
    public bool IsPermanent => _isPermanent;

    public void OnUpdate()
    {
        if (Time.time < _nextActivation)
            return;

        _nextActivation = Time.time + _length;
        _onTick();
    }
}

/// <summary>
/// Repeatedly perform an event on a random interval, uniformly distributed between the given time bounds.
/// </summary>
public class RandomTicker : ITimeable
{
    /// <summary>
    /// <see cref="RandomTicker"/>
    /// </summary>
    /// <param name="timeBounds">The possible min and max of the interval at which the event may occur</param>
    /// <param name="permanent"></param>
    /// <param name="onTick"></param>
    public RandomTicker(Vector2 timeBounds, bool permanent, Action onTick)
    {
        _onTick = onTick;
        _isPermanent = permanent;
        _minTime = timeBounds.x;
        _maxTime = timeBounds.y;
        if (timeBounds.x > timeBounds.y) // ensures `_minTime` is smaller than `_maxTime`
        {
            _minTime = timeBounds.y;
            _maxTime = timeBounds.x;
        }
        UpdateNextActivation();
    }

    private readonly Action _onTick;
    private readonly float _minTime;
    private readonly float _maxTime;

    private bool _isPermanent;
    private float _length;
    private float _nextActivation;

    public bool ShouldBeRemoved => false;
    public bool IsPermanent => _isPermanent;

    public void OnUpdate()
    {
        if (Time.time < _nextActivation)
            return;

        UpdateNextActivation();
        _onTick();
    }

    private void UpdateNextActivation()
    {
        _length = UnityEngine.Random.Range(_minTime, _maxTime);
        _nextActivation = Time.time + _length;
    }
}

public interface ITimeable
{
    /// <summary>
    /// Tag that indicates an object should be removed by `TimeHandler` on next frame
    /// </summary>
    bool ShouldBeRemoved { get; }

    /// <summary>
    /// Tag that prevent the timer from removal when `TimeHandler` resets.
    /// </summary>
    bool IsPermanent { get; }

    void OnUpdate();
}