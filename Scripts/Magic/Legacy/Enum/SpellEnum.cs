using System;

using CM.Magic.MagicObject;


namespace CM.Magic.Legacy
{
    [Obsolete("���Enum�̎����҂�")]
    public enum SpellObjectControllerType
    {
        GENERATE,
        REMOVE,
        UPDATE
    }

    [Obsolete("���Enum�̎����҂�")]
    public enum SpellObjectType
    {
        NONE,
        DEBUG,
        PROJECTILE,
        AOE
    }




    public static class SpellObjectTypeExtension
    {
        [Obsolete("��֊֐��̎����҂�")]
        public static bool isSameType(this SpellObjectType type, BaseMagicObject spellObject)
        {
            return type == type.GetSpellObjectType(spellObject);
        }


        [Obsolete("��֊֐��̎����҂�")]
        public static SpellObjectType GetSpellObjectType(this SpellObjectType type, BaseMagicObject spellObject)
        {
            switch (spellObject)
            {
                case Projectile proj:
                    return SpellObjectType.PROJECTILE;
                case AreaOfEffect aoe:
                    return SpellObjectType.AOE;
                default:
                    return SpellObjectType.NONE;
            }
        }
    }
}
