using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networking.NetworkingManagers;
using Assets.Scripts.Networking.Serializers;
using Assets.Scripts.Util;
using ECS.Core;
using Network.Proxy;
using UnityEngine;

public class Snapshoter : IEngineWrapper
{
    private readonly Engine _engine;
    private readonly IHostNetworkProxy _hostProxy;

    public Snapshoter(Engine engine, IHostNetworkProxy hostProxy)
    {
        _engine = engine;
        _hostProxy = hostProxy;
    }

    public void Update(double dt)
    {
        Receive();
        _engine.Update(dt);
        Send();
    }

    private void Receive()
    {
        var receivedSnapshots = new List<List<Entity>>();

        foreach (var client in _hostProxy.Clients)
        {
            var buffer = _hostProxy.GetReadBuffer(client);

            if (!buffer.IsEmpty)
            {
                try
                {
                    var message = buffer.ReadLast();
                    var snapshot = MessageHelper.GetSnapshot(EcsContextHelper.ClientWorldContext, message);
                    receivedSnapshots.Add(snapshot);
                }
                catch
                {
                    Debug.Log("Не удалось десериализовать сообщение.");
                }
            }
        }

        var identifiedEntities = _engine.GetIdentifiedEntities();

        foreach (var snapshot in receivedSnapshots)
        {
            foreach (var entity in snapshot)
            {
                var entityToUpdate = identifiedEntities[entity.Id()];

                foreach (var typeComponentPair in entity.ToList())
                {
                    entityToUpdate.Remove(typeComponentPair.Key);
                    entityToUpdate.Add(typeComponentPair.Value);
                }
            }
        }
    }

    private void Send()
    {
        var serializableEntities = GetSerializableEntities();
        var message = MessageHelper.GetMessage(EcsContextHelper.HostWorldContext, serializableEntities);

        foreach (var client in _hostProxy.Clients)
        {
            var buffer = _hostProxy.GetWriteBuffer(client);
            buffer.Write(message);
        }
    }

    private List<Entity> GetSerializableEntities() 
        => _engine.GetEntities().Filter<IsSerializableComponent>().ToList();
}
