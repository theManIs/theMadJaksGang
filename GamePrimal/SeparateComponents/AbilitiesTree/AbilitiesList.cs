namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public struct AbilitiesList
    {
        public const string AutoAttackRanged = "AutoAttackRanged";
        public const string PoisonArrow = "PoisonArrow";

        public static string[] GetAbilities()
        {
            string[] localString = {AutoAttackRanged, PoisonArrow};

            return localString;
        }
    }
}