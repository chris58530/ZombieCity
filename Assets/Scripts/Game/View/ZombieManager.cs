using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public Vector2 spawnPosX; //左右
    public Vector2 spawnPosY;// 上下多少
    private Action<SafeZombieBase> deadCallback;
    private int poolCount = 15;
    private int zombieHp;
    public Action<SafeZombieBase, bool> isAutoHitTarget;
    private PoolManager poolManager;
    private Dictionary<SafeZombieBase, int> zombieHpDic = new();
    private Dictionary<SafeZombieBase, Tween> zombieMoveTween = new();
    private HashSet<SafeZombieBase> activeZombies = new HashSet<SafeZombieBase>();
    public int managerID;

    public SafeZombieBase InitZombie(SafeZombieBase zombie, int hp, Action<SafeZombieBase> deadCallback)
    {
        managerID = zombie.id;
        this.deadCallback = deadCallback;
        this.zombieHp = hp;
        poolManager = new GameObject("ZombiePool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(zombie, poolCount, poolManager.transform);
        return zombie;
    }
    public void HitZombie(SafeZombieBase zombie)
    {
        if (zombieHpDic.ContainsKey(zombie))
        {
            zombieHpDic[zombie] -= 1;
            zombie.Hit();
            if (zombieHpDic[zombie] <= 0)
            {
                KillZombie(zombie);
            }
        }
        else
        {
            Debug.LogWarning("Zombie not found in dictionary.");
        }
    }
    public void KillZombie(SafeZombieBase zombie)
    {
        zombieMoveTween[zombie].Kill();
        AddAutoHitTarget(zombie, false);

        zombie.Kill(() => //表演資料
        {
            deadCallback?.Invoke(zombie);//噴錢啥的 邏輯資料
            ResetZombie(zombie);
        });

    }
    public void ResetZombie(SafeZombieBase zombie)
    {
        AddAutoHitTarget(zombie, false);
        if (zombieHpDic.ContainsKey(zombie))
            zombieHpDic.Remove(zombie);
        if (zombieMoveTween.ContainsKey(zombie))
            zombieMoveTween.Remove(zombie);
        activeZombies.Remove(zombie);
        poolManager.Despawn(zombie);
    }
    public void ResetView()
    {
        foreach (var zombie in activeZombies.ToArray())
        {
            if (zombie != null)
            {
                if (zombieMoveTween.ContainsKey(zombie))
                    zombieMoveTween[zombie].Kill();
                AddAutoHitTarget(zombie, false);
                poolManager.Despawn(zombie);
            }
        }

        zombieHpDic.Clear();
        zombieMoveTween.Clear();
        activeZombies.Clear();
    }
    public void SpawnZombie()
    {
        SafeZombieBase zombie = poolManager.Spawn<SafeZombieBase>(poolManager.transform);
        zombie.manager = this;
        zombieHpDic.Add(zombie, zombieHp);
        activeZombies.Add(zombie);
        DOVirtual.DelayedCall(3f, () =>
          {
              AddAutoHitTarget(zombie, true);

          }).SetId(zombie.GetHashCode());
        float[] xFloat = new float[2] { spawnPosX.x, spawnPosX.y };
        int x = UnityEngine.Random.Range(0, xFloat.Length);
        Vector2 spawnPos = new Vector2(xFloat[x], UnityEngine.Random.Range(spawnPosY.x, spawnPosY.y));
        zombie.transform.position = spawnPos;
        bool isFlip = GameDefine.IsFlipByWorld(xFloat[x]);
        MoveZombie(zombie, isFlip);
    }
    public void MoveZombie(SafeZombieBase zombie, bool isFlip)
    {
        zombie.SetFlip(isFlip);
        float speed = UnityEngine.Random.Range(10, 20f);
        float xPos = isFlip ? spawnPosX.x : spawnPosX.y;
        DOVirtual.DelayedCall(8f, () =>
          {
              AddAutoHitTarget(zombie, false);

          }).SetId(zombie.GetHashCode());
        zombieMoveTween.Add(zombie, zombie.transform.DOMoveX(xPos, speed).SetEase(Ease.Linear).OnComplete(() =>
        {

            zombieMoveTween[zombie].Kill();
            ResetZombie(zombie);
        }));

    }
    public void AddAutoHitTarget(SafeZombieBase zombie, bool isTarget)
    {
        isAutoHitTarget?.Invoke(zombie, isTarget);
        zombie.SetIsTarget(isTarget);
    }

}