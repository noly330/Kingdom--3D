using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
    [SerializeField] private int _initMaxTimerCount;
    private Queue<GameTimer> _notWorkerTimer = new Queue<GameTimer>();
    private List<GameTimer> _workingTimer = new List<GameTimer>();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initTimerManager();
    }

    private void Update()
    {
        UpdateWorkingTimer();

    }


    private void initTimerManager()
    {
        for (int i = 0; i < _initMaxTimerCount; i++)
        {
            CreatTimer();
        }
    }

    private void CreatTimer()
    {
        GameTimer timer = new GameTimer();
        _notWorkerTimer.Enqueue(timer);
    }

    public void TryGetOneTimer(float time, Action task)
    {
        if (_notWorkerTimer.Count == 0)
        {
            CreatTimer();
        }
        var timer = _notWorkerTimer.Dequeue();
        timer.StartTimer(time, task);
        _workingTimer.Add(timer);
    }
    private void UpdateWorkingTimer()
    {
        if (_workingTimer.Count == 0) return;
        for(int i =0;i<_workingTimer.Count;i++)
        {
            if(_workingTimer[i].GetTimerState() == TimeState.Working)
            {
                _workingTimer[i].UpdateTimer();
            }
            else
            {
                _notWorkerTimer.Enqueue(_workingTimer[i]);
                _workingTimer[i].ResetTimer();
                _workingTimer.Remove(_workingTimer[i]);
            }
        }
    }
}

public enum TimeState
{
    NotWork, Working, Done
}

public class GameTimer
{
    private float _startTime;
    private Action _task;
    private bool _isStopTimer;
    private TimeState _timeState;
    public GameTimer()
    {
        ResetTimer();
    }

    //开始计时
    public void StartTimer(float time, Action task)
    {
        _startTime = time;
        _task = task;
        _isStopTimer = false;
        _timeState = TimeState.Working;
    }

    //更新计时器
    public void UpdateTimer()
    {
        if (_isStopTimer) return;
        _startTime -= Time.deltaTime;
        if (_startTime <= 0f)
        {
            _timeState = TimeState.Done;
            _task?.Invoke();
            _isStopTimer = true;
        }
    }

    public TimeState GetTimerState() => _timeState;

    public void ResetTimer()
    {
        _startTime = 0f;
        _task = null;
        _isStopTimer = false;
        _timeState = TimeState.NotWork;
    }
}
