using System;

// AppUsageInfo class to hold app name and usage stats
[Serializable]
public class UsageStat
{
    public string PackageName { get; set; }

    public long LastTimeUsed { get; set; }
    public long TotalTimeInForeground { get; set; }
}
