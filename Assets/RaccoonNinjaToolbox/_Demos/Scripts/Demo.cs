using System.Collections;
using System.Collections.Generic;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using TMPro;
using UnityEngine;

namespace RaccoonNinjaToolbox._Demo.Scripts
{
    public class Demo : MonoBehaviour
    {
        [SerializeField, TagSelector] private string singleTag;
        [SerializeField, TagSelector] private string[] multipleTagsArray;
        [SerializeField, TagSelector] private List<string> multipleTagsList;
        
        [Header("Debug Controls")]
        [SerializeField] private RangedFloat delayBetweenPrints;
        [SerializeField] private TextMeshProUGUI singleTagText;
        [SerializeField] private TextMeshProUGUI multipleTagsArrayText;
        [SerializeField] private TextMeshProUGUI multipleTagsListText;
        
        [Header("Readonly Info")]
        [SerializeField, InspectorReadOnly] private float currentDelay;
        [SerializeField, InspectorReadOnly] private int timesUpdated;
        [SerializeField, InspectorReadOnly] private int currentFramesPerSecond;
        
        private void Start()
        {
            StartCoroutine(DebugTags());
        }

        private void Update()
        {
            // Calculate FPS - Just to have something to show in the Readonly inspector fields.
            currentFramesPerSecond = (int) (1f / Time.deltaTime);
        }
        
        private IEnumerator DebugTags()
        {
            while (true)
            {
                UpdateTextValues();
                currentDelay = delayBetweenPrints.Random();
                yield return new WaitForSeconds(currentDelay);
            }
        }

        private void UpdateTextValues()
        {
            singleTagText.TrySetText($"Single tag: {singleTag}");
            multipleTagsArrayText.TrySetText($"Multiple tags array: {string.Join(", ", multipleTagsArray)}");
            multipleTagsListText.TrySetText($"Multiple tags list: {string.Join(", ", multipleTagsList)}");
            timesUpdated++;
        }
    }
}