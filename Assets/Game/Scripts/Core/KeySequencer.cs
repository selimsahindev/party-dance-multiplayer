using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Enums;

namespace Game.Core
{
    public class KeySequencer : MonoBehaviour
    {
        [SerializeField] private float keyMoveSpeed;
        [SerializeField] private float timeBetweenKeys;
        [SerializeField] private RectTransform successPoint;
        [SerializeField] private RectTransform endPoint;
        [Header("Prefabs")]
        [SerializeField] private Key keyPrefab;

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

            KeyDirection direction = (KeyDirection)Random.Range(0, (int)KeyDirection.Count);

            key.Init(direction, keyMoveSpeed, successPoint, endPoint.position);

            key.RectTransform.anchoredPosition = new Vector3(100f, 0f, 0f);
            key.gameObject.SetActive(true);
        }
    }
}
