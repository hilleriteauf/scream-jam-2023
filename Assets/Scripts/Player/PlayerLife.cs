using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLife : MonoBehaviour
    {

        public float lifeExpectancyOutsideSafezone = 60f * 5f;

        public float life { get; private set; } = 1f;

        private PlayerSafezoneDetector safezoneDetector;

        void Awake()
        {
            safezoneDetector = FindObjectOfType<PlayerSafezoneDetector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (safezoneDetector.IsInSafezone)
            {
                life = 1f;
            } else
            {
                life -= Time.deltaTime / lifeExpectancyOutsideSafezone;
            }
        }
    }
}