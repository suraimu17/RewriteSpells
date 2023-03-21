using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHealth
{
    void MaxHPSet(int floor);
    void DealDamage(int damage);
}
