using System;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    [SerializeField] bool showJson;
    [ShowIf("showJson"), TextArea(3, 8)] public string savedDataJson_int;
    [ShowIf("showJson"), TextArea(3, 8)] public string savedDataJson_string;
    [ShowIf("showJson"), TextArea(3, 8)] public string savedDataJson_inventory;
    [SerializeField] private Dictionary<string, int> SavedInt = new();
    [SerializeField] Dictionary<string, string> SavedString = new();
    public Dictionary<string, int> SavedInventory = new();
    [SerializeField, TextArea, Space(10)] string testConditions;


    public void SetVariable(string name, string value)
    {
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            SetInt(name, intValue);
        }
        else if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
        {
            Debug.LogWarning($"SetVariable: float values are not supported ('{name}' = '{value}'). Use int or string.");
        }
        else
        {
            SetString(name, value.Replace("\"", ""));
        }
    }

    public void SetInt(string key, int value = 0)
    {
        if (!SavedInt.ContainsKey(key)) SavedInt.Add(key, value);
        SavedInt[key] = value;
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        if (SavedInt.ContainsKey(key)) return SavedInt[key];
        SavedInt.Add(key, defaultValue);
        return defaultValue;
    }

    public void SetString(string key, string value)
    {
        if (!SavedString.ContainsKey(key)) SavedString.Add(key, value);
        SavedString[key] = value;
    }

    public string GetString(string key, string defaultValue = "")
    {
        if (SavedString.ContainsKey(key)) return SavedString[key];
        SavedString.Add(key, defaultValue);
        return defaultValue;
    }
    
    [Button(ButtonSizes.Large)]
    void TestConditions()
    {
        Debug.Log($"Test Conditions {testConditions} res {CheckConditions(testConditions)}");
    }
    
     public bool CheckConditions(string conditions, bool checkLog = false)
    {
        if (string.IsNullOrEmpty(conditions)) return true;

        // var splitConditions =  conditions.Split(new string[] { "&&" }, StringSplitOptions.None);
        string tempCond = conditions;
        if (tempCond.Contains(" && ")) tempCond = tempCond.Replace(" && ", ",");
        var splitConditions = tempCond.Split(',');
        for (int i = 0; i < splitConditions.Length; i++)
        {
            if (!CheckCondition(splitConditions[i])) return false;
        }

        return true;
    }

    public bool CheckCondition(string conditions)
    {
         Debug.Log($"CheckCondition {conditions}");
        if (conditions.Contains("NoCondition")) return true;
        if (conditions.Contains("Default")) return true;
     

        bool result = false;
        var targetVarName = conditions.Split(' ')[0];
        var comparer = conditions.Split(' ')[1];
        var desiredValue = conditions.Split(' ')[2];
        if (desiredValue.Contains("\""))
        {
            // Debug.Log($"Check [{conditions}] ");
            var currentValue = GetString(targetVarName);
            var targetValue = desiredValue.Replace("\"", "");
            result = currentValue.Equals(targetValue);
        }
        else if (int.TryParse(desiredValue, out var intValue))
        {
            var currentValue = GetInt(targetVarName);
            result = CompareInt((int)currentValue, comparer, intValue);
        }
        else
        {
            result = GetString(targetVarName) == desiredValue;
        }

        // if(result)
        // Debug.Log($"Check {targetVarName} {currentValue} {comparer} {checkValue} => {result}");
        return result;
    }

    bool CompareInt(int current, string comparer, int target)
    {
        switch (comparer)
        {
            case "==": return current == target;
            case ">": return current > target;
            case ">=": return current >= target;
            case "<=": return current <= target;
            case "<": return current < target;
            case "!=": return current != target;
            default:
                Debug.LogWarning($"Cant Process {comparer}");
                return false;
        }
    }
}
