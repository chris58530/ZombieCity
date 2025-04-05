using UnityEngine;

public class LogService : Singleton<LogService>
{
    public void Log(string message)
    {
        Debug.Log(message);
    }
}
