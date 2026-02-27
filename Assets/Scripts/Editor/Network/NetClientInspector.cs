using System;
using System.IO;
using System.Net;
using System.Text;
using Network;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Editor.Network
{
    [CustomEditor(typeof(NetClient))]
    public class NetClientInspector : UnityEditor.Editor
    {
        private string _ip = "127.0.0.1";
        private int _port = 8000;
        
        private Vector2 _logScroll;
        private string _log = "";
        private string _eventName = "";
        private string _eventData = "";
        
        private void OnEnable()
        {
            var client = (NetClient) target;
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/Icons/Net.png");
            
            EditorGUIUtility.SetIconForObject(client, icon);
            EditorGUIUtility.SetIconForObject(client.gameObject, icon);
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUILayout.Label("NetClient Connector", EditorStyles.boldLabel);
            
            _ip = EditorGUILayout.TextField("IP Address", _ip);
            _port = EditorGUILayout.IntField("Port", _port);

            var client = (NetClient) target;

            if (GUILayout.Button("Connect"))
                client.Connect(IPAddress.Parse(_ip), _port); ;

            if (GUILayout.Button("Disconnect"))
                client.Disconnect();
            
            GUILayout.Space(10);
            GUILayout.Label("Send Packets", EditorStyles.boldLabel);

            _eventName = EditorGUILayout.TextField("Event name", _eventName);
            _eventData = EditorGUILayout.TextField("Events data", _eventData);
            
            if (GUILayout.Button("Send SpinRequested"))
                SendPacket(client, new Packet { EventName = _eventData, Data = _eventData });

            GUILayout.Space(10);
            GUILayout.Label("Log", EditorStyles.boldLabel);

            _logScroll = EditorGUILayout.BeginScrollView(_logScroll, GUILayout.Height(150));
            EditorGUILayout.TextArea(_log, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
        
        // ReSharper disable once AsyncVoidMethod
        private async void SendPacket(NetClient client, object evt)
        {
            if (client.Stream is null || !client.Connected)
            {
                AppendLog("[Inspector] Client not connected!");
                return;
            }

            try
            {
                var packet = new Packet
                {
                    EventName = evt.GetType().Name,
                    Data = evt
                };
                
                string json;
                await using (var sw = new StringWriter())
                {
                    var serializer = new JsonSerializer();
                    
                    serializer.Serialize(sw, packet);
                    json = sw.ToString();
                }

                var messageBytes = Encoding.UTF8.GetBytes(json);
                var lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

                await client.Stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await client.Stream.WriteAsync(messageBytes, 0, messageBytes.Length);

                AppendLog($"[Inspector] Sent: {packet.EventName} ({messageBytes.Length} bytes)");
            }
            catch (Exception ex)
            {
                AppendLog($"[Inspector] Error sending packet: {ex.Message}");
            }
        }

        private void AppendLog(string msg)
        {
            _log += msg + "\n";
            Repaint();
        }
    }
    
    [Serializable]
    public class Packet
    {
        public string EventName { get; set; } = string.Empty;
        public object Data { get; set; } = null!;
    }
}