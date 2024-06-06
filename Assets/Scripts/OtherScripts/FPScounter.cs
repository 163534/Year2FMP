using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMesh Pro namespace

public class FPScounter : MonoBehaviour
{
    // Reference to the TextMesh Pro UI component
    public TMP_Text Text;

    private Dictionary<int, string> CachedNumberStrings = new Dictionary<int, string>();
    private int[] _frameRateSamples;
    private int _cacheNumbersAmount = 300;
    private int _averageFromAmount = 30;
    private int _averageCounter = 0;
    private int _currentAveraged;

    void Awake()
    {
        // Cache strings and create array
        {
            for (int i = 0; i < _cacheNumbersAmount; i++)
            {
                CachedNumberStrings[i] = i.ToString();
            }
            _frameRateSamples = new int[_averageFromAmount];
        }
    }

    void Update()
    {
        {
            var currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime);
            _frameRateSamples[_averageCounter] = currentFrame;
        }

        {
            var average = 0f;

            foreach (var frameRate in _frameRateSamples)
            {
                average += frameRate;
            }

            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;
        }

        // Assign to UI
        {
            // Use a switch expression to determine the FPS display
            Text.text = _currentAveraged switch
            {
                var x when x >= 0 && x < _cacheNumbersAmount => CachedNumberStrings[x],
                var x when x >= _cacheNumbersAmount => $"> {_cacheNumbersAmount}",
                var x when x < 0 => "< 0",
                _ => "?"
            };
        }
    }
}
