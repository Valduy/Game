using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using UnityEngine;
using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class MenuSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<MenuNode>().ToList())
            {
                
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

    }
}
