using Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree;
using Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Helpers
{
    [CreateAssetMenu(fileName = "CharacterFeatures", menuName = "ScriptableObjects/CharacterFeatures", order = 1)]
    public class CharacterFeatures : ScriptableObject
    {
        public string ObjectName;
        public int StartHealth;
        public int MaxHealth;
        public int Damage;
        public int Initiative;
        public int NavMeshSpeed;
        public int TurnPoints;
        public int MaxTurnPoints;
        public WeaponOperator StartWeapon;
        public Sprite CharacterPortrait;
        public int ExperienceActual;
        public int ExperienceMax;
//        public Transform PreferredProjectile;
        public int MoveSpeed;
        public string[] AbilitiesSet = AbilitiesList.GetAbilities(); //todo such a bound has to be dismissed
    }
}
