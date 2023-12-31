﻿using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerLife : MonoBehaviour
    {

        public float lifeExpectancyOutsideSafezone = 60f * 5f;

        public float timerDuration = 60f * 25;
        public float timerStartTime = 0f;

        public float killingAnimationAngularVelocity = 360f;

        public float Health { get; private set; } = 1f;

        public string MenuSceneName;

        public List<string> winMessages;
        public List<float> winMessagesDurations;

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

        private bool alive = true;
        private float killTime = -1f;

        private BoxCollider endCollider;

        private void OnEnable()
        {
            timerStartTime = Time.time;
        }

        void Awake()
        {
            safezoneDetector = FindObjectOfType<PlayerSafezoneDetector>();
            endCollider = GameObject.FindGameObjectWithTag("EndCollider").GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (killTime > -1f && Time.time - killTime > 0.5f)
            {
                lightOff();
            }

            if (alive && endCollider != null && endCollider.bounds.Contains(transform.position))
            {
                WinAnimation();
                return;
            }

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

            if (alive && RemainingTime <= 0f)
            {
                KillAnimationEnd("You ran out of time");
            }

            if (alive && Health <= 0f)
            {
                KillAnimationEnd("The abyss drained all your life");
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

        public void WinAnimation()
        {
            alive = false;
            killTime = Time.time;

            BlackScreenUI blackScreenUI = FindObjectOfType<BlackScreenUI>();
            blackScreenUI.Display(winMessages, winMessagesDurations, false, true, () => { SceneManager.LoadScene(MenuSceneName); return null; });

            fixeEverything();
        }

        public void KillAnimationEnd(string killReason)
        {
            alive = false;

            BlackScreenUI blackScreenUI = FindObjectOfType<BlackScreenUI>();
            blackScreenUI.Display(new List<string> { "Game Over", killReason }, new List<float> { 2f, 2f }, false, true, () => { SceneManager.LoadScene(MenuSceneName); return null; });

            DisableAllEnemies();
        }

        public void DisableAllEnemies()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController enemy in enemies)
            {
                enemy.DisableEnemy();
            }
        }

        private void fixeEverything()
        {
            FindObjectOfType<PlayerMovement>().enabled = false;
            FindObjectOfType<MouseLook>().enabled = false;
            FindObjectOfType<PlayerInteraction>().enabled = false;

            DisableAllEnemies();
        }

        private void lightOff()
        {
            Light[] lights = FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }
    }
}