using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Enums;
using Game.ScriptableObjects;

namespace Game.Core
{
    public class KeySequencer : MonoBehaviour
    {   
        [SerializeField] private float keyMoveSpeed;
        [SerializeField] private RectTransform successPoint;
        [SerializeField] private RectTransform endPoint;
        [SerializeField] private KeyDirection direction; 
        [SerializeField] private float timeBetweenKeys;
        
        private Key keyPrefab;

        public KeyDirection Direction => direction;

        public ObjectPool<Key> KeyPool { get; private set; }

        private void Awake()
        {
            PopulateArrowPool();
        }

        private void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private void PopulateArrowPool()
        {
            KeyPool = new ObjectPool<Key>(
                12,
                CreateFunction: () => {
                    switch (direction)
                    {
                        case KeyDirection.Up:
                            direction = KeyDirection.Up;
                            keyPrefab = Resources.Load<Key>("KeyUp");
                            break; 
                        case KeyDirection.Down:
                            direction = KeyDirection.Down;
                            keyPrefab = Resources.Load<Key>("KeyDown");
                            break;
                        case KeyDirection.Left:
                            direction = KeyDirection.Left;
                            keyPrefab = Resources.Load<Key>("KeyLeft");
                            break;
                        case KeyDirection.Right:
                            direction = KeyDirection.Right;
                            keyPrefab = Resources.Load<Key>("KeyRight");
                            break;
                    }
                    
                    Key key = Instantiate(keyPrefab, transform);
                    key.sequencer = this;

                    key.RectTransform.anchorMin = new Vector2(1f, 0.5f);
                    key.RectTransform.anchorMax = new Vector2(1f, 0.5f);
                    key.RectTransform.anchoredPosition = new Vector2(key.RectTransform.sizeDelta.x / 2, 0f);

                    return key;
                },
                OnPush: pushItem => {
                    pushItem.gameObject.SetActive(false);
                }
            );
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                SpawnRandomKey();
                yield return new WaitForSeconds(timeBetweenKeys);
            }
        }

        private void SpawnRandomKey()
        {
            Key key = KeyPool.Pop();

            key.Init(keyMoveSpeed, successPoint, endPoint.position);

            key.RectTransform.anchoredPosition = new Vector3(100f, 0f, 0f);
            key.gameObject.SetActive(true);
        }
    }
}
