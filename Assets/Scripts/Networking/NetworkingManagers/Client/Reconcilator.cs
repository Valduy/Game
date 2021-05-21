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

public class Reconcilator : IEngineWrapper
{
    private readonly Engine _engine;
    private readonly ClientNetworkProxy _clientProxy;
    private readonly ComponentSerializer _componentSerializer;
    private readonly WorldSerializer _worldSerializer;

    public Reconcilator(Engine engine, ClientNetworkProxy clientProxy)
    {
        _engine = engine;
        _clientProxy = clientProxy;
        _componentSerializer = new ComponentSerializer();
        _worldSerializer = new WorldSerializer();
    }

    public void Update(double dt)
    {
        Send();
        Receive();
        _engine.Update(dt);
    }

    private void Receive()
    {
        if (!_clientProxy.ReadBuffer.IsEmpty)
        {
            var message = _clientProxy.ReadBuffer.ReadLast();
            var data = Encoding.ASCII.GetString(message);
            var snapshot = _worldSerializer.Deserialize(EcsContextHelper.HostWorldContext, data);
            var reconcilableEntities = GetReconcilableEntities();

            foreach (var entity in snapshot)
            {
                var reconcilable = reconcilableEntities[entity.Get<IdComponent>().Id];

                foreach (var typeComponentPair in entity.ToList())
                {
                    reconcilable.Remove(typeComponentPair.Key);
                    reconcilable.Add(typeComponentPair.Value);
                }
            }
        }
    }

    private void Send()
    {
        foreach (var node in _engine.GetNodes<KeysNode>())
        {
            var data = _componentSerializer.Serialize(
                EcsContextHelper.ClientWorldContext, typeof(KeysComponent), node.KeysComponent);
            var message = Encoding.ASCII.GetBytes(data);
            _clientProxy.WriteBuffer.Write(message);
        }
    }

    private Dictionary<uint, Entity> GetReconcilableEntities() 
        => _engine.GetEntities()
            .Filter<IdComponent>()
            .ToDictionary(e => e.Get<IdComponent>().Id, e => e);
}