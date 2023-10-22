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

        public float Speed = 7f;
        public float ChasingAngularSpeed = 360f;
        public float Acceleration = 8f;

        [Header("Looking To Last Seen Direction Settings")]
        public float PostLookingToLastSeenDirectionPause = 0f;

        [Header("Looking Around Settings")]

        public int LookingAroundIterationNumber = 3;
        [Tooltip("The number of rays casted in each direction to evaluate the best direction to look at (clockwise or counterclockwise)")]
        public int DirectionEvaluationResolution = 45;
        public float PostLookingAroundPause = 1f;
        public float LookingAroundAngularSpeed = 50f;

        public bool Enabled { get { return step != Step.Disabled; } }

        private PlayerLightDetector playerLightDetector;

        private Step _step = Step.Disabled;
        private Step step
        {
            get { return _step; }
            set
            {
                if (_step != value)
                {
                    stepStartTime = Time.time;
                    postStepPauseStartTime = 0f;
                    _step = value;
                }
            }
        }
        private float stepStartTime = 0f;
        private float postStepPauseStartTime = 0f;

        // The direction in which to turn when looking around (1 for clockwise, -1 for counter-clockwise)
        private int lookingAroundDirection = 1;
        // The rotation to reach when looking around
        private Quaternion lookingAroundRotationTarget = Quaternion.identity;
        private int lookingAroundIterationCounter = 0;

        public void StartChasing()
        {
            Agent.speed = Speed;
            Agent.angularSpeed = ChasingAngularSpeed;
            Agent.acceleration = Acceleration;
            step = Step.Screaming;
        }

        public void StopChasing()
        {
            step = Step.Disabled;
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

            // If the player is in sight, and we are chasing or investigating, we resume chasing with the new position of the player
            if (EnemyVision.GetPlayerInSight() && step >= Step.Chasing)
            {
                // We don't want to pause when resuming chasing
                StartChasingStep();
            }

            switch (step)
            {
                case Step.Screaming:
                    UpdateScreaming();
                    break;
                case Step.Chasing:
                    UpdateChasing();
                    break;
                case Step.LookingToLastSeenDirection:
                    UpdateLookingToLastSeenDirection();
                    break;
                case Step.LookingAround:
                    UpdateLookingAround();
                    break;
            }
        }

        private void UpdateScreaming()
        {
            if (Time.time - stepStartTime > ScreamingShakingDuration)
            {
                // We finished screaming, start chasing
                StartChasingStep();
                return;
            }

            Vector3 directionToTarget = EnemyVision.LastSeenPosition - transform.position;
            directionToTarget.y = 0f;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            float animationAngle = ScreamingShakingIntensity * Mathf.Sin((Time.time - stepStartTime) * ScreamingShakingSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget * Quaternion.Euler(0f, animationAngle, 0f), Agent.angularSpeed * Time.deltaTime);
        }

        private void StartChasingStep()
        {
            step = Step.Chasing;
            Agent.SetDestination(EnemyVision.LastSeenPosition);
        }

        private void UpdateChasing()
        {
            if (EnemyVision.PlayerInSight)
            {
                // Direction to player
                Vector3 directionToTarget = playerLightDetector.transform.position - transform.position;
                directionToTarget.y = 0f;
                Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget, ChasingAngularSpeed * Time.deltaTime);
            }

            if (Agent.remainingDistance < 0.1f)
            {
                if (postStepPauseStartTime == 0f)
                {
                    postStepPauseStartTime = Time.time;
                }

                if (Time.time - postStepPauseStartTime > PostLookingToLastSeenDirectionPause)
                {
                    // We reached the last seen position, start looking in the direction the player was last seen looking at
                    step = Step.LookingToLastSeenDirection;
                }
            }
        }

        private void UpdateLookingToLastSeenDirection()
        {
            if (Quaternion.Angle(transform.rotation, EnemyVision.LastSeenRotation) < 0.1f)
            {
                if (postStepPauseStartTime == 0f)
                {
                    postStepPauseStartTime = Time.time;
                }

                if (Time.time - postStepPauseStartTime > PostLookingToLastSeenDirectionPause)
                {
                    // We finished looking in the direction the player was last seen looking at, start looking around
                    StartLookingAround();
                }
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, EnemyVision.LastSeenRotation, ChasingAngularSpeed * Time.deltaTime);

        }

        private void UpdateLookingAround()
        {
            float angle = Quaternion.Angle(transform.rotation, lookingAroundRotationTarget);
            //Debug.Log("Angle : " + angle);
            if (angle < 0.1f)
            {
                if (postStepPauseStartTime == 0f)
                {
                    postStepPauseStartTime = Time.time;
                }

                if (Time.time - postStepPauseStartTime > PostLookingAroundPause)
                {
                    // We finished looking around, going for another iteration if we didn't reach the maximum number of iterations
                    // Or we stop chasing if we reached the maximum number of iterations
                    lookingAroundIterationCounter++;
                    if (lookingAroundIterationCounter < LookingAroundIterationNumber)
                    {
                        StartLookingAround(false);
                        return;
                    }

                    StopChasing();
                }
            }

            // We rotate the enemy in the direction we want it to look at, clockwise or counter-clockwise depending on the evaluated direction
            float deltaAngle = Math.Abs(angle) * lookingAroundDirection;
            Quaternion target = transform.rotation * Quaternion.Euler(0f, deltaAngle, 0f);
            //Debug.DrawRay(transform.position, target * Vector3.forward * 5, Color.yellow, 0.2f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, LookingAroundAngularSpeed * Time.deltaTime);

        }

        private void StartLookingAround(bool firstIteration = true)
        {
            step = Step.LookingAround;
            postStepPauseStartTime = 0f;

            if (firstIteration)
            {
                lookingAroundIterationCounter = 0;
            }
            
            EvaluateLookingAroundDirection();
        }

        private void EvaluateLookingAroundDirection()
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

                        //Debug.DrawRay(transform.transform.position, Quaternion.Euler(0f, angle, 0f) * transform.forward * score, direction == 1 ? new Color(0, 0, multiplier - 0.5f) : new Color(multiplier - 0.5f, 0, 0), 3f);

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

            float angleToRotate = 0f;
            if (firstIntrestingAngle[directionArrayIndex] == -1)
            {
                angleToRotate = lastIntrestingAngle[directionArrayIndex];
            } else
            {
                angleToRotate = firstIntrestingAngle[(directionArrayIndex + 1) % 2];
            }
            //Debug.Log("Angle to rotate : " + angleToRotate);
            lookingAroundRotationTarget = Quaternion.Euler(0f, angleToRotate, 0f) * transform.rotation;
            //Debug.DrawRay(transform.position, lookingAroundRotationTarget * Vector3.forward * 10, Color.green, 5f);
        }

        // Different steps of the chasing behaviour
        private enum Step
        {
            Disabled,
            Screaming,
            Chasing,
            LookingToLastSeenDirection,
            LookingAround,
        }
    }
}