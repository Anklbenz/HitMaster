using System;
using UnityEngine;

public class InputReceiver
{
    public event Action<Vector3, float> TapStartEvent, TapEndEvent, MoveEvent;

    private bool _isActive = true;

    public void Update(){
        if (!_isActive) return;

        if(Input.GetMouseButtonDown(0))
            TapStartEvent?.Invoke(Input.mousePosition, 0);  
        
        
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        Tap(touch);
    }

    private void Tap(Touch touch){
        var touchPosition = touch.position;
        var time = Time.time;
        
        if (touch.phase == TouchPhase.Began)
            TapStartEvent?.Invoke(touchPosition, time);

        if (touch.phase == TouchPhase.Moved)
            MoveEvent?.Invoke(touchPosition, time);

        if (touch.phase == TouchPhase.Ended)
            TapEndEvent?.Invoke(touchPosition, time);
    }

    public void SetActive(bool state) => _isActive = state;
}
