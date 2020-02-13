namespace Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath
{
    public class MovementMath
    {
        public static float CalcMovementLength(int actionPoints, int movementSpeed) => actionPoints * movementSpeed;
        public static float AttackVectorCoercion(float realDistance, float coercionCoefficient) => 
            ((realDistance - coercionCoefficient) / realDistance);
    }
}
