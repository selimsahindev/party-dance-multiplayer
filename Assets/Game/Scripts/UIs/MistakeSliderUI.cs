using System;
using Game.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MistakeSliderUI : MonoBehaviour
    {
        private Slider _mistakeSlider;
        private MistakeController mistakeController;

        public Slider MistakeSlider => _mistakeSlider;

        private void Awake()
        {
            _mistakeSlider = GetComponent<Slider>();
            mistakeController = FindObjectOfType<MistakeController>();
        }

        private void OnEnable()
        {
            mistakeController.OnEliminated += HandleOnEliminated;
        }

        private void Update()
        {
            //The mistake count in here will come from later on server side
            _mistakeSlider.value = mistakeController.HealthMistakeCount;
        }

        private void OnDisable()
        {
            mistakeController.OnEliminated -= HandleOnEliminated;
        }

        public void HandleOnEliminated()
        {
            _mistakeSlider.value = 0;
        }
    }
}