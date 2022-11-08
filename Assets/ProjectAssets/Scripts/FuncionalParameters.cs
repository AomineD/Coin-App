using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ApiManager;

public static class FuncionalParameters
{
    // THIS FUNCTIONS ARE EXTENSIBLE FUNCTIONS FOR BETTER READABLE CODE
    // 
  public static TMP_Text getButtonText(this Button button)
    {
        return button.GetComponentInChildren<TMP_Text>();
    }
  public static float toSecondsFloat(this DateTime dateTimeFromDB)
    {
       long millisUtc = DateTime.UtcNow.GetCurrentMillis();
        long millisFrom = dateTimeFromDB.GetCurrentMillis();
        return (millisFrom - millisUtc) / 1000; // Divide in 1000 to transform milliseconds in seconds
    }

    public static string timeLeftText(this float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        return string.Format("{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
    }

    public static long GetCurrentMillis(this DateTime time)
    {
        DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSpan = time - Jan1970; // calculate milliseconds from 1970, January to current milliseconds datetime and get difference result
        return (long)timeSpan.TotalMilliseconds;
    }

    public static bool isReadyForGetCoins(this User user)
    {
        long millisUtc = DateTime.UtcNow.GetCurrentMillis();
        long millisFrom = user.getTimeForClaim().GetCurrentMillis();
        return millisFrom - millisUtc < 0;
    }

}
