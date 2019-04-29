using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera Camera;
    public LayerMask TouchMask;
    private ITouchable _oldTouch;
    private ITouchable _currentTouch;
    private RaycastHit _hit;
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            _oldTouch = _currentTouch;
            _currentTouch = null;

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit, TouchMask))
            {
                Debug.Log(_hit.transform.gameObject.name, _hit.transform.gameObject);
                ITouchable current = _hit.transform.gameObject.GetComponent<ITouchable>();
                _currentTouch = current;
                if (Input.GetMouseButtonDown(0))
                {
                    current.OnTouchDown();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    current.OnTouchUp();
                }
                if (Input.GetMouseButton(0))
                {
                    current.OnTouchDown();
                }
            }
            
            if (_oldTouch != null & _oldTouch != _currentTouch)
            {
                _oldTouch.OnTouchUp();
            }
        }

#endif
        if (Input.touchCount > 0)
        {
            Touch t = Input.touches[0];
            Ray ray = Camera.ScreenPointToRay(t.position);
            RaycastHit hit;

            _oldTouch = _currentTouch;
            _currentTouch = null;

            if (Physics.Raycast(ray, out hit, TouchMask))
            {
                ITouchable current = _hit.transform.gameObject.GetComponent<ITouchable>();
                _currentTouch = current;
                if (t.phase == TouchPhase.Began)
                {
                    current.OnTouchDown();
                }
                if (t.phase == TouchPhase.Ended)
                {
                    current.OnTouchUp();
                }
                if (t.phase == TouchPhase.Stationary)
                {
                    current.OnTouchDown();
                }
                if (t.phase == TouchPhase.Moved)
                {
                    current.OnTouchDown();
                }
                if (t.phase == TouchPhase.Canceled)
                {
                    current.OnTouchUp();
                }
            }

            if (_oldTouch != null & _oldTouch != _currentTouch)
            {
                _oldTouch.OnTouchUp();
            }
        }
        
    }
}
