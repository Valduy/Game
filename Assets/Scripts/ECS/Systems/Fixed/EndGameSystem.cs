using System.Linq;
using ECS.Core;

using UnityEngine.SceneManagement;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.ECS.Nodes;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class EndGameSystem : SystemBase
    {
        public class NodeCharacter : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public PlayerFlagComponent PlayerFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                PlayerFlagComponent = Entity.Get<PlayerFlagComponent>();
            }
        }

        public class NodeEnemies : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public EnemyFlagComponent EnemyFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                EnemyFlagComponent = Entity.Get<EnemyFlagComponent>();
            }
        }

        public class MenuTimerNode : NodeBase
        {
            public EndGameComponent EndGameComponent { get; private set; }
            public MenuComponent MenuComponent { get; private set; }
            public KeysComponent KeysComponent { get; private set; }
            public TimerComponent TimerComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                EndGameComponent = Entity.Get<EndGameComponent>();
                MenuComponent = Entity.Get<MenuComponent>();
                KeysComponent = Entity.Get<KeysComponent>();
                TimerComponent = Entity.Get<TimerComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            if (_engine.GetNodes<NodeEnemies>().ToList().All(enemy => enemy.HealthComponent.CurrentHealth <= 0))
            {
                foreach (var node in _engine.GetNodes<MenuNode>().ToList())
                {
                    node.EndGameComponent.Win.SetActive(true);
                    node.Entity.Add(new TimerComponent());

                }
            }
            else if (_engine.GetNodes<NodeCharacter>().ToList().All(player => player.HealthComponent.CurrentHealth <= 0))
            {
                foreach (var node in _engine.GetNodes<MenuNode>().ToList())
                {
                    node.EndGameComponent.GameOver.SetActive(true);
                    node.Entity.Add(new TimerComponent());

                }
            }

            foreach (var node in _engine.GetNodes<MenuTimerNode>())
            {
                node.TimerComponent.TimeLeft += time;

                if (node.TimerComponent.TimeLeft > 10)
                {
                    LoadingData.NextScene = "MainMenu";
                    SceneManager.LoadScene("Loading");
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
