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
        if (floorProxy.floorProductData != null)
        {
            data.floorInfoData = floorProxy.floorProductData;
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
            resourceInfoData = new()
            {

            },
            floorInfoData = new Dictionary<int, FloorInfoData>()
            {
                { 901, new FloorInfoData { productAmount = 0, level = 1, facilityData = new Dictionary<int, FacilityData>() {
                    { 0, new FacilityData { order = 0, animationString = "Idle", isUsing = false, efficientTime = 0, startTime = 0 } },
                    { 1, new FacilityData { order = 1, animationString = "Idle", isUsing = false, efficientTime = 0, startTime = 0 } },
                    { 2, new FacilityData { order = 2, animationString = "Idle", isUsing = false, efficientTime = 0, startTime = 0 } },
                    { 3, new FacilityData { order = 3, animationString = "Idle", isUsing = false, efficientTime = 0, startTime = 0 } }
                }} },
                { 902, new FloorInfoData { productAmount = 0, level = 1, facilityData = new Dictionary<int, FacilityData>() } },
                { 903, new FloorInfoData { productAmount = 0, level = 1, facilityData = new Dictionary<int, FacilityData>() } },
                { 904, new FloorInfoData { productAmount = 0, level = 1, facilityData = new Dictionary<int, FacilityData>() } }
            },
            workingSurvivorData = new Dictionary<int, bool>()
            {
              {101,false},
              {102,false},
              {103,false},
              {104,false},
              {105,false},
              {106,false},
              {107,false},
              {108,false},
              {109,false},
              {110,false}
            },
            logOutData = new()
            {

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
    public ResourceInfoData resourceInfoData;

    public Dictionary<int, FloorInfoData> floorInfoData;
    public Dictionary<int, bool> workingSurvivorData;
    public LogOutData logOutData;
}
[System.Serializable]
public class LogOutData
{
    public string lastLogoutTime;
    public double logOutTime;

}

[System.Serializable]
public class ResourceInfoData
{
    public int moneyAmount;
    public int satisfactionAmount;
    public int gemAmount;
}

[System.Serializable]
public class FloorInfoData
{
    public int productAmount;
    public int level;
    public Dictionary<int, FacilityData> facilityData;
}
public class FacilityData
{
    public int order;
    public string animationString;
    public bool isUsing;
    public int efficientTime;
    public int startTime;
    public int usingSurvivor;

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
