using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;

public class PlayerDataCmd : ICommand
{
    [Inject] private PlayerDataProxy proxy;
    [Inject] private ResourceInfoProxy resourceInfoProxy;


    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        LoadPlayerDataFromPrefs();
    }
    [Listener(PlayerDataEvent.ON_UPDATE_PLAYER_DATA)]
    public void UpdateAndSave()
    {
        PlayerData data = proxy.playerData;
        data.resourceInfoData = resourceInfoProxy.resourceInfoData;
        SavePlayerDataToPrefs(data);
    }

    public void SavePlayerDataToPrefs(PlayerData data)
    {
        string json = JsonConvert.SerializeObject(data);
        string AESJson = AESSerice.EncryptAES(json);
        Debug.Log("Saving PlayerData to PlayerPrefs: " + json);
        PlayerPrefs.SetString("PlayerDataJson", AESJson);
        PlayerPrefs.Save();
    }

    public void LoadPlayerDataFromPrefs()
    {
        if (PlayerPrefs.HasKey("PlayerDataJson"))
        {
            string AESJson = PlayerPrefs.GetString("PlayerDataJson");
            string json = AESSerice.DecryptAES(AESJson);

            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            proxy.SetData(data);
            Debug.Log("Player data loaded from PlayerPrefs. Data: " + json);
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
                moneyAmount = 100,
                satisfactionAmount = 87
            },
            // floorProductData = new FloorProductData
            // {
            //     entries = new Dictionary<int, int>
            //     {
            //         { 1, 0 },
            //         { 2, 0 },
            //         { 3, 0 },
            //         { 4, 0 },
            //         { 5, 0 }
            //     }
            // }
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
  
    // public FloorProductData floorProductData;
}

[System.Serializable]
public class ResourceInfoData
{
    public int moneyAmount;
    public int satisfactionAmount;

}

[System.Serializable]
public class FloorProductData
{
    public Dictionary<int, int> entries = new Dictionary<int, int>();
}
