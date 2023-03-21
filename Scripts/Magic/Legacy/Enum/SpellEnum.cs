using System;

using CM.Magic.MagicObject;


namespace CM.Magic.Legacy
{
    [Obsolete("ë„ë÷EnumÇÃé¿ëïë“Çø")]
    public enum SpellObjectControllerType
    {
        GENERATE,
        REMOVE,
        UPDATE
    }

    [Obsolete("ë„ë÷EnumÇÃé¿ëïë“Çø")]
    public enum SpellObjectType
    {
        NONE,
        DEBUG,
        PROJECTILE,
        AOE
    }




    public static class SpellObjectTypeExtension
    {
        [Obsolete("ë„ë÷ä÷êîÇÃé¿ëïë“Çø")]
        public static bool isSameType(this SpellObjectType type, BaseMagicObject spellObject)
        {
            return type == type.GetSpellObjectType(spellObject);
        }


        [Obsolete("ë„ë÷ä÷êîÇÃé¿ëïë“Çø")]
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
