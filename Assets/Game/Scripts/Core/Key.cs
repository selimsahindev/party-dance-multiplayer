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
        public RectTransform RectTransform { get; private set; }
        private bool isSuccessful;
        private float moveSpeed;
        private KeyDirection direction;
        private RectTransform successArea;
        private Vector3 endPoint;
        private Color initialColor;

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
            SetDirection(direction);
            this.moveSpeed = moveSpeed;
            this.successArea = successArea;
            this.endPoint = endPoint;
        }

        private void SetDirection(KeyDirection direction)
        {
            this.direction = direction;
            arrowTransform.rotation = Quaternion.Euler(0f, 0f, ((int)direction - 1) * -90f);
        }

        public void UpdatePosition()
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        private void CheckPosition()
        {
            float halfOfTheInteractableLength = (successArea.sizeDelta.x + RectTransform.sizeDelta.x) / 2;

            // Control if the arrow is in the square indicator.
            bool isInTheSuccessArea = Mathf.Abs(successArea.position.x - RectTransform.position.x) <= halfOfTheInteractableLength;
            if (isInTheSuccessArea)
            {
                ListenKeyPress();
            }

            // Check if the arrow passed the indicator area.
            bool isReachedFailDistance = (successArea.position.x - RectTransform.position.x) > halfOfTheInteractableLength;
            if (!isSuccessful && isReachedFailDistance)
            {
                HandleMissedInput();
            }

            // Check if the arrow is off the screen.
            bool isOffTheScreen = RectTransform.position.x + RectTransform.sizeDelta.x - endPoint.x < 0f;
            if (isOffTheScreen)
            {
                HandleOffScreen();
            }
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
            if (direction == KeyDirection.Up    && Input.GetKeyDown(KeyCode.UpArrow)    ||
                direction == KeyDirection.Right && Input.GetKeyDown(KeyCode.RightArrow) ||
                direction == KeyDirection.Down  && Input.GetKeyDown(KeyCode.DownArrow)  ||
                direction == KeyDirection.Left  && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HandleCorrectInput();
            }
        }
    }
}
