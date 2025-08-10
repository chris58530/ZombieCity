using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public Vector2 spawnPosX; //左右
    public Vector2 spawnPosY;// 上下多少
    private Action<ZombieBase> deadCallback;
    private int poolCount = 15;
    private int zombieHp;
    public Action<ZombieBase, bool> isAutoHitTarget;
    private PoolManager poolManager;
    private Dictionary<ZombieBase, int> zombieHpDic = new();
    private Dictionary<ZombieBase, Tween> zombieMoveTween = new();
    private HashSet<ZombieBase> activeZombies = new HashSet<ZombieBase>();
    public int managerID;

    public ZombieBase InitZombie(ZombieBase zombie, int hp, Action<ZombieBase> deadCallback)
    {
        managerID = zombie.id;
        this.deadCallback = deadCallback;
        this.zombieHp = hp;
        poolManager = new GameObject("ZombiePool_").AddComponent<PoolManager>();
        poolManager.transform.SetParent(transform);
        poolManager.RegisterPool(zombie, poolCount, poolManager.transform);
        return zombie;
    }
    public void HitZombie(ZombieBase zombie)
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
    public void KillZombie(ZombieBase zombie)
    {
        zombieMoveTween[zombie].Kill();
        AddAutoHitTarget(zombie, false);

        zombie.Kill(() => //表演資料
        {
            deadCallback?.Invoke(zombie);//噴錢啥的 邏輯資料
            ResetZombie(zombie);
        });

    }
    public void ResetZombie(ZombieBase zombie)
    {
        AddAutoHitTarget(zombie, false);
        if (zombieHpDic.ContainsKey(zombie))
            zombieHpDic.Remove(zombie);
        if (zombieMoveTween.ContainsKey(zombie))
            zombieMoveTween.Remove(zombie);
        activeZombies.Remove(zombie);
        activeBattleZombies.Remove(zombie);
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

        foreach (var zombie in activeBattleZombies.ToArray())
        {
            if (zombie != null)
            {
                poolManager.Despawn(zombie);
            }
        }

        zombieHpDic.Clear();
        zombieMoveTween.Clear();
        activeZombies.Clear();
        activeBattleZombies.Clear();
    }
    public void SpawnZombie()
    {
        ZombieBase zombie = poolManager.Spawn<ZombieBase>(poolManager.transform);
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
    public void MoveZombie(ZombieBase zombie, bool isFlip)
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
    public void AddAutoHitTarget(ZombieBase zombie, bool isTarget)
    {
        isAutoHitTarget?.Invoke(zombie, isTarget);
        zombie.SetIsTarget(isTarget);
    }
    private HashSet<ZombieBase> activeBattleZombies = new HashSet<ZombieBase>();

    public void SpawnBattleZombie(Vector2 spawnPoint, IHittable campCar, int hp, float atk)
    {
        ZombieBase zombie = poolManager.Spawn<ZombieBase>(poolManager.transform);
        zombie.SetLayer("Battle");
        zombie.manager = this;
        zombie.hp = hp;
        zombie.deadCallBack = (zombie) =>
        {
            Debug.Log("Zombie is dead.");
        };
        zombie.attack = atk;
        zombie.transform.position = spawnPoint;
        zombie.SetBattleData(campCar);
        activeBattleZombies.Add(zombie);
        zombie.StartMove();
    }
}