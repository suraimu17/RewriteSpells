using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Magic.Temp
{
    public class EnemySpellSetter : MonoBehaviour
    {
        [SerializeField] private Caster caster;
        [SerializeField] private string spell;
        [SerializeField] private int level;


        private void Start()
        {
            Spell spell = MagicLoader.loader.GetSpell(this.spell, level);
            if(spell != null)
            {
                //caster.SetCastable(spell, 0);
            }
        }

    }
}
