using System.Collections.Generic;
using UnityEngine;

namespace Constructors
{
    public static class GenerateSceneExtensions
    {
        public static IConstructible[] GetObjects(GameObject GatePrefab, GameObject ScoreboardPrefab) =>
            new IConstructible[]
            {
                new PlayerContructor(),
                new FieldConstructor(GatePrefab, ScoreboardPrefab),
                new BallConstructor()
            };
    }
}