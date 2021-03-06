﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RLTKTutorial.Part1_2
{
    [GenerateAuthoringComponent]
    public struct GenerateMap : IComponentData
    {
        public int iterationCount;
        public int2 playerPos;
        public int seed;

        public static GenerateMap Default => new GenerateMap
        {
            iterationCount = 100,
            playerPos = new int2(1,1),
            seed = 0
        };
    }
}