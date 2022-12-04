using System;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.Enums;
using Game.Events;

namespace Game.Core
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private RectTransform arrowTransform;
        [HideInInspector] public KeySequencer sequencer;
        
        private RectTransform successArea;
        private Vector3 endPoint;
        private Color initialColor;
        
        private KeyDirection direction;

        private bool isPressed;
        private bool isSuccessful;
        private bool isReachedFailDistance;

        private float moveSpeed;
        
        
        public  RectTransform RectTransform { get; private set; }
        public bool IsOffTheScreen => RectTransform.position.x + RectTransform.sizeDelta.x - endPoint.x < 0f;
        public float HalfOfTheInteractableLength => (successArea.sizeDelta.x + RectTransform.sizeDelta.x) / 2;
        public bool IsReachedFailDistance => (successArea.position.x - RectTransform.position.x) > HalfOfTheInteractableLength;
        public bool IsMissedInput => !isSuccessful && IsReachedFailDistance;
        public bool IsInSuccessArea => Mathf.Abs(successArea.position.x - RectTransform.position.x) <= HalfOfTheInteractableLength;
        
        
        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            initialColor = backgroundImage.color;
        }

        private void Update()
        {
            UpdatePosition();
            CheckPosition();
        }

        public void Init(KeyDirection direction, float moveSpeed, RectTransform successArea, Vector3 endPoint)
        {
            ResetToDefaults();
            SetDirection(direction);
            
            this.moveSpeed = moveSpeed;
            this.successArea = successArea;
            this.endPoint = endPoint;
        }

        private void ResetToDefaults()
        {
            isPressed = false;
            isSuccessful = false;
            isReachedFailDistance = false;
        }

        private void SetDirection(KeyDirection direction)
        {
            this.direction = direction;
            arrowTransform.rotation = Quaternion.Euler(0f, 0f, ((int)direction - 1) * -90f);
        }

        public void UpdatePosition()
        {
            Vector3 movement = (Vector3.left * moveSpeed * Time.deltaTime);
            float screenWidthFactor = (Screen.width / 1000f);
            
            transform.position +=  movement * screenWidthFactor;
        }

        private void CheckPosition()
        {
            // Control if the arrow is in the square indicator.
            CheckPositionRules(IsInSuccessArea, ListenKeyPress);

            // Check if the arrow passed the indicator area.
            CheckPositionRules(IsMissedInput, HandleMissedInput);

            // Check if the arrow is off the screen.
            CheckPositionRules(IsOffTheScreen, HandleOffScreen);
        }
        
        private void HandleCorrectInput()
        {
            isSuccessful = true;
            backgroundImage.color = Color.green;
            EventManager.SuccessfulMove?.Invoke();
        }

        private void HandleMissedInput()
        {
            EventManager.MissedInput?.Invoke();
        }

        private void HandleOffScreen()
        {
            backgroundImage.color = initialColor;

            sequencer.KeyPool.Push(this);
        }

        private void ListenKeyPress()
        {
            if (isPressed)
            {
                return;
            }
            
            if (direction == KeyDirection.Up    && Input.GetKeyDown(KeyCode.UpArrow)    ||
                direction == KeyDirection.Right && Input.GetKeyDown(KeyCode.RightArrow) ||
                direction == KeyDirection.Down  && Input.GetKeyDown(KeyCode.DownArrow)  ||
                direction == KeyDirection.Left  && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isPressed = true;
                HandleCorrectInput();
            }
            
            else if (direction != KeyDirection.Up && Input.GetKeyDown(KeyCode.UpArrow) ||
                direction != KeyDirection.Right && Input.GetKeyDown(KeyCode.RightArrow) || 
                direction != KeyDirection.Down  && Input.GetKeyDown(KeyCode.DownArrow) ||
                direction != KeyDirection.Left  && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isPressed = true;
                HandleMissedInput();    
            }
        }

        private void CheckPositionRules(bool condition, Action calledFunction)
        {
            if (condition) calledFunction.Invoke();
        }
    }
}