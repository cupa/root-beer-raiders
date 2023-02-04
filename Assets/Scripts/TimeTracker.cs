using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TimeTracker
{
    private float startTime;
    private float seconds;
    private bool hasStartTimeBeenSet;
    private bool hasFinished;

    public TimeTracker(float seconds)
    {
        this.seconds = seconds;
        hasStartTimeBeenSet = false;
        hasFinished = false;
    }

    public bool HasTimePassed()
    {
        if (hasFinished)
        {
            return false;
        }
        if (!hasStartTimeBeenSet)
        {
            startTime = Time.time;
            hasStartTimeBeenSet = true;
        }

        if (hasStartTimeBeenSet && Time.time - startTime >= seconds)
        {
            hasFinished = true;
            return true;
        }

        return false;
    }

    public void RestartTimer(float seconds)
    {
        this.seconds = seconds;
        hasStartTimeBeenSet = false;
        hasFinished = false;
    }
}