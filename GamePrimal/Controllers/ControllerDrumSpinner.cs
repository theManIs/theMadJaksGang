using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerDrumSpinner
    {
        public float InstanceId;

        private Queue<Transform> _theDrum = new Queue<Transform>();
        private bool _roundIsFilled = false;
        private int _frameCount = -1;
        private readonly int _frameThrottle = 25;
        private Transform _whoseTurn;
        private int _frameShieldIdleRunning = Int32.MinValue;
        private bool _idleRunning = false;

        public bool IdleRunning => _idleRunning;

        public Queue<Transform> ActualDrum => new Queue<Transform>(_theDrum);
        public Queue<Transform> DrumBlank => SpinTheDrum();
        public Transform GetWhoseTurn() => _whoseTurn;

        public ControllerDrumSpinner()
        {
            InstanceId = Random.value;
//            Debug.Log("ControllerDrumSpinner " + InstanceId);
        }

        public bool ReleaseRound()
        {
            return ReleaseRoundUnconditional();
        }

        public bool ReleaseRoundUnconditional()
        {
            ReleaseFrame();

            return !_roundIsFilled;
        }

        public void RemoveBody() => _theDrum = new Queue<Transform>
            (_theDrum.ToArray().Where(participant => participant.GetComponent<MonoMechanicus>()));

        private void ResolveWhoseTurn()
        {
            if (!_roundIsFilled)
                foreach (MonoMechanicus m in StaticProxyObjectFinder.FindObjectOfType<MonoMechanicus>())
                    DoIMove(m.transform);
        }

        public void EnterIdleMode()
        {
            if (_frameShieldIdleRunning > Time.frameCount - _frameThrottle && _frameShieldIdleRunning != Int32.MinValue)
                return;

            _idleRunning = true;
            _frameShieldIdleRunning = Time.frameCount;
        }

        public void LeaveIdleMode()
        {
            if (_frameShieldIdleRunning > Time.frameCount - _frameThrottle && _frameShieldIdleRunning != Int32.MinValue)
                return;

            _idleRunning = false;
            _frameShieldIdleRunning = Time.frameCount;
        }

        public void UserUpdate()
        {
            if (_idleRunning)
                return;

            ResolveWhoseTurn();

            if (StaticProxyInput.Space)
                ReleaseRound();
        }

        public bool DoIMove(Transform applicant)
        {
            if (_roundIsFilled) return false;
//            Debug.Log(_theDrum.Count + " " + Time.time);
            RemoveBody();
            FillTheDrum();

            int whoIsNext = _theDrum.Peek().GetInstanceID();
            int applicantId = applicant.GetInstanceID();
//            Debug.Log($"whoIsNext {whoIsNext} _theDrum {_theDrum.Count} applicantId {applicantId}");
            bool doIMove = whoIsNext == applicantId;
            //            Debug.Log(whoIsNext + " == " + applicantId + " = " + doIMove);
            MarkThisRoundFilled(doIMove);

            if (doIMove)
            {
//                Debug.Log("Turn was found " + Time.time);
                StaticProxyEvent.ETurnWasFound.Invoke(new EventTurnWasFoundParams() { TurnApplicant = applicant });

                _whoseTurn = applicant;
            }

            return doIMove;
        }

        private void ReleaseFrame()
        {
            //Debug.Log(Time.frameCount + " " + _frameCount + " " + _frameThrottle);
            if (Time.frameCount - _frameCount > _frameThrottle)
            {
                _roundIsFilled = false;
                _frameCount = Time.frameCount;
            }
        }

        private void FillTheDrum()
        {
            if (_theDrum.Count <= 0)
            {
                _theDrum = SpinTheDrum();

                StaticProxyEvent.ETurnWasFound.Invoke(new EventTurnWasFoundParams());
            }
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
            Dictionary<int, Transform> initiativeList = new Dictionary<int, Transform>();

            foreach (MonoMechanicus mech in allParticipants)
            {
                MonoAmplifierRpg rpg = mech.GetComponent<MonoAmplifierRpg>();

                if (rpg)
                    initiativeList.Add(RecursiveIncrement(rpg.GetInitiative(), initiativeList), rpg.transform);
            }

            List<KeyValuePair<int, Transform>> sortedList = initiativeList.ToList();

            sortedList.Sort((p1, p2) => p1.Key.CompareTo(p2.Key) * -1);

//            foreach (var VARIABLE in sortedList)
//            {
//                Debug.Log(Time.time + " " + VARIABLE);
//            }

            foreach (KeyValuePair<int, Transform> mech in sortedList)
                toPutInDrum.Enqueue(mech.Value);

            return toPutInDrum;
        }

        protected int RecursiveIncrement(int value, Dictionary<int, Transform> initiativeList)
        {
            if (initiativeList.ContainsKey(value))
                return RecursiveIncrement(value + 1, initiativeList);
            else
                return value;
        }

        public ControllerDrumSpinner UserAwakeInstantiator() => this;
    }
}