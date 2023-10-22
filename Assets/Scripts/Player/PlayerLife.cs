using Assets.Scripts.Enemy;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLife : MonoBehaviour
    {

        public float lifeExpectancyOutsideSafezone = 60f * 5f;

        public float timerDuration = 60f * 25;
        public float timerStartTime = 0f;

        public float killingAnimationAngularVelocity = 360f;

        public float Health { get; private set; } = 1f;

        // Seconds left before the player dies
        public float RemainingTime
        {
            get
            {
                return timerDuration - (Time.time - timerStartTime);
            }
        }

        private PlayerSafezoneDetector safezoneDetector;

        private bool killingAnimation = false;
        private EnemyChasingController enemyKillingPlayer;

        private void OnEnable()
        {
            timerStartTime = Time.time;
        }

        void Awake()
        {
            safezoneDetector = FindObjectOfType<PlayerSafezoneDetector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (killingAnimation)
            {
                UpdateKillingAnimation();
                return;
            }

            if (safezoneDetector.IsInSafezone)
            {
                Health = 1f;
            } else
            {
                Health -= Time.deltaTime / lifeExpectancyOutsideSafezone;
            }
        }

        private void UpdateKillingAnimation()
        {
            // Rotate the player to face the enemy
            Vector3 direction = enemyKillingPlayer.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotationToEnemy = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToEnemy, killingAnimationAngularVelocity * Time.deltaTime);

            // Rotate the camera horizontally to face the enemy
            MouseLook mouseLook = FindObjectOfType<MouseLook>();
            Vector3 cameraDirection = enemyKillingPlayer.transform.position + enemyKillingPlayer.HeadHeight * Vector3.up - mouseLook.transform.position;
            float currentHorizontalAngle = mouseLook.transform.localEulerAngles.x;
            float targetHorizontalAngle = Quaternion.LookRotation(cameraDirection).eulerAngles.x;
            float newHorizontalAngle = Mathf.MoveTowardsAngle(currentHorizontalAngle, targetHorizontalAngle, killingAnimationAngularVelocity * Time.deltaTime);
            mouseLook.transform.localEulerAngles = new Vector3(newHorizontalAngle, mouseLook.transform.localEulerAngles.y, mouseLook.transform.localEulerAngles.z);
        }

        public void KillAnimationStart(EnemyChasingController enemy)
        {
            FindObjectOfType<PlayerMovement>().enabled = false;
            FindObjectOfType<MouseLook>().enabled = false;
            FindObjectOfType<PlayerInteraction>().enabled = false;

            killingAnimation = true;
            enemyKillingPlayer = enemy;
        }
    }
}