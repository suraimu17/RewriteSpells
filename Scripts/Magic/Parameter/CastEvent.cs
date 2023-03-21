namespace CM.Magic.Parameter
{
    public struct CastEvent
    {
        public CastEvent(ICastable castable, float freezeTime)
        {
            this.castable = castable;
            this.freezeTime = freezeTime;
        }
        public readonly ICastable castable;     //�r�������X�y��
        public readonly float freezeTime;       //�d������
    }
}
