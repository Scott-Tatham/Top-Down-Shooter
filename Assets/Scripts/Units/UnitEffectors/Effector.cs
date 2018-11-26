using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EffectorType
{
    ARC = 1 << 0,
    BLIND = 1 << 1,
    BOUNCE = 1 << 2,
    CONDUCTIVE = 1 << 3,
    COOL = 1 << 4,
    DAMAGE = 1 << 5,
    DRAIN = 1 << 6,
    DOT = 1 << 7,
    EXPAND = 1 << 8,
    FEAR = 1 << 9,
    FRENZY = 1 << 10,
    GLUE = 1 << 11,
    HEAL = 1 << 12,
    HEAT = 1 << 13,
    HOMING = 1 << 14,
    HOT = 1 << 15,
    JUXTAPOSE = 1 << 16,
    LEAK = 1 << 17,
    LIFESTEAL = 1 << 18,
    MAGNETIC = 1 << 19,
    PIERCE = 1 << 20,
    PIN = 1 << 21,
    PULL = 1 << 22,
    PUSH = 1 << 23,
    RETRACT = 1 << 24,
    REVEAL = 1 << 25,
    RICOCHET = 1 << 26,
    SHARDS = 1 << 27,
    SHRINK = 1 << 28,
    SLIPPERY = 1 << 29,
    SLOW = 1 << 30,
    SPAWN = 1 << 31//,
    //SPLIT = 1 << 33,
    //STICK = 1 << 34,
    //STUN = 1 << 35,
    //TRACK = 1 << 36,
    //TRANSFORM = 1 << 37
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