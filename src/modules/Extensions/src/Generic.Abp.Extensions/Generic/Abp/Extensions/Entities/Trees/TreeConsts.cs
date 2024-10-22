namespace Generic.Abp.Extensions.Entities.Trees
{
    public class TreeConsts
    {
        public static int DisplayNameMaxLength { get; set; } = 128;

        public const int MaxDepth = 16;

        public const int CodeUnitLength = 7;

        public const int CodeMaxLength = MaxDepth * (CodeUnitLength + 1) - 1;

        public static int GetCodeLength(int level)
        {
            return CodeUnitLength * level + level - 1;
        }
    }
}