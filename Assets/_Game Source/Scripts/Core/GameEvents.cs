using System;
using UnityEngine;

#region Custom Events Generics

public class CustomEvents
{
    private event Action _action = delegate { };

    public void Invoke()
    {
        _action?.Invoke();
    }

    public void AddListener(Action listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action listener)
    {
        _action -= listener;
    }
}

public class CustomEvents<T>
{
    private event Action<T> _action = delegate { };

    public void Invoke(T param)
    {
        _action?.Invoke(param);
    }

    public void AddListener(Action<T> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        _action -= listener;
    }
}

public class CustomEvents<T1, T2>
{
    private event Action<T1, T2> _action = delegate { };

    public void Invoke(T1 param1, T2 param2)
    {
        _action?.Invoke(param1, param2);
    }

    public void AddListener(Action<T1, T2> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T1, T2> listener)
    {
        _action -= listener;
    }
}

public class CustomEvents<T1, T2, T3>
{
    private event Action<T1, T2, T3> _action = delegate { };

    public void Invoke(T1 param1, T2 param2, T3 param3)
    {
        _action?.Invoke(param1, param2, param3);
    }

    public void AddListener(Action<T1, T2, T3> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T1, T2, T3> listener)
    {
        _action -= listener;
    }
}

public class CustomEvents<T1, T2, T3, T4>
{
    private event Action<T1, T2, T3, T4> _action = delegate { };

    public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4)
    {
        _action?.Invoke(param1, param2, param3, param4);
    }

    public void AddListener(Action<T1, T2, T3, T4> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T1, T2, T3, T4> listener)
    {
        _action -= listener;
    }
}

public class CustomEvents<T1, T2, T3, T4, T5>
{
    private event Action<T1, T2, T3, T4, T5> _action = delegate { };

    public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        _action?.Invoke(param1, param2, param3, param4, param5);
    }

    public void AddListener(Action<T1, T2, T3, T4, T5> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T1, T2, T3, T4, T5> listener)
    {
        _action -= listener;
    }
}

#endregion

public class GameEvents
{
    
    public static readonly CustomEvents<string, bool, Action> ChangeScreenTo = new();
    public static readonly CustomEvents Universal_BackButton = new();
    public static readonly CustomEvents<string, float , float> PlaySFX = new();
    
    public static readonly CustomEvents<GameViewMode> OnModeChanged = new();
    
    

}