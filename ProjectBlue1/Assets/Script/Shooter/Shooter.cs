using Assets.HeroEditor.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooter : SingletonMonoBehaviour<Shooter>
{
    [SerializeField]
    SwordAura projectile;

    [SerializeField]
    Transform shootPos;

    GameObjectPool<SwordAura> projectilePool;

    protected override void OnStart()
    {
        projectilePool = new GameObjectPool<SwordAura>(5, () =>
        {
            var obj = Instantiate(projectile);
            projectile.transform.position = shootPos.position;
            obj.SetActive(false);
            return obj;
        });
    }

    public void GetProjectile()
    {
        Debug.Log("투사체 발사!!");
        var obj = projectilePool.Get();
        obj.transform.position = shootPos.position;
        obj.SetActive(true);
    }
    public void SetProjectile(SwordAura swordAura)
    {
        swordAura.SetActive(false);
        projectilePool.Set(swordAura);
    }
}
