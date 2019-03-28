using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
    void CollisionOnWallsAndEnemies();
}

public interface IWandering
{
    bool isWandering { get; set; }

    IEnumerator Wander();
}
