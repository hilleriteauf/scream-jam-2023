using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLife : MonoBehaviour
    {

        public float lifeExpectancyOutsideSafezone = 60f * 5f;

        public float timerDuration = 60f * 25;
        public float timerStartTime = 0f;

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
            if (safezoneDetector.IsInSafezone)
            {
                Health = 1f;
            } else
            {
                Health -= Time.deltaTime / lifeExpectancyOutsideSafezone;
            }
        }
    }
}