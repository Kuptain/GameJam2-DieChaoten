﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WallsStrategy : ScriptableObject
{
   public abstract Wall[] GenerateWalls(ClusterSettings settings, RectInt bounds, int level);
}
