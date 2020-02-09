

using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.AbstractSources;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public struct MonomechAndDistance
    {
        public float RawDistance;
        public MonoMechanicus monomech;
    }
    public class AiFrame : EnableIncluded, IArtificial
    {
        public Transform CurrentTransform;
        public MonoMechanicus monomech;

        public void SeekTarget()
        {
            MonoMechanicus[] allMonomech = Object.FindObjectsOfType<MonoMechanicus>();
            MonoMechanicus[] allEnemies = allMonomech.Where(c => c._monoAmplifierRpg.BlueRedTeam != monomech._monoAmplifierRpg.BlueRedTeam).ToArray();
//            MonomechAndDistance[] enemiesAndDistance =
//                allEnemies.Select((item, index) => new MonomechAndDistance()
//                {
//                    monomech = item, 
//                    RawDistance = Vector3.Distance(item.transform.position, monomech.transform.position)
//                }).ToArray();

            SortedDictionary<float, MonoMechanicus> sd = new SortedDictionary<float, MonoMechanicus>();

            foreach (var monomechEnemy in allEnemies)
                sd.Add(Vector3.Distance(monomechEnemy.transform.position, monomech.transform.position), monomechEnemy);

            foreach (KeyValuePair<float, MonoMechanicus> kvp in sd)
                Debug.Log(kvp.Value.gameObject.name + " " + kvp.Key);
        }
    }
}