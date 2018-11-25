using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EffectorType
{
    ARC,
    BLIND,
    BOUNCE,
    CONDUCTIVE,
    COOL,
    DAMAGE,
    DRAIN,
    DOT,
    EXPAND,
    FEAR,
    FRENZY,
    GLUE,
    HEAL,
    HEAT,
    HOMING,
    HOT,
    JUXTAPOSE,
    LEAK,
    LIFESTEAL,
    MAGNETIC,
    PIERCE,
    PIN,
    PULL,
    PUSH,
    RETRACT,
    REVEAL,
    RICOCHET,
    SHARDS,
    SHRINK,
    SLIPPERY,
    SLOW,
    SPAWN,
    SPLIT,
    STICK,
    STUN,
    TRACK,
    TRANSFORM
}

public class Effector : MonoBehaviour
{
    protected UnitStats unitStats;
    protected Ammo ammo;

    void Start()
    {
        unitStats = GetComponent<UnitStats>();
        ammo = GetComponent<Ammo>();
    }
}