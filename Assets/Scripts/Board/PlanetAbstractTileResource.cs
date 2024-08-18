using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Board;

[Serializable]
public class PlanetAbstractTileResource
{
    public PlanetType Type;
    public RuleTile BaseTile;
}
