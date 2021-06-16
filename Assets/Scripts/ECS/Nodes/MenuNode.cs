using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using UnityEngine;
using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Nodes
{
    public class MenuNode : NodeBase
    {
        public EndGameComponent EndGameComponent { get; private set; }
        public MenuComponent MenuComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            EndGameComponent = Entity.Get<EndGameComponent>();
            MenuComponent = Entity.Get<MenuComponent>();
        }
    }
}
