using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.enemys
{
    interface IRadar
    {
        void SearchWall(Vector3 target);
    }
}
