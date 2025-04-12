using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;


public class ControlsDropdown : MonoBehaviour
{
    private void Awake()
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keycode.ToString().Contains("Joy") == false)
            {
                GetComponent<TMPro.TMP_Dropdown>().options.Add(new TMPro.TMP_Dropdown.OptionData() { text = keycode.ToString() });
            }

        }

        

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
