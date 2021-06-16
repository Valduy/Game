using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    public class EndGameComponent: ComponentBase
    {
        public GameObject GameOver;
        public GameObject Win;
    }
}
