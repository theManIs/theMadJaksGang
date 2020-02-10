namespace Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath
{
    public class MovementMath
    {
        public static float CalcMovementLength(int actionPoints, int movementSpeed) => actionPoints * movementSpeed;
    }
}
