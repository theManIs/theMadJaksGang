namespace Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath
{
    public class MovementMath
    {
        public static float CalcMovementLength(int actionPoints, int movementSpeed) => actionPoints * movementSpeed;
        public static float CalcRadiusError(float errorRadius) => errorRadius;
        public static float AttackVectorCoercion(float realDistance, float coercionCoefficient, float coercionError) => 
            (realDistance - coercionCoefficient + coercionError) / realDistance;
    }
}
