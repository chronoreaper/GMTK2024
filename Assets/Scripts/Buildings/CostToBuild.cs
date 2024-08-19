using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CostToBuild
{
    public BuildTypes Building;
    public ResourceCost[] Cost;

    public enum BuildTypes
    {
        Miner,
        Turret,
        Ship
    }

    [Serializable]
    public class ResourceCost
    {
        public ResourceTypes Type;
        public int Amount;
    }
}
