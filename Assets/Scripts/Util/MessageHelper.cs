using System.Collections.Generic;
using System.Text;
using ECS.Core;
using ECS.Serialization;
using ECS.Serialization.Contexts;

namespace Assets.Scripts.Util
{
    public static class MessageHelper
    {
        private static readonly WorldSerializer WorldSerializer = new WorldSerializer();

        public static List<Entity> GetSnapshot(IEcsContext context, byte[] message)
        {
            var data = Encoding.ASCII.GetString(message);
            var snapshot = WorldSerializer.Deserialize(context, data);
            return snapshot;
        }

        public static byte[] GetMessage(IEcsContext context, List<Entity> entities)
        {
            var data = WorldSerializer.Serialize(context, entities);
            var message = Encoding.ASCII.GetBytes(data);
            return message;
        }
    }
}
