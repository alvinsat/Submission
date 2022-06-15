using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(float damage);
    void ApplyDamage(float damage, Vector3 position);
}
