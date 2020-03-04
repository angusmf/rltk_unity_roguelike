using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace RLTKTutorial.Part1_5A
{
    [DisableAutoCreation]
    public class MonsterTurnSystem : TurnActionSystem
	{
		public override ActorType ActorType => ActorType.Monster;

        EntityQuery _playerQuery;
        EntityQuery _mapQuery;

        Random _rand;

        Entity _playerEntity;
        MapData _mapData;

        MoveSystem _moveSystem;

        bool _dontRun;
        
        protected override void OnCreate()
        {
            base.OnCreate();

            _playerQuery = GetEntityQuery(
                ComponentType.ReadOnly<Player>(),
                ComponentType.ReadOnly<Position>(),
                ComponentType.ReadOnly<Name>()
                );

            _mapQuery = GetEntityQuery(
                ComponentType.ReadOnly<MapData>()
                );

            _rand = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));
        }

        public override void OnFrameBegin()
        {
            var mapEntity = GetSingletonEntity<MapData>();

            _moveSystem = World.GetOrCreateSystem<MoveSystem>();

            // Early out if the map is regenerating.
            if (EntityManager.HasComponent<GenerateMap>(mapEntity)
             || _playerQuery.IsEmptyIgnoreFilter)
            {
                _dontRun = true;
                return;
            }

            _mapData = EntityManager.GetComponentData<MapData>(mapEntity);
            _playerEntity = _playerQuery.GetSingletonEntity();

            _dontRun = false;
        }

        protected override int OnTakeTurn(Entity e)
        {

            var mapEntity = _mapQuery.GetSingletonEntity();
            var mapData = EntityManager.GetComponentData<MapData>(mapEntity);

            var view = EntityManager.GetBuffer<TilesInView>(e);
            var playerEntity = _playerQuery.GetSingletonEntity();
            var playerPos = (int2)EntityManager.GetComponentData<Position>(playerEntity);
            var playerName = EntityManager.GetComponentData<Name>(playerEntity);
            int playerIndex = playerPos.y * mapData.width + playerPos.x;

            var name = EntityManager.GetComponentData<Name>(e);
            var welcomeText = EntityManager.GetComponentData<WelcomeText>(e);

            if (view[playerIndex])
            {
                Debug.Log($"{name} {welcomeText} {playerName}");
            }

            var dir = GetRandomDirection(ref _rand);

            _moveSystem.TryMove(e, dir);

            return Energy.ActionThreshold;
        }


        static int2 GetRandomDirection(ref Random rand)
        {
            int i = rand.NextInt(0, 5);
            switch (i)
            {
                case 0: return new int2(-1, 0);
                case 1: return new int2(1, 0);
                case 2: return new int2(0, -1);
                case 3: return new int2(0, 1);
            }
            return default;
        }
    }

}