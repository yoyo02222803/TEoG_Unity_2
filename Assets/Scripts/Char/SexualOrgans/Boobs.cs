﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Boobs : SexualOrgan
{
    public Boobs() : base()
    {
        _fluid = new SexualFluid(FluidType.Milk, Size);
    }

    public Boobs(int size) : base(size)
    {
        _fluid = new SexualFluid(FluidType.Milk, Size);
    }

    [SerializeField]
    private SexualFluid _fluid;

    public virtual SexualFluid Fluid
    {
        get
        {
            if (baseSize != lastBase)
            {
                _fluid.FluidCalc(Size);
            }
            return _fluid;
        }
    }

    public string Looks()
    {
        string boobs = "";
        boobs += $" {Fluid.Current}";
        return boobs;
    }
}

public static class BoobExtensions
{
    public static void AddBoobs(this List<Boobs> boobs) => boobs.Add(new Boobs());

    public static void AddBoobs(this List<Boobs> boobs, int parSize) => boobs.Add(new Boobs(parSize));

    public static float Cost(this List<Boobs> boobs) => Mathf.Round(30 * Mathf.Pow(4, boobs.Count));

    public static float ReCycle(this List<Boobs> boobs)
    {
        Boobs toShrink = boobs[boobs.Count - 1];
        if (toShrink.Shrink())
        {
            boobs.Remove(toShrink);
            return 30f;
        }
        return toShrink.Cost;
    }

    public static float Milking(this List<Boobs> boobs) => boobs.Sum(b => b.Fluid.DisCharge());

    public static float Milking(this List<Boobs> boobs, float dischargePrecentage) => boobs.Sum(b => b.Fluid.DisCharge(dischargePrecentage));

    public static float MilkTotal(this List<Boobs> boobs) => boobs.Select(b => b.Fluid.Current).DefaultIfEmpty(0).Sum();

    public static float MilkMax(this List<Boobs> boobs) => boobs.Select(b => b.Fluid.Current).DefaultIfEmpty(0).Sum();
}