using System;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.GravediggerClasses
{
    public class Gravedigger : AbstractGravedigger
    {
        #region AbstractGravedigger

        public override void SomeoneDied(MonoMechanicus monomech)
        {
            Collider collider = monomech.GetComponent<Collider>();
            MonoAmplifierRpg amplifier = monomech.GetComponent<MonoAmplifierRpg>();
            ClickToMove clickToMove = monomech.GetComponent<ClickToMove>();
            DamageLogger damageLogger = monomech.GetComponent<DamageLogger>();

            Object.Destroy(monomech);
            Object.Destroy(amplifier);
            Object.Destroy(damageLogger);
            Object.Destroy(collider);
            Object.Destroy(clickToMove);
        } 

        #endregion
    }
}