using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventories.Debugs;



namespace CM.Magic.Legacy
{
    public static class CasterExtend
    {
        public static int GetNumInput(this Caster caster)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) return 0;
            else if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6)) return 6;
            else if (Input.GetKeyDown(KeyCode.Alpha7)) return 7;
            else if (Input.GetKeyDown(KeyCode.Alpha8)) return 8;
            else if (Input.GetKeyDown(KeyCode.Alpha9)) return 9;
            return -1;
        }
    }
}
