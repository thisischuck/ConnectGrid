using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Placeholder : GridElement
{
    public int Level;
    public TextMeshProUGUI textLevel;

    public override bool Activated
    {
        get { return _activated; }
    }

    public override void Start()
    {
        base.Start();
        textLevel = GetComponentInChildren<TextMeshProUGUI>();
        textLevel.text = Level.ToString();
    }

    protected override void OnEnable()
    {
        if (Level > GameManager.Instance.MaxLevel)
        {
            DisableInput();
        }
        else
            EnableInput();
    }

    public override void Emit()
    {
        return;
    }

    public override void Receive(GridElement emiter)
    {
        return;
    }

    public override void OnClick()
    {
        GameManager.Instance.LoadLevel(Level);
        return;
    }
}