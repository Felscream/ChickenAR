using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    public Color DefaultColor;
    public Color Selected;

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void OnTouchDown()
    {
        mat.SetColor("_Color", Selected);
    }

    private void OnTouchUp()
    {
        mat.SetColor("_Color", DefaultColor);
    }

    private void OnTouchStay()
    {
        mat.SetColor("_Color", Selected);
    }

    private void OnTouchMoved()
    {
        mat.SetColor("_Color", Selected);
    }

    private void OnTouchExit()
    {
        mat.SetColor("_Color", Selected);
    }
}
