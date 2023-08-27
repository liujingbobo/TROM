using UnityEngine;

public class Countdown
{
    public float countdownTime = 1f;  // Duration of countdown in seconds
    private float startTime;

    public Countdown(float countdownTime)
    {
        // Register the start time when the component is enabled
        this.countdownTime = countdownTime;
    }

    public void SetCountdownTime(float time)
    {
        this.countdownTime = time;
    }
    
    public bool IsCountdownOver()
    {
        // Return whether the current time is greater than the start time plus the countdown duration
        return Time.time >= startTime + countdownTime;
    }

    public void Flush()
    {
        startTime = Time.time;
    }

    public bool IsCountdownOverThenFlush()
    {
        if (Time.time >= startTime + countdownTime)
        {
            Flush();
            return true;
        }
        return false;
    }

    public float TimeRatio
    {
        get => (Time.time - startTime) / countdownTime;
    }

    public float TimeLeftRatio
    {
        get => 1 - TimeRatio;
    }
}
