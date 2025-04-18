using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;
using System;

public class PlayerDataCmd : ICommand
{
    [Inject] private PlayerDataProxy proxy;
    [Inject] private ResourceInfoProxy resourceInfoProxy;
    [Inject] private FloorProxy floorProxy;

    [SerializeField] private const string SaveString = "PlayerDataJson";
    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        LoadPlayerDataFromPrefs();
    }
    [Listener(PlayerDataEvent.ON_UPDATE_PLAYER_DATA)]
    public void UpdateAndSave()
    {
        PlayerData data = proxy.playerData;
        data.logOutData.lastLogoutTime = System.DateTime.UtcNow.ToString("o");
        Debug.Log("儲存玩家當前時間 ： " + data.logOutData.lastLogoutTime);
        if (resourceInfoProxy.resourceInfoData != null)
        {
            data.resourceInfoData = resourceInfoProxy.resourceInfoData;
        }
        if (floorProxy.floorProductData != null)
        {
            data.floorProductData = floorProxy.floorProductData;
        }
        SavePlayerDataToPrefs(data);
    }

    public void SavePlayerDataToPrefs(PlayerData data)
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

            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
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
        PlayerData data = new PlayerData
        {
            resourceInfoData = new ResourceInfoData
            {

            },
            floorProductData = new FloorProductData
            {
                FloorProduct = new Dictionary<int, int>
                {

                }
            },
            logOutData = new LogOutData
            {

            }
        };

        SavePlayerDataToPrefs(data);
        proxy.SetData(data);
        SetComplete(); // 加入這裡
    }
}

[System.Serializable]
public class PlayerData
{
    public ResourceInfoData resourceInfoData;

    public FloorProductData floorProductData;
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
    public bool isLock = true;
}

[System.Serializable]
public class FloorProductData
{
    public Dictionary<int, int> FloorProduct = new();
    //Floor ID , ProductAmount  e.g.(901,999)、(902,878)
    public Dictionary<int, List<FacilityWorkData>> FloorFacility = new();
    //Floor ID , FacilityData  
}
public class FacilityWorkData
{
    public string animationString;
    public bool isUsing;
    public int efficientTime;
    public int startTime;
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
