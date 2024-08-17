using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBaseBuilding : MonoBehaviour
{
    public abstract BuildingType Type { get; protected set; }
}
