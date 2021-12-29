using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class PercentInputValidation : MonoBehaviour
{
    [SerializeField] private TMP_InputField field;

    private void Awake()
    {
        field = GetComponentInChildren<TMP_InputField>();    
    }

    public void ValidatePercent()
    {
        Regex rx = new Regex(@"^[0-9]+$");
        var matches = rx.Matches(field.text);
        if (matches.Count > 0)
        {
            float value = float.Parse(field.text);
            if (value > 100) field.text = "100";
            if (value < 0) field.text = "0";
        }
        else
        {
            field.text = "0";
        }
    }
}
