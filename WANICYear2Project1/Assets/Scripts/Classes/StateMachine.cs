/*111111111111111111111111111111111111111111111111111111111
 * 
 * Name        : Carson Lakefish
 * Date        : 9 / 14 / 2023
 * Description : Main State Machine
 111111111111111111111111111111111111111111111111111111111*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public bool Debug = false;
    internal State currentState = null;
    internal State previousState = null;
    internal float stateDuration = 0;

    protected virtual State Initialize() { return null; }

    public void Start() => currentState = Initialize();

    public void Update()
    {
        if (currentState != null) currentState.Update();

        stateDuration += Time.deltaTime;
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.FixedUpdate();
    }

    public void ChangeState(State newState)
    {
        if (currentState != null) currentState.Exit();

        stateDuration = 0;
        previousState = currentState;
        currentState = newState;

        currentState.Enter();
    }

    private void OnGUI()
    {
        if (!Debug) return;

        GUILayout.BeginArea(new Rect(10, 10, 200, 200));

        string current  = "Current State : " + $"<color='red'>{(currentState == null ? "None" : currentState)}</color>",
               previous = "Previous State : " + $"<color='red'>{(previousState == null ? "None" : previousState)}</color>";

        GUILayout.Label($"<size=15>{current} </size>");
        GUILayout.Label($"<size=15>{previous}</size>");

        GUILayout.Label($"<size=18>{stateDuration}</size>");

        GUILayout.EndArea();
    }
}

[System.Serializable]
public class State
{
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
