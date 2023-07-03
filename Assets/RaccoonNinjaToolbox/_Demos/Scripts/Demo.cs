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
        [SerializeField, MinMaxFloatRange(1, 10)] private RangedFloat delayBetweenPrints;
        [SerializeField] private TextMeshProUGUI singleTagText;
        [SerializeField] private TextMeshProUGUI multipleTagsArrayText;
        [SerializeField] private TextMeshProUGUI multipleTagsListText;
        [SerializeField] private TextMeshProUGUI rangedIntWithDefaultValuesText;
        [SerializeField] private TextMeshProUGUI rangedIntWithDefaultMinMax10Text;
        [SerializeField] private TextMeshProUGUI rangedIntWithMin5Max15Text;
        [SerializeField] private TextMeshProUGUI countFromSingletonText;
        
        [Header("Readonly Info")]
        [SerializeField, InspectorReadOnly] private float currentDelay;
        [SerializeField, InspectorReadOnly] private int timesUpdated;
        [SerializeField, InspectorReadOnly] private int currentFramesPerSecond;

        [Header("Demo")]
        [SerializeField] private RangedInt rangedIntWithDefaultValues;
        [SerializeField, MinMaxIntRange(max: 10)] private RangedInt rangedIntWithDefaultMinMax10;
        [SerializeField, MinMaxIntRange(5, 15)] private RangedInt rangedIntWithMin5Max15;
        
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
            SingletonGameObject.Instance.IncrementCount();
            singleTagText.TrySetText($"Single tag: {singleTag}");
            multipleTagsArrayText.TrySetText($"Multiple tags array: {string.Join(", ", multipleTagsArray)}");
            multipleTagsListText.TrySetText($"Multiple tags list: {string.Join(", ", multipleTagsList)}");
            rangedIntWithDefaultValuesText.TrySetText($"RangedInt (default) (Min: {rangedIntWithDefaultValues.MinValue}/ Max: {rangedIntWithDefaultValues.MaxValue}): {rangedIntWithDefaultValues.Random()}");
            rangedIntWithDefaultMinMax10Text.TrySetText($"RangedInt (only max set) (Min: {rangedIntWithDefaultMinMax10.MinValue}/ Max: {rangedIntWithDefaultMinMax10.MaxValue}): {rangedIntWithDefaultMinMax10.Random()}");
            rangedIntWithMin5Max15Text.TrySetText($"RangedInt (min and max set) (Min: {rangedIntWithMin5Max15.MinValue}/ Max: {rangedIntWithMin5Max15.MaxValue}): {rangedIntWithMin5Max15.Random()}");
            countFromSingletonText.TrySetText($"Count from singleton: {SingletonGameObject.Instance.Count}");
            timesUpdated++;
        }
    }
}