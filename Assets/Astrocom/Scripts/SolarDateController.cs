using FantomLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolarDateController : MonoBehaviour {

    //public InputField yearText;
    //public InputField monthText;
    //public InputField dayText;
    public TextMeshProUGUI CurrentDate;
    public GameObject SolarSystemControlPanel;

    //public Text currentTime;

    public SolarSystem solarSystem;

    private bool clearTrail;
    private int clearTrailDelay; 

    void Start() {
        //yearText.text = "2016";
        //monthText.text = "6";
        //dayText.text = "1";
    }

    
    private void FixedUpdate()
    {
        if(solarSystem == null)
            solarSystem = (SolarSystem)FindObjectOfType(typeof(SolarSystem));
    }

    public void SetPhysTime(string dateTime) {
        //int year = int.Parse(yearText.text);
        //int month = int.Parse(monthText.text);
        //int day = int.Parse(dayText.text);
        DateTime TempDate = DateTime.ParseExact(dateTime, "yyyy/M/d", CultureInfo.InvariantCulture);
        int year = TempDate.Year;
        int month = TempDate.Month;
        int day = TempDate.Day;

        solarSystem.SetTime(year, month, day, 0);
        clearTrail = true;
        clearTrailDelay = 3;
    }

    private void ResetTrails() {
        foreach (TrailRenderer tr in (TrailRenderer[])FindObjectsOfType(typeof(TrailRenderer))) {
            tr.Clear();
        }
    }

    void Update() {
        // space toggles evolution
        if (Input.GetKeyDown(KeyCode.Space)) {
            GravityEngine.Instance().SetEvolve(!GravityEngine.Instance().GetEvolve());
        }
        DateTime newTime = SolarUtils.DateForEpoch(solarSystem.GetStartEpochTime());
        newTime += GravityScaler.GetTimeSpan(GravityEngine.Instance().GetPhysicalTimeDouble(), GravityScaler.Units.SOLAR);
        //currentTime.text =  newTime.ToString("yyyy:MM:dd"); // 24h format
        //currentTime.text = newTime.ToString("yyyy:MM:dd HH:mm:ss");
        CurrentDate.SetText(newTime.ToString("yyyy:MM:dd HH:mm:ss"));

        // ICK! Need to wait until GE moves these objects before can clear the trail
        if (clearTrail) {
            if (clearTrailDelay-- <= 0) {
                ResetTrails();
                clearTrail = false;
            }
        }
    }
}
