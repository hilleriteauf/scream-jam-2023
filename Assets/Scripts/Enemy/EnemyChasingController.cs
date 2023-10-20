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

        [Header("Looking Around Settings")]

        [Tooltip("The number of rays casted in each direction to evaluate the best direction to look at (clockwise or counterclockwise)")]
        public int DirectionEvaluationResolution = 45;

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

        // The direction in which to turn when looking around (1 for clockwise, -1 for counter-clockwise)
        private int lookingAroundDirection = 1;
        // The rotation to reach when looking around
        private Quaternion lookingAroundRotationTarget = Quaternion.identity;

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
                // We finished looking in the direction the player was last seen looking at, start looking around
                StartLookingAround();
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, EnemyVision.LastSeenRotation, Agent.angularSpeed * Time.deltaTime);

        }

        private void UpdateLookingAround()
        {
            float angle = Quaternion.Angle(transform.rotation, lookingAroundRotationTarget);
            Debug.Log("Angle : " + angle);
            if (angle < 0.1f)
            {
                // We finished looking around, stop chasing
                StopChasing();
            }

            // We rotate the enemy in the direction we want it to look at, clockwise or counter-clockwise depending on the evaluated direction
            float deltaAngle = Math.Abs(angle) * lookingAroundDirection;
            Quaternion target = transform.rotation * Quaternion.Euler(0f, deltaAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Agent.angularSpeed * Time.deltaTime);

        }

        private void StartLookingAround()
        {
            
            // We evaluate the best direction in which to turn (clockwise or counter-clockwise)

            float[] scores = new float[2];
            float[] firstIntrestingAngle = new float[] { -1, -1 };
            float[] lastIntrestingAngle = new float[] { -1, -1 };

            // We dont evaluate the directions that are in the field of view of the enemy
            int bound = (int)Math.Ceiling((EnemyVision.FieldOfViewAngle * (float)DirectionEvaluationResolution) / (360f * 2f));

            for (int direction = -1; direction < 2; direction += 2)
            {
                for (int i = bound; i < DirectionEvaluationResolution / 2; i++)
                {
                    float angle = (360f / DirectionEvaluationResolution) * i * direction;
                    // Cast a ray in the direction
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Quaternion.Euler(0f, angle, 0f) * transform.forward, out hit, Mathf.Infinity))
                    {
                        float score = hit.distance;

                        // Rays score count more for low angles because low angles will be reached faster
                        float multiplier = 1 + i / (DirectionEvaluationResolution - (float)bound);
                        score *= multiplier;

                        //Debug.DrawRay(transform.transform.position, Quaternion.Euler(0f, angle, 0f) * transform.forward * score, direction == 1 ? new Color(0, 0, multiplier - 0.5f) : new Color(multiplier - 0.5f, 0, 0), 20f);

                        scores[direction == 1 ? 0 : 1] += score;

                        // We keep track of the minimum intresting rotation for each direction
                        if (firstIntrestingAngle[direction == 1 ? 0 : 1] == -1 && hit.distance > 3f)
                        {
                            firstIntrestingAngle[direction == 1 ? 0 : 1] = angle;
                        }
                        if (hit.distance > 3f)
                        {
                            lastIntrestingAngle[direction == 1 ? 0 : 1] = angle;
                        }
                    }

                }
            }
            
            lookingAroundDirection = scores[0] > scores[1] ? 1 : -1;
            int directionArrayIndex = lookingAroundDirection == 1 ? 0 : 1;
            //Debug.Log("Looking around direction : " + lookingAroundDirection);

            float angleToRotate = firstIntrestingAngle[(directionArrayIndex + 1) % 2] == -1 ? lastIntrestingAngle[directionArrayIndex] * lookingAroundDirection : firstIntrestingAngle[(directionArrayIndex + 1) % 2] * lookingAroundDirection * -1;
            //Debug.Log("Angle to rotate : " + angleToRotate);
            lookingAroundRotationTarget = Quaternion.Euler(0f, angleToRotate, 0f) * transform.rotation;
            //Debug.DrawRay(transform.position, lookingAroundRotationTarget * Vector3.forward * 10, Color.green, 20f);
            state = State.LookingAround;
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