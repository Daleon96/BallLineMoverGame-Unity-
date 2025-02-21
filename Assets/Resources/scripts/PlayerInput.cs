using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMoveSide();

public abstract class PlayerInput
{
    public OnMoveSide onMoveLeft;
    public OnMoveSide onMoveRight;
    public OnMoveSide onMoveTop;
    public OnMoveSide onMoveDown;

    public abstract void StartInput(MonoBehaviour monoBehaviour);
    public abstract void EndInput(MonoBehaviour monoBehaviour);
    
    protected void MoveLeft()
    {
        onMoveLeft?.Invoke();
    }
    protected void MoveRight()
    {
        onMoveRight?.Invoke();
    }
    protected void MoveTop()
    {
        onMoveTop?.Invoke();
    }
    protected void MoveDown()
    {
        onMoveDown?.Invoke();
    }
}
