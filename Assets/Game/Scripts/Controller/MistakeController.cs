using System;
using Game.Core;
using TMPro;
using UnityEngine;

namespace Game.Controllers
{
    public class MistakeController : MonoBehaviour, IMistake
    {
        [SerializeField] private float mistakeIncreaserFactor = 10;
        [SerializeField] private float mistakeDecreaserFactor = 2;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private KeySequencer keySequencer;
        [SerializeField] private TextMeshProUGUI loseText;
        
        public float HealthBorder { get; set; }
        private float _healthMistakeCount = 0;

        public System.Action OnEliminated;
        
        public float HealthMistakeCount => _healthMistakeCount;
        public PlayerController PlayerController => playerController;
        
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

            Debug.Log("Health Mistake Count: " + _healthMistakeCount);
            EliminatedPlayer();
        }

        private void EliminatedPlayer()
        {
            if (_healthMistakeCount == HealthBorder)
            {
                playerController.CanDance = false;
                keySequencer.gameObject.SetActive(false);
                loseText.gameObject.SetActive(true);
                OnEliminated?.Invoke();
            }            
        }
        
    }
}