using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Lidgren.Network;

namespace TwoSides.Network
{
    public class NetPlay
    {
        public NetPlay()
        {
        }
        static NetServer server;
        public static int typeNet = 0;
        static NetClient client;
        public static void ClientLoop()
        {
            NetIncomingMessage msg;
            bool open = true;
            while (open)
            {
                msg = client.ReadMessage();
                if(msg == null) continue;
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            readMsg(msg);
                            break;
                        }

                    case NetIncomingMessageType.VerboseDebugMessage:

                    case NetIncomingMessageType.DebugMessage:

                    case NetIncomingMessageType.WarningMessage:

                    case NetIncomingMessageType.ErrorMessage:

                        Console.WriteLine(msg.ReadString());

                        break;
                    case NetIncomingMessageType.StatusChanged:
                        {
                            break;
                        }
                    default:

                        Console.WriteLine("Unhandled type: " + msg.MessageType);

                        break;

                }
                client.Recycle(msg);

            }

        }
        static void readMsg(NetIncomingMessage msg)
        {
            int type = msg.ReadInt32();
            if (type == 0)
            {

                string a = msg.ReadString();
                Console.WriteLine(a);
            }
            else if (type == 1)
            {
                int dimensionsID = msg.ReadInt32();
                int x = msg.ReadInt32();
                int y = msg.ReadInt32();
                if (typeNet == 1) sendMsg(0, dimensionsID,x,y);
                else
                {
                    Program.game.dimension[dimensionsID].map[x,y].read(msg);
                }
            }
            else if (type == 2)
            {
                int dimensionsID = msg.ReadInt32();
                int x = msg.ReadInt32();
                if (typeNet == 1) sendMsg(0, dimensionsID, x);
                else
                {
                    Program.game.dimension[dimensionsID].mapB[x].read(msg);
                    Program.game.dimension[dimensionsID].mapHeight[x] = msg.ReadInt32();
                }
            }
            else if (type == 3)
            {
                int dimensionsID = msg.ReadInt32();
                int x = msg.ReadInt32();
                int x1 = msg.ReadInt32();
                int y = msg.ReadInt32();
                int y1 = msg.ReadInt32();
                if (typeNet == 1) sendMsg(3, dimensionsID, x,x1,y,y1);
                else
                {
                    System.Console.WriteLine("Client read object");
                    Program.game.dimension[dimensionsID].active = false;
                    for (int i = x; i < x1; i++)
                    {
                        Program.game.dimension[dimensionsID].mapB[i].read(msg);
                        Program.game.dimension[dimensionsID].mapHeight[i] = msg.ReadInt32();
                        for (int j = y; j < y1; j++)
                        {
                            Program.game.dimension[dimensionsID].map[i, j].read(msg);
                        }
                    }
                    Program.game.dimension[dimensionsID].active = true;
                }
            }
        }
        public static void sendMsg(int type,params object[] msg)
        {
            NetOutgoingMessage sendMsg;
            if (typeNet == 1) sendMsg = server.CreateMessage();
            else sendMsg = client.CreateMessage();
            sendMsg.Write(type);
            if (type == 0) {

                sendMsg.Write(msg[0].ToString());
                if (typeNet == 1) server.SendToAll(sendMsg, NetDeliveryMethod.ReliableOrdered);
                else client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
            }
            else if (type == 1)
            {
                int dimensionsID = (int)msg[0];
                int x = (int)msg[1];
                int y = (int)msg[2];
                if (typeNet == 1)
                {
                    Program.game.dimension[dimensionsID].map[x, y].send(sendMsg);
                    server.SendMessage(sendMsg, server.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }
                else
                {
                    sendMsg.Write(dimensionsID);
                    sendMsg.Write(x);
                    sendMsg.Write(y);
                }

            }
            else if (type == 3)
            {
                int dimensionsID = (int)msg[0];
                int x = (int)msg[1];
                int x1 = (int)msg[2];
                int y = (int)msg[3];
                int y1 = (int)msg[4];
                sendMsg.Write(dimensionsID);
                sendMsg.Write(x);
                sendMsg.Write(x1);
                sendMsg.Write(y);
                sendMsg.Write(y1);
                if (typeNet == 1)
                {
                    System.Console.WriteLine("Server send object");
                    for(int i = x;i <x1;i++){
                        Program.game.dimension[dimensionsID].mapB[i].send(sendMsg);
                        sendMsg.Write(Program.game.dimension[dimensionsID].mapHeight[i]);
                        for (int j = y; j < y1; j++)
                        {
                                Program.game.dimension[dimensionsID].map[i, j].send(sendMsg);
                        }
                    }
                    server.SendMessage(sendMsg, server.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }
                else
                {
                    System.Console.WriteLine("Client call object");
                    Program.game.dimension[dimensionsID].active = false;
                    client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
                }
            }
            else if (type == 2)
            {
                int dimensionsID = (int)msg[0];
                int x = (int)msg[1];
                if (typeNet == 1)
                {
                    Program.game.dimension[dimensionsID].mapB[x].send(sendMsg);
                    sendMsg.Write(Program.game.dimension[dimensionsID].mapHeight[x]);
                    server.SendMessage(sendMsg, server.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }
                else
                {
                    sendMsg.Write(dimensionsID);
                    sendMsg.Write(x);
                }
            }
        }

        public static void loop()
        {
            NetIncomingMessage msg;
            bool open = true;
            while (open)
            {
                msg = server.ReadMessage();
                if(msg == null) continue;
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:{
                        readMsg(msg);
                        break;
                    }

                    case NetIncomingMessageType.VerboseDebugMessage:

                    case NetIncomingMessageType.DebugMessage:

                    case NetIncomingMessageType.WarningMessage:

                    case NetIncomingMessageType.StatusChanged:
                        {
                            byte a = msg.ReadByte();
                            Console.WriteLine(a);
                            if (a == 5)
                            {
                                string status = msg.ReadString();
                                sendMsg(0, status);
                            }
                            break;
                        }
                    case NetIncomingMessageType.ErrorMessage:

                        Console.WriteLine(msg.ReadString());

                        break;

                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);

                        break;

                }

                server.Recycle(msg);

            }


        }
        public static void startServer(object thread)
        {

            NetPeerConfiguration config = new NetPeerConfiguration("MyExampleName");
            config.Port = 14242;
            server = new NetServer(config);
            
            server.Start();
            typeNet = 1;
            loop();
        }
        public static void startClient(object thread)
        {

            NetPeerConfiguration config = new NetPeerConfiguration("MyExampleName");
            client = new NetClient(config);
            client.Start();
            client.Connect(host: "127.0.0.1", port: 14242);
            typeNet = 2;
            ClientLoop();
            
        }
    }
}
