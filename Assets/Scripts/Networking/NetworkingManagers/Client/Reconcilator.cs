using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ECS.Components;
using Assets.Scripts.Networking.NetworkingManagers;
using Assets.Scripts.Networking.Serializers;
using Assets.Scripts.Util;
using ECS.Core;
using Network.Proxy;

public class Reconcilator : IEngineWrapper
{
    private readonly Engine _engine;
    private readonly ClientNetworkProxy _clientProxy;

    public Reconcilator(Engine engine, ClientNetworkProxy clientProxy)
    {
        _engine = engine;
        _clientProxy = clientProxy;
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
            var snapshot = MessageHelper.GetSnapshot(EcsContextHelper.HostWorldContext, message);
            var reconcilableEntities = _engine.GetIdentifiedEntities();

            foreach (var entity in snapshot)
            {
                var reconcilable = reconcilableEntities[entity.Id()];

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
        var serializableEntities = GetSerializableEntities();
        var message = MessageHelper.GetMessage(EcsContextHelper.ClientWorldContext, serializableEntities);
        _clientProxy.WriteBuffer.Write(message);
    }

    private List<Entity> GetSerializableEntities()
        => _engine.GetEntities().Filter<SerializableComponent>().ToList();
}