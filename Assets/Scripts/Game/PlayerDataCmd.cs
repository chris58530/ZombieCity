using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDataCmd : ICommand
{
    [Inject] private PlayerDataProxy proxy;


    public override void Execute(MonoBehaviour mono)
    {
        LoadPlayerDataFromPrefs();
    }

    public void SavePlayerDataToPrefs(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log("Saving PlayerData to PlayerPrefs: " + json);
        PlayerPrefs.SetString("PlayerDataJson", json);
        PlayerPrefs.Save();
    }

    public void LoadPlayerDataFromPrefs()
    {
        if (PlayerPrefs.HasKey("PlayerDataJson"))
        {
            string json = PlayerPrefs.GetString("PlayerDataJson");
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            proxy.SetData(data);
            Debug.Log("Player data loaded from PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No PlayerPrefs data found for PlayerDataJson. Creating new PlayerData with default values.");
            CreateNewPlayerDataAndSave();
        }
        SetComplete();
    }


    public void CreateNewPlayerDataAndSave()
    {
        PlayerData data = new PlayerData
        {
            resourceInfo = new ResourceInfoData
            {
                moneyAmount = 1000,
                satisfactionAmount = 87
            },
            floorProduct = new FloorProductData
            {
                floor_01 = 0,
                floor_02 = 0
            }
        };

        SavePlayerDataToPrefs(data);
    }
}

public class PlayerData
{
    public ResourceInfoData resourceInfo;
    public FloorProductData floorProduct;
}

public class ResourceInfoData
{
    public int moneyAmount;
    public int satisfactionAmount;
}

public class FloorProductData
{
    public int floor_01;
    public int floor_02;

}
