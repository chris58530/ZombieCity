using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;
using System;

public class JsonDataCmd : ICommand
{
    [Inject] private JsonDataProxy proxy;
    [Inject] private ResourceInfoProxy resourceInfoProxy;
    [Inject] private FloorProxy floorProxy;

    [SerializeField] private const string SaveString = "PlayerDataJson";
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        LoadPlayerDataFromPrefs();
    }
    [Listener(JsonDataEvent.ON_UPDATE_PLAYER_DATA)]
    public void UpdateAndSave()
    {
        JsonData data = proxy.jsonData;
        data.logOutData.lastLogoutTime = System.DateTime.UtcNow.ToString("o");
        // Debug.Log("儲存玩家當前時間 ： " + data.logOutData.lastLogoutTime);
        if (resourceInfoProxy.resourceInfoData != null)
        {
            data.resourceInfoData = resourceInfoProxy.resourceInfoData;
        }
        if (floorProxy.floorInfoData != null)
        {
            data.floorInfoData = floorProxy.floorInfoData;
        }
        SavePlayerDataToPrefs(data);
    }

    public void SavePlayerDataToPrefs(JsonData data)
    {
        string json = JsonConvert.SerializeObject(data);
        string AESJson = AESSerice.EncryptAES(json);
        Debug.Log("Saving PlayerData to PlayerPrefs: " + json);
        PlayerPrefs.SetString(SaveString, AESJson);
        PlayerPrefs.Save();
    }

    public void LoadPlayerDataFromPrefs()
    {
        if (PlayerPrefs.HasKey(SaveString))
        {
            string AESJson = PlayerPrefs.GetString(SaveString);
            string json = AESSerice.DecryptAES(AESJson);

            JsonData data = JsonConvert.DeserializeObject<JsonData>(json);
            double logoutTime = OfflineTimeService.GetOfflineSeconds(data.logOutData.lastLogoutTime);
            data.logOutData.logOutTime = logoutTime;
            proxy.SetData(data);
            Debug.Log("玩家離開總秒數 ： " + logoutTime + "秒");
            SetComplete(); // ← 等讀取完成再呼叫
        }
        else
        {
            Debug.LogWarning("No PlayerPrefs data found for PlayerDataJson. Creating new PlayerData with default values.");
            CreateNewPlayerDataAndSave(); // fallback 中 SetComplete 也會呼叫
        }
    }

    public void CreateNewPlayerDataAndSave()
    {
        JsonData data = new()
        {
            resourceInfoData = new ResourceJsonData
            {
                moneyAmount = 0,
                satisfactionAmount = 0,
                zombieCoreAmount = 0
            },
            floorInfoData = new Dictionary<int, FloorJsonData>
            {
                { 901, new FloorJsonData { level = 1, productAmount = 0 } },
                { 902, new FloorJsonData { level = 1, productAmount = 0 } },
                { 903, new FloorJsonData { level = 1, productAmount = 0 } }
            },
            survivorInfoData = new Dictionary<int, SurvivorJsonData>
            {
                { 101, new SurvivorJsonData { level = 1, stayingFloor = 901 } },
                { 102, new SurvivorJsonData { level = 1, stayingFloor = 902 } },
                { 103, new SurvivorJsonData { level = 1, stayingFloor = 902 } }
            },
            logOutData = new LogOutData
            {
                lastLogoutTime = DateTime.UtcNow.ToString("o"),
                logOutTime = 0
            }
        };

        SavePlayerDataToPrefs(data);
        proxy.SetData(data);
        SetComplete(); // 加入這裡
    }
}

[System.Serializable]
public class JsonData
{
    public ResourceJsonData resourceInfoData;

    public Dictionary<int, FloorJsonData> floorInfoData;
    public Dictionary<int, SurvivorJsonData> survivorInfoData;
    public LogOutData logOutData;
}
[System.Serializable]
public class LogOutData
{
    public string lastLogoutTime;
    public double logOutTime;

}

[System.Serializable]
public class ResourceJsonData
{
    public int moneyAmount;//瓶蓋(金錢)
    public int satisfactionAmount;//滿意度(玩家等級)
    public int zombieCoreAmount; //屍核
}

[System.Serializable]
public class FloorJsonData
{
    public int level;
    public int productAmount;
}
[System.Serializable]
public class SurvivorJsonData
{
    public int level;
    public int stayingFloor;
}
public static class OfflineTimeService
{
    /// <summary>
    /// 根據登出時間與現在時間，回傳離線秒數
    /// </summary>
    /// <param name="logoutTimeStr">儲存的登出時間字串 (ISO 格式)</param>
    /// <param name="loginTime">登入時間，預設為 DateTime.UtcNow</param>
    /// <returns>離線秒數</returns>
    public static double GetOfflineSeconds(string logoutTimeStr, System.DateTime? loginTime = null)
    {
        if (string.IsNullOrEmpty(logoutTimeStr))
            return 0;

        if (!System.DateTime.TryParse(logoutTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out System.DateTime logoutTime))
            return 0;

        System.DateTime now = loginTime ?? System.DateTime.UtcNow;
        System.TimeSpan diff = now - logoutTime;
        return System.Math.Max(diff.TotalSeconds, 0);
    }
}
