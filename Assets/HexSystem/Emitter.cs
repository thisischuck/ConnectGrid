using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Emitter : GridElement
{
    public override void Start()
    {
        gameObject.name = "Emitter";
        base.Start();
        Activated = true;

        InvokeRepeating("InvokeEmit", 0.1f, 0.1f);
    }

    public void InvokeEmit()
    {
        Activated = true;
        Emit();
    }

    public override void OnClick()
    {
        neighbors.ForEach(n => n.Activated = false);
        base.OnClick();
    }
}