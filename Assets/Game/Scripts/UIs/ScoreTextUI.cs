using System;
using System.Collections;
using Game.Core;
using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private TextMeshProUGUI _scoreText;
    
    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _scoreText.text = "Your Score: " + player.Score.ToString();
        StartCoroutine(UpdateScale());
    }

    private IEnumerator UpdateScale()
    {

        if (!Input.anyKey)
        {
            _scoreText.transform.localScale = Vector3.Lerp
            (
                _scoreText.transform.localScale,
                new Vector3(0.75f, 0.75f, 0.75f),
                Time.deltaTime * 5
            );
        }
        
        yield return new WaitForSeconds(2);

        if (Input.anyKey)
        {
            _scoreText.transform.localScale = Vector3.Lerp
            (
                _scoreText.transform.localScale,
                new Vector3(2f, 2f, 2f),
                Time.deltaTime * 5
            );
        }
        
        StartCoroutine(UpdateScale());
    }
}