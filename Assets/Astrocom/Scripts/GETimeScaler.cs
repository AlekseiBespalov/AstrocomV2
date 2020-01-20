using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GETimeScaler : MonoBehaviour
{
    private float[] timeZoomSteps = new float[] { 1f, 1f, 2f, 8f, 32f, 1024f, 1000000f, 6000000f };

    public int CurrentTimeZoomStep
    { 
        get => _currentTimeZoomStep; 
        set => _currentTimeZoomStep = Mathf.Clamp(value, 1, timeZoomSteps.Length - 1); 
    }

    public TextMeshProUGUI Text;
    private int _currentTimeZoomStep = 1;

    private void FixedUpdate()
    {
        Text.SetText($"x{timeZoomSteps[_currentTimeZoomStep]}");
    }

    public void SpeedUp()
    {
        CurrentTimeZoomStep += 1;
        GravityEngine.Instance().SetTimeZoom(timeZoomSteps[CurrentTimeZoomStep]);
    }

    public void SpeedDown()
    {
        CurrentTimeZoomStep -= 1;
        GravityEngine.Instance().SetTimeZoom(timeZoomSteps[CurrentTimeZoomStep]);
    }
}