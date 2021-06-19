using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networking.NetworkingManagers;
using Assets.Scripts.Networking.Serializers;
using Assets.Scripts.Util;
using ECS.Core;
using Network.Proxy;
using UnityEngine;

public class Reconcilator : IEngineWrapper
{
    private const int AllowedAttemptsToReceive = 300;

    private readonly Engine _engine;
    private readonly IClientNetworkProxy _clientProxy;

    private int _attemptsToReceive;

    public Reconcilator(Engine engine, IClientNetworkProxy clientProxy)
    {
        _engine = engine;
        _clientProxy = clientProxy;
        _attemptsToReceive = 0;
    }

    public void Update(double dt)
    {
        Send();
        Receive();

        if (_attemptsToReceive > AllowedAttemptsToReceive)
        {
            throw new ReceiveException("Хост не отвечает.");
        }

        _engine.Update(dt);
    }

    private void Receive()
    {
        if (!_clientProxy.ReadBuffer.IsEmpty)
        {
            _attemptsToReceive = 0;
            var message = _clientProxy.ReadBuffer.ReadLast();

            try
            {
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
            catch
            {
                Debug.Log("Не удалось десериализовать сообщение.");
            }
        }
        else
        {
            _attemptsToReceive++;
        }
    }

    private void Send()
    {
        var serializableEntities = GetSerializableEntities();
        var message = MessageHelper.GetMessage(EcsContextHelper.ClientWorldContext, serializableEntities);
        _clientProxy.WriteBuffer.Write(message);
    }

    private List<Entity> GetSerializableEntities()
        => _engine.GetEntities().Filter<IsSerializableComponent>().ToList();
}