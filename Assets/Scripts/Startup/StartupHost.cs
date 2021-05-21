using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using Network.Proxy;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupHost : StartupBase
    {
        private UdpClient _udpClient;
        private List<IPEndPoint> _clients;
        private HostNetworkProxy _hostProxy;
        private Snapshoter _snapshoter;

        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;

        protected override void Start()
        {
            base.Start();

            _udpClient = new UdpClient(54322);
            _clients = new List<IPEndPoint>() { new IPEndPoint(IPAddress.Loopback, 54321) };
            _hostProxy = new HostNetworkProxy(_udpClient, _clients);
            _hostProxy.Start();
            _snapshoter = new Snapshoter(Fixed, _hostProxy);
            
            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
                new ResetDirectionSystem(),
                new DirectionSystem(),
                new VelocitySystem(),
                new MovePlayerSystem(),
                new MoveWeaponSystem(),
                new MoveCameraSystem(),
                new GetPositionSystem(),
                new GetRotationSystem(),
                new ResetKeysInputsSystem());
            
            var thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            var thisPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            thisPlayerSwordGo.transform.parent = thisPlayerGo.transform;
            var cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            var otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            //var otherPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            //otherPlayerSwordGo.transform.parent = otherPlayerGo.transform;

            var thisPlayerEntity = EntityHelper.GetThisPlayerEntity(thisPlayerGo, 0, 3);
            var thisPlayerSwordEntity = EntityHelper.GetPlayerSwordEntity(thisPlayerSwordGo, thisPlayerGo, 1, 1.2f)
                .MakeEntityMouseInputReceiver(cameraGo);
            var cameraEntity = EntityHelper.GetCameraEntity(cameraGo, thisPlayerGo);

            var otherPlayerEntity = EntityHelper.GetOtherPlayerEntity(otherPlayerGo, 2, 3, _clients.First());
            //var otherPlayerSwordEntity = EntityHelper.GetOtherPlayerSwordEntity(otherPlayerSwordGo, otherPlayerGo, 1.2f);

            Unfixed.AddEntity(thisPlayerEntity);
            Unfixed.AddEntity(thisPlayerSwordEntity);
            Unfixed.AddEntity(otherPlayerEntity);
            
            Fixed.AddEntity(thisPlayerEntity);
            Fixed.AddEntity(thisPlayerSwordEntity);
            Fixed.AddEntity(cameraEntity);
            Fixed.AddEntity(otherPlayerEntity);
            //Fixed.AddEntity(otherPlayerSwordEntity);
        }

        protected override void FixedUpdate()
        {
            _snapshoter.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _udpClient?.Dispose();
            _hostProxy?.Dispose();
        }
    }
}
