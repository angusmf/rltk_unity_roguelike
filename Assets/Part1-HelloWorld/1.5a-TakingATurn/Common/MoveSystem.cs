﻿
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace RLTKTutorial.Part1_5A
{
    [DisableAutoCreation]
    public class MoveSystem : SystemBase
    {
        EntityQuery _mapQuery;
        EntityQuery _moveQuery;
        
        protected override void OnCreate()
        {
            _mapQuery = GetEntityQuery(
                ComponentType.ReadWrite<MapTiles>(),
                ComponentType.ReadOnly<MapData>()
                );

            _moveQuery = GetEntityQuery(
                ComponentType.ReadWrite<Position>(),
                ComponentType.ReadOnly<Movement>()
                );
            
            _moveQuery.AddChangedVersionFilter(typeof(Movement));
        }

        //protected override JobHandle OnUpdate(JobHandle inputDeps)
        protected override void OnUpdate()
        {
            if (_moveQuery.CalculateEntityCount() == 0)
                return;

            var mapEntity = _mapQuery.GetSingletonEntity();
            var map = EntityManager.GetBuffer<MapTiles>(mapEntity);
            var mapData = EntityManager.GetComponentData<MapData>(mapEntity);

            var buffer = new EntityCommandBuffer(Allocator.Temp);

            var nameFromEntity = GetComponentDataFromEntity<Name>(true);
            
            Entities
                //.WithReadOnly(nameFromEntity)
                .WithReadOnly(map)
                //.WithoutBurst()
                .WithAll<Actor>()
                .WithAll<TakingATurn>()
                .WithNone<ActionPerformed>()
                .ForEach((int entityInQueryIndex, Entity e, ref Position p, ref Movement move) =>
            {

                if (move.value.x == 0 && move.value.y == 0)
                    return;

                int2 dest = p.value + move.value;
                int index = dest.y * mapData.width + dest.x;

                //if (math.lengthsq(move.value) != 0 && nameFromEntity.HasComponent(e)) Debug.Log($"{nameFromEntity[e].ToString()} moved");

                move = int2.zero;

                var performed = new ActionPerformed { cost = 100 };
                buffer.AddComponent(e, performed);

                if (index < 0 || index >= map.Length)
                    return;

                if( map[index] != TileType.Wall )
                    p = dest;
            }).Run();

            buffer.Playback(EntityManager);
        }
    }
}