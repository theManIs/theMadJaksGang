namespace Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath
{
    public class MovementMath
    {
        public static float CalcMovementLength(int actionPoints, int movementSpeed) => actionPoints * movementSpeed;
        public static float CalcHitRange(int actionPoints, int movementSpeed, float weaponRange) =>
            CalcMovementLength(actionPoints, movementSpeed) + weaponRange;
        public static float CalcRadiusError(float errorRadius) => errorRadius;

        public static float AttackVectorCoercion(float realDistance, float coercionCoefficient, float coercionError, float maxMovementLength) =>
            maxMovementLength > realDistance ? (realDistance - coercionCoefficient + coercionError) / realDistance  : maxMovementLength / realDistance;
    }
}
