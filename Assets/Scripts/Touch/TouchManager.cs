using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Camera Camera;
    public LayerMask TouchMask;
    private ITouchable _oldTouch;
    private ITouchable _currentTouch;
    private RaycastHit _hit;

    public Text CountText;

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
            
            if (_oldTouch != null && _oldTouch != _currentTouch)
            {
                _oldTouch.OnTouchUp();
            }
        }

#endif
        CountText.text = Input.touchCount.ToString();
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Ray ray = Camera.ScreenPointToRay(t.position);

            _oldTouch = _currentTouch;
            _currentTouch = null;

            if (Physics.Raycast(ray, out _hit, TouchMask))
            {
                ITouchable current = _hit.transform.gameObject.GetComponent<ITouchable>();
                _currentTouch = current;

                if(current != null)
                {
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
                
            }

            if (_oldTouch != null && _oldTouch != _currentTouch)
            {
                _oldTouch.OnTouchUp();
            }
        }
        
    }
}
