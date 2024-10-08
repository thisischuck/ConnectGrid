using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Emitter : GridElement
{
    public override bool Activated
    {
        get { return _activated; }
        set
        {
            if (value)
            {
                _activated = value;
                lineRenderer.color = Activated ? Color.cyan : Color.white;
                Emit();
            }
        }
    }

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