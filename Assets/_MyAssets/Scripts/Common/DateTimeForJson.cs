using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// system.datetimeはJsonUtilityでパースできない
/// </summary>
[System.Serializable]
public class DateTimeForJson
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;

    public DateTimeForJson(DateTime dt)
    {
        DateTime = dt;
    }

    public DateTime DateTime
    {
        set
        {
            year = value.Year;
            month = value.Month;
            day = value.Day;
            hour = value.Hour;
            minute = value.Minute;
            second = value.Second;
        }
        get
        {
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
