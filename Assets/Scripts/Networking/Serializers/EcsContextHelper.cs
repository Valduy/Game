using Assets.Scripts.ECS.Components;
using ECS.Serialization.Contexts;

namespace Assets.Scripts.Networking.Serializers
{
    public static class EcsContextHelper
    {
        public static IEcsContext HostWorldContext { get; } = new EcsContext();
        public static IEcsContext ClientWorldContext { get; } = new EcsContext();

        static EcsContextHelper()
        {
            HostWorldContext.Register<IdComponent>();
            HostWorldContext.Register<VelocityComponent>();
            HostWorldContext.Register<PositionComponent>();
            HostWorldContext.Register<RotationComponent>();

            ClientWorldContext.Register<KeysComponent>();
            ClientWorldContext.Register<MouseComponent>();
            ClientWorldContext.Register<IdComponent>();
        }
    }
}
