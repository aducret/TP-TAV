using System;
using System.Linq;
using System.Threading;
using Network;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public float MoveSpeed;
        public string Ip;
        public int Port;
        public int ReceivePort;
        public int Id;
        
        private Channel _channel;
        private Thread _receiveThread;
        
        private void Start()
        {
            _channel = new Channel();
            _channel.Connect(Ip, Port);

            if (Id != 1)
            {
                _receiveThread = new Thread(new ThreadStart(ReceivePosition)) { IsBackground = true };
                _receiveThread.Start();
            }
        }

        private void Update()
        {
            if (Id != 1) 
                return;
            
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Time.deltaTime * MoveSpeed * -transform.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Time.deltaTime * MoveSpeed * transform.right;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Time.deltaTime * MoveSpeed * -transform.forward;
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Time.deltaTime * MoveSpeed * transform.forward;
            }
            
            SendPosition();
        }

        private void SendPosition()
        {
            print("Player " + Id + ": Sending data.");
            var data = new byte[sizeof(float) * 3];
            data = data.Concat(BitConverter.GetBytes(transform.position.x)).ToArray();
            data = data.Concat(BitConverter.GetBytes(transform.position.y)).ToArray();
            data = data.Concat(BitConverter.GetBytes(transform.position.z)).ToArray();
            _channel.Send(data);
        }

        private void ReceivePosition()
        {
            print("Player " + Id + ": Receiving start.");
            while (true)
            {
                var position = new Vector3();
                var data = _channel.Receive(ReceivePort);
                print("Player " + Id + ": Receiving data.");
                position.x = BitConverter.ToSingle(data, 0 * sizeof(float));
                position.y = BitConverter.ToSingle(data, 1 * sizeof(float));
                position.z = BitConverter.ToSingle(data, 2 * sizeof(float));
//                var player2 = GameObject.FindGameObjectWithTag("Player 2");
//                player2.transform.position = position;
                print(position);
            }
        }
    }
}
