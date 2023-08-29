using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleController
{
    void Attack();
    void Damaged(float damage);
    void Die();
}
