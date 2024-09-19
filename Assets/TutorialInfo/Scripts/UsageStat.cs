[System.Serializable]
public class UsageStat
{
    // This class creates a datatype that represents each App's usage stat 
    public string PackageName { get; set; }
    public long LastTimeUsed { get; set; }
}
