namespace CM.Magic.Parameter
{
    public struct CastEvent
    {
        public CastEvent(ICastable castable, float freezeTime)
        {
            this.castable = castable;
            this.freezeTime = freezeTime;
        }
        public readonly ICastable castable;     //‰r¥‚µ‚½ƒXƒyƒ‹
        public readonly float freezeTime;       //d’¼ŠÔ
    }
}
