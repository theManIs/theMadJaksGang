namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public abstract class AbstractWeaponBased : AbstractAbility
    {
        public int ActionCost = 2;
        public override bool IsWeaponBased() => true;
    }
}
