namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public struct AbilitiesList
    {
        public const string AutoAttackRanged = "AutoAttackRanged";
        public const string AutoAttackMelee = "AutoAttackMelee";
        public const string PoisonArrow = "PoisonArrow";
        public const string IceShard = "IceShard";

        public static string[] GetAbilities()
        {
            string[] localString = {AutoAttackRanged, PoisonArrow, IceShard};

            return localString;
        }
    }
}