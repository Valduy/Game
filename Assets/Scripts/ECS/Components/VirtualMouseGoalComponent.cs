using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;

    public class VirtualMouseGoalComponent: ComponentBase
    {
        public float Angle = -1;
        public int Tendention = 0;
        public int CurrentStage = 0;
    }
