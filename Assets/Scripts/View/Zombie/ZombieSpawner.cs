using UnityEngine;
using DG.Tweening;
using TMPro;
public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;     // 生成位置
    public float spawnInterval = 2f; // 每隔幾秒生成一隻
    public float moveSpeed = 5f;     // 移動速度
    public TMP_Text zombieCounter;   // 顯示殭屍數量的 TMP_Text
    public int zombieCount = 0;     // 當前殭屍數量


    void Start()
    {
        InvokeRepeating(nameof(SpawnZombie), 0f, spawnInterval);
        ReduceZombieCount();
    }

    void SpawnZombie()
    {
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        float leftBound = Random.Range(-0.4f, -0.6f); // 隨機生成左邊界
        // 讓殭屍移動到左邊
        zombie.transform.DOMoveX(leftBound, moveSpeed)
            .SetSpeedBased(true) // 讓它以固定速度移動
            .OnComplete(() =>
            {
                ReduceZombieCount();
                Destroy(zombie);}
                ); // 移動完成後刪除
    }
    void ReduceZombieCount()
    {
        if (zombieCount > 0)
        {
            zombieCount--; // 死亡後再減 1
            UpdateCounter();
        }
    }
    void UpdateCounter()
    {
        if (zombieCounter != null)
            zombieCounter.text = "Zombies Left: " + zombieCount;
    }
}
