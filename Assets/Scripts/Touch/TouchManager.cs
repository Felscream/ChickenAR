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
    
    public Component CurrentTouchable { get; set; }

    private RaycastHit _hit;
    private static TouchManager _instance;

    public Text CountText;
    public static TouchManager Instance {
        get {
            if(_instance == null)
            {
                Debug.LogError("No instance of TouchManager");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            _oldTouch = _currentTouch;
            _currentTouch = null;
            CurrentTouchable = null;

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
                current.Register(this);
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
            CurrentTouchable = null;

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
                    current.Register(this);
                }
                
            }

            if (_oldTouch != null && _oldTouch != _currentTouch)
            {
                _oldTouch.OnTouchUp();
            }
        }
        
    }
}
