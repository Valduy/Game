using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ECS.Core;

using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class VirtualMouseToMouseConverterSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public VirtualMouseComponent VirtualMouseComponent { get; private set; }
            public VirtualMouseRadiusComponent VirtualMouseRadiusComponent { get; private set; }
            public MouseComponent MouseComponent { get; private set; }
            public IsEnemyWeaponComponent IsEnemyWeaponComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                VirtualMouseComponent = Entity.Get<VirtualMouseComponent>();
                VirtualMouseRadiusComponent = Entity.Get<VirtualMouseRadiusComponent>();
                MouseComponent = Entity.Get<MouseComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
            }
        }

        Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                var radius = node.VirtualMouseRadiusComponent.Radius;
                var mouseCoords = new Vector2(radius * (float)Math.Cos(Math.PI * node.VirtualMouseComponent.Angle / 180.0),
                    radius * (float)Math.Sin(Math.PI * node.VirtualMouseComponent.Angle / 180.0));

                node.MouseComponent.MousePosition = mouseCoords;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
