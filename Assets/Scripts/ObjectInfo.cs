using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    Tooltip tooltip;
    [SerializeField]
    private string tooltipText;
    public string TooltipText { get { return tooltipText; } set { tooltipText = value; } }

    private void Awake()
    {
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }

    private void OnMouseEnter()
    {
        tooltip.Show(TooltipText);
    }

    private void OnMouseExit()
    {
        tooltip.Hide();
    }
}
