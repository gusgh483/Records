using System;
using UnityEngine;


public partial class Player : Character
{
#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        dataFile = "Kachujin";
    }
#endif

    protected override void Awake()
    {
        base.Awake();

        Awake_BindInput();
    }

    protected override void Start()
    {
        base.Start();

        Move();
        bFacingRight = true;
    }

    protected override void OnGUI()
    {
        //GUILayout.Label(inputMove.ToString());
    }
}
