﻿using System.Linq;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupBase : MonoBehaviour
    {
        protected Engine Unfixed { get; private set; }
        protected Engine Fixed { get; private set; }

        protected virtual void Start()
        {
            Unfixed = new Engine();
            Fixed = new Engine();
        }

        protected virtual void Update()
        {
            Unfixed.Update(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            Fixed.Update(Time.fixedDeltaTime);
        }

        protected void AddUnfixedSystems(params SystemBase[] systems) => AddSystems(Unfixed, systems);

        protected void AddFixedSystems(params SystemBase[] systems) => AddSystems(Fixed, systems);

        private void AddSystems(Engine engine, params SystemBase[] systems)
        {
            uint priority = 0;

            if (engine.GetSystems().Any())
            {
                priority = engine.GetSystems().Max(s => s.Priority);
            }

            foreach (var system in systems)
            {
                engine.AddSystem(system, priority);
                priority++;
            }
        }
    }
}
