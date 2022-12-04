using System;
using UnityEngine;

namespace Game.Controllers
{
    public class MistakeController : MonoBehaviour, IMistake
    {
        [SerializeField] private float mistakeIncreaserFactor = 10;
        [SerializeField] private float mistakeDecreaserFactor = 2;
        public float HealthBorder { get; set; }
        private float _healthMistakeCount = 0;

        public float HealthMistakeCount => _healthMistakeCount;
        
        private void Start()
        {
            HealthBorder = 3;
        }

        public void CharacterHasMadeMistake()
        {
            _healthMistakeCount += Time.deltaTime * mistakeIncreaserFactor;
        }
        private void Update()
        {
            _healthMistakeCount -= Time.deltaTime * mistakeDecreaserFactor;
            _healthMistakeCount = Mathf.Clamp(_healthMistakeCount, 0, HealthBorder);
        }
        
    }
}