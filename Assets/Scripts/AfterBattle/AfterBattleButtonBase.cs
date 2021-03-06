﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AfterBattleButtonBase : MonoBehaviour
{
    [SerializeField] protected Button btn = null;

    [SerializeField] protected TextMeshProUGUI title = null;

    public virtual void Start()
    {
        btn = btn != null ? btn : GetComponent<Button>();
    }
}