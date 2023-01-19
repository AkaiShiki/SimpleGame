using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreDisplay;
    [SerializeField] IntVariable  _currentScore;
    private int ScoreUpdate = 0;

    void Start()
    {
         ScoreUpdate= _currentScore.Value; 
         _scoreDisplay.text = _currentScore.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentScore.Value != ScoreUpdate)
        {
           ScoreUpdate= _currentScore.Value; 
         _scoreDisplay.text = _currentScore.Value.ToString();
        }
        
     
    }
}
