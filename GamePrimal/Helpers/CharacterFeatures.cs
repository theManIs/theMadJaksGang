using Assets.GamePrimal.Mono;
using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
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
    }
}
