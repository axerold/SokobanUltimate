using System.Collections.Generic;
using System.Data;
using System.Linq;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Levels;

public class Cell
{
    public IEntity Landlord { get; }
    public List<IEntity> Tenants { get; }
    public bool DeadZone { get; set; }

    public Cell(IEntity entity, IntVector2 location)
    {
        if (IsLandlord(entity))
        {
            Landlord = entity;
            Tenants = [];
        }
        else
        {
            Landlord = new Space(location);
            Tenants = [entity];
        }

        DeadZone = false;
    }

    public void AddTenant(IEntity tenant)
    {
        if (Landlord is Wall)
            throw new ConstraintException("No tenants may live upon the wall");
        Tenants.Add(tenant);
    }

    public void RemoveTenant(IEntity tenant)
    {
        Tenants.Remove(tenant);
    }

    public IEntity GetLastTenant() => Tenants.LastOrDefault();
    public static bool IsLandlord(IEntity entity) => entity is Wall or Space or BoxCollector;

    public bool FreeToMove() => Landlord is BoxCollector || (Landlord is Space && Tenants.LastOrDefault() is not Box);
}