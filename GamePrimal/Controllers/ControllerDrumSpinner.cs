﻿using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerDrumSpinner
    {
        private static Queue<Transform> _theDrum = new Queue<Transform>();
        private bool _roundIsFilled = false;
        private int _frameCount = -1;
        private readonly int _frameThrottle = 25;
        public float instanceId;

        public ControllerDrumSpinner()
        {
            instanceId = Random.value;
        }

        public bool ReleaseRound()
        {
            ReleaseFrame();

            return !_roundIsFilled;
        }

        public bool DoIMove(Transform applicant)
        {
            if (_roundIsFilled) return false;

            Debug.Log(_theDrum.Count + " " + Time.time);

            FillTheDrum();

            int whoIsNext = _theDrum.Peek().GetInstanceID();
            int applicantId = applicant.GetInstanceID();
            bool doIMove = whoIsNext == applicantId;

            MarkThisRoundFilled(doIMove);

            return doIMove;
        }

        private void ReleaseFrame()
        {
            if (Time.frameCount - _frameCount > _frameThrottle)
            {
                _roundIsFilled = false;
                _frameCount = Time.frameCount;
            }
        }
        private void FillTheDrum()
        {
            if (_theDrum.Count <= 0)
                _theDrum = SpinTheDrum();
        }
        private void MarkThisRoundFilled(bool doIMove)
        {
            if (!doIMove) return;

            _theDrum.Dequeue();

//            Debug.Log(_theDrum.Count);

            _roundIsFilled = true;
        }
        protected Queue<Transform> SpinTheDrum()
        {
            List<MonoMechanicus> allParticipants = Object.FindObjectsOfType<MonoMechanicus>().ToList();
            Queue<Transform> toPutInDrum = new Queue<Transform>();
            Dictionary<int, Transform> _initiativeList = new Dictionary<int, Transform>();

            foreach (MonoMechanicus mech in allParticipants)
            {
                MonoAmplifierRpg rpg = mech.GetComponent<MonoAmplifierRpg>();

                if (rpg)
                    _initiativeList.Add(rpg.GetInitiative(), rpg.transform);
            }

            List<KeyValuePair<int, Transform>> _sortedList = _initiativeList.ToList();

            _sortedList.Sort((p1, p2) => p1.Key.CompareTo(p2.Key) * -1);

            foreach (var VARIABLE in _sortedList)
            {
                Debug.Log(Time.time + " " + VARIABLE);
            }

            foreach (KeyValuePair<int, Transform> mech in _sortedList)
                toPutInDrum.Enqueue(mech.Value);

            return toPutInDrum;
        }
    }
}