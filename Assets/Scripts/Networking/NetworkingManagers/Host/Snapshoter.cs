using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using Assets.Scripts.Networking.NetworkingManagers;
using Assets.Scripts.Networking.Serializers;
using Assets.Scripts.Util;
using ECS.Core;
using ECS.Serialization;
using Network.Proxy;
using UnityEngine;

public class Snapshoter : IEngineWrapper
{
    private readonly Engine _engine;
    private readonly HostNetworkProxy _hostProxy;
    private readonly ComponentSerializer _componentSerializer;
    private readonly WorldSerializer _worldSerializer;

    public Snapshoter(Engine engine, HostNetworkProxy hostProxy)
    {
        _engine = engine;
        _hostProxy = hostProxy;
        _componentSerializer = new ComponentSerializer();
        _worldSerializer = new WorldSerializer();
    }

    public void Update(double dt)
    {
        Receive();
        _engine.Update(dt);
        Send();
    }

    private void Receive()
    {
        foreach (var node in _engine.GetNodes<ClientKeyNode>().ToList())
        {
            var buffer = _hostProxy.GetReadBuffer(node.EndPointComponent.EndPoint);

            if (!buffer.IsEmpty)
            {
                var message = buffer.ReadLast();
                var data = Encoding.ASCII.GetString(message);
                var component = _componentSerializer.Deserialize(EcsContextHelper.ClientWorldContext, data);
                node.Entity.Remove<KeysComponent>();
                node.Entity.Add(component);
            }
        }
    }

    private void Send()
    {
        var serializableEntities = GetSerializableEntities();
        var data = _worldSerializer.Serialize(EcsContextHelper.HostWorldContext, serializableEntities);
        var message = Encoding.ASCII.GetBytes(data);

        foreach (var node in _engine.GetNodes<EndPointNode>())
        {
            var buffer = _hostProxy.GetWriteBuffer(node.EndPointComponent.EndPoint);
            buffer.Write(message);
        }
    }

    private List<Entity> GetSerializableEntities() 
        => _engine.GetEntities().Filter<SerializableComponent>().ToList();
}
