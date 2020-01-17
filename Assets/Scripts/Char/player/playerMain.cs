﻿using UnityEngine;

public class PlayerMain : BasicChar
{
    public PlayerMain() : base()
    {
        thisPlayer = this;
    }

    // public Settings sett;
    public override void Awake()
    {
        if (thisPlayer == null)
        {
            thisPlayer = this;
        }
        else if (thisPlayer != this)
        {
            Destroy(gameObject);
        }
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        RaceSystem.AddRace(Races.Human, 100);
        body = new Body(160, 10, 20);
        Inventory.AddItem(ItemId.Pouch);
        for (int i = 0; i < 40; i++)
        {
            Inventory.AddItem(ItemId.Stick);
        }
        Essence.Masc.Gain(19999);
        Essence.Femi.Gain(9999);
        Currency.Gold += 100;
    }

    public void PlayerInit(string first, string last)
    {
        Identity.FirstName = first;
        Identity.LastName = last;
    }

    private static PlayerMain thisPlayer;

    public static PlayerMain GetPlayer
    {
        get
        {
            if (thisPlayer == null)
            {
                // Something called Getplayer before player could awake
                Debug.Log("Getplayer was null");
                thisPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
            }
            if (Debug.isDebugBuild)
            {
                //  Debug.Log(new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType + " missed playermain");
            }
            return thisPlayer;
        }
    }
}