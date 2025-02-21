using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayerInput : PlayerInput
{
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private float _swipeThreshold = 250f; 

    private bool dragTouchStart = false;
    private Coroutine detectDragCoroutine;
    
    public override void StartInput(MonoBehaviour monoBehaviour)
    {
        if (detectDragCoroutine != null)
        {
            monoBehaviour.StopCoroutine(detectDragCoroutine);
        }

        detectDragCoroutine = monoBehaviour.StartCoroutine(DetectDrag());

    }

    public override void EndInput(MonoBehaviour monoBehaviour)
    {
        if (detectDragCoroutine != null)
        {
            monoBehaviour.StopCoroutine(detectDragCoroutine);
        }

        detectDragCoroutine = null;
    }

    IEnumerator DetectDrag()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _startTouchPosition = touch.position;
                    dragTouchStart = true;
                }
                else if (touch.phase == TouchPhase.Moved && dragTouchStart)
                {
                    _endTouchPosition = touch.position;
                    DetectSwipe();
                }
            }

            yield return null;
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeVector = _endTouchPosition - _startTouchPosition;
        if (swipeVector.magnitude < _swipeThreshold)
            return; 

        dragTouchStart = false;
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0)
                MoveRight();
            else
                MoveLeft();
        }
        else
        {
            if (swipeVector.y < 0)
                MoveTop();
            else
                MoveDown();
        }
    }
    
}
