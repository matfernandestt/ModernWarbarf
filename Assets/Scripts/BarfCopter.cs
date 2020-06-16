using System;
using UnityEngine;

public class BarfCopter : BarfFlyController
{
    [SerializeField] private Transform shootSource;
    [SerializeField] private ObjectPool shotPool;
    private void OnEnable()
    {
        ActionAttackSpecial += Shoot;
    }

    private void OnDisable()
    {
        ActionAttackSpecial -= Shoot;
    }

    public void Shoot()
    {
        var newShoot = shotPool.RequestObject(shootSource.position, Quaternion.identity);
        shotPool.ReturnObject(newShoot, 2f);
    }
}
