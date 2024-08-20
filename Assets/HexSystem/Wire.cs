using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Wire : GridElement
{
    public override void Start()
    {
        gameObject.name = "Wire";
        base.Start();
        Activated = false;

        InvokeRepeating("InvokeEmit", 0.1f, 0.1f);
    }

    public void InvokeEmit()
    {
        Emit();
    }

    public override void OnClick()
    {
        neighbors.ForEach(n => n.Activated = false);
        base.OnClick();
    }
}