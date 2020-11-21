using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootableObjectsHealth : MonoBehaviour
{
    public virtual void TakeDamage(int amount, Vector3 hitPoint) {}
}
