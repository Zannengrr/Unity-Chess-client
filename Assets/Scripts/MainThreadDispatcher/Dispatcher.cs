using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    public static void RunAsync(Action action)
    {
        ThreadPool.QueueUserWorkItem(o => action());
    }

    public static void RunAsync(Action<object> action, object state)
    {
        ThreadPool.QueueUserWorkItem(o => action(o), state);
    }

    public static void RunOnMainThread(Action action)
    {
        AddActions(action);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    private void Update()
    {
        RunActions();
    }

    private void RunActions()
    {
        if (_queued)
        {
            lock (_backlog)
            {
                _actions.AddRange(_backlog);
                _backlog.Clear();
            }

            foreach (var action in _actions)
            {
                try
                {
                    action();
                }
                catch (Exception err)
                {
                    Debug.LogError($"Dispatcher action failed : \n {err.Message} \n {err.StackTrace}");
                    continue;
                }
            }

            _actions.Clear();

            Debug.Log($"Dispatcher _actions loop clear, actions left {_actions.Count}");
            _queued = false;
        }
    }

    private static void AddActions(Action action)
    {
        lock (_backlog)
        {
            _backlog.Add(action);
            _queued = true;
        }
    }

    static Dispatcher _instance;
    static volatile bool _queued = false;
    static List<Action> _backlog = new List<Action>(20);
    static List<Action> _actions = new List<Action>(20);
}