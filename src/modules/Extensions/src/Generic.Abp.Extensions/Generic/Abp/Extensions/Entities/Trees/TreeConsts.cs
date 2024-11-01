namespace Generic.Abp.Extensions.Entities.Trees
{
    public static class TreeConsts
    {
        public static int NameMaxLength { get; set; } = 256;

        public static int MaxDepth { get; set; } = 16;

        public static int CodeUnitLength { get; set; } = 7;

        //public static int CodeMaxLength = MaxDepth * (CodeUnitLength + 1) - 1;

        public static int GetCodeLength(int level)
        {
            return CodeUnitLength * level + level - 1;
        }
    }
}