using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyChasingController : MonoBehaviour
    {

        public EnemyVision EnemyVision;

        public EnemyController EnemyController;

        public NavMeshAgent Agent;

        [Header("Screaming Settings")]

        public float ScreamingShakingDuration = 1f;
        public float ScreamingShakingIntensity = 10f;
        public float ScreamingShakingSpeed = 50f;

        [Header("Chasing Settings")]

        public float Speed = 3.5f;
        public float AngularSpeed = 120f;
        public float Acceleration = 8f;

        public bool Enabled { get { return state != State.Disabled; } }

        private PlayerLightDetector playerLightDetector;

        private State _state = State.Disabled;
        private State state
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    stateStartTime = Time.time;
                }
            }
        }
        private float stateStartTime = 0f;

        public void StartChasing()
        {
            Agent.speed = Speed;
            Agent.angularSpeed = AngularSpeed;
            Agent.acceleration = Acceleration;
            state = State.Screaming;
        }

        public void StopChasing()
        {
            state = State.Disabled;
            EnemyController.ChaseEnded();
        }

        // Use this for initialization
        void Start()
        {
            playerLightDetector = FindObjectOfType<PlayerLightDetector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Enabled)
            {
                return;
            }

            if (EnemyVision.GetPlayerInSight() && state >= State.Chasing)
            {
                Agent.SetDestination(EnemyVision.LastSeenPosition);
                state = State.Chasing;
            }

            switch (state)
            {
                case State.Screaming:
                    UpdateScreaming();
                    break;
                case State.Chasing:
                    UpdateChasing();
                    break;
                case State.LookingToLastSeenDirection:
                    UpdateLookingToLastSeenDirection();
                    break;
                case State.LookingAround:
                    UpdateLookingAround();
                    break;
            }
        }

        private void UpdateScreaming()
        {
            if (Time.time - stateStartTime > ScreamingShakingDuration)
            {
                // We finished screaming, start chasing
                state = State.Chasing;
                Agent.SetDestination(EnemyVision.LastSeenPosition);
                return;
            }

            Vector3 directionToTarget = EnemyVision.LastSeenPosition - transform.position;
            directionToTarget.y = 0f;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            float animationAngle = ScreamingShakingIntensity * Mathf.Sin((Time.time - stateStartTime) * ScreamingShakingSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget * Quaternion.Euler(0f, animationAngle, 0f), Agent.angularSpeed * Time.deltaTime);
        }

        private void UpdateChasing()
        {
            if (Agent.remainingDistance < 0.1f)
            {
                // We reached the last seen position, start looking in the direction the player was last seen looking at
                state = State.LookingToLastSeenDirection;
                Debug.Log("Start investigating");
            }
        }

        private void UpdateLookingToLastSeenDirection()
        {
            if (Quaternion.Angle(transform.rotation, EnemyVision.LastSeenRotation) < 0.1f)
            {
                // We finished looking in the direction the player was last seen looking at, stop chasing
                StopChasing();
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, EnemyVision.LastSeenRotation, Agent.angularSpeed * Time.deltaTime);

        }

        private void UpdateLookingAround()
        {

        }

        // Different states of the chasing behaviour
        private enum State
        {
            Disabled,
            Screaming,
            Chasing,
            LookingToLastSeenDirection,
            LookingAround,
        }
    }
}