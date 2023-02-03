using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IHM_1v0
{
    public class Controller
    {
        public string message   = "00000000000000000000";  // The message that will be sent to controller
        public string rTelegram = "00000000000000000000";  //The message received from controller
        public bool keepFlag    = true;                    //If this == true, then Keep Alive is enable
        public bool _fatalError = false;                   //Check if the session has some fatal error
        public bool newChange   = false;                   //Check if controller sent something

        public int MIDfromController = 0;
        public int batchCount = 0;

        public struct OPHeader
        {
            public int Size { get; set; }
            public int MID { get; set; }
            public int Revision { get; set; }
        }

        private void HandleOpTelegram(OPHeader header, string PayloadStr)
        {
            switch (header.MID) // This switch verify the header from MID. U can implement any MID u want just following the manual from the controller.
            {
                case 2:
                    //Application Communication start acknowledge
                    _connected = true;
                    message = "00200060000000000000\0"; //Never forget to send the Null character, or program will not work with Stanley Controllers or Cleco.
                    client.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);
                    break;
                case 4:
                    //Application Command error
                    break;
                case 5:
                    //Application Command accepted
                    break;
                case 11:
                    //Parameter set ID upload reply 
                    break;
                case 13:
                    //Parameter set data upload reply 
                    break;
                case 15:
                    //Parameter set selected 
                    break;
                case 22:
                    //Lock at batch done upload
                    break;
                case 31:
                    //Job ID upload reply
                    break;
                case 33:
                    //Job data upload reply
                    break;
                case 35:
                    //Job info
                    break;
                case 41:
                    //Tool data upload reply
                    break;
                case 48:
                    //Pairing Status 
                    break;
                case 52:
                    //Vehicle ID number
                    break;
                case 61:
                    message = "00200062000000000000\0"; //This is the response from data receive
                    var messageBytes = Encoding.UTF8.GetBytes(message); //U can use this var as Global var, but i prefered to declare here
                    client.SendAsync(messageBytes, SocketFlags.None);

                    torque = float.Parse(PayloadStr.Substring(121, 5)) / 100;
                    angle = int.Parse(PayloadStr.Substring(149, 5));
                    status = PayloadStr.Substring(87, 1);
                    torqueStatus = PayloadStr.Substring(105, 1);

                    switch (status)
                    {
                        case "1": //Check if bolt is ok
                            status = "OK";
                            batchCount++;
                            if (batchCount == 10) //Here i defined the batch as 10, but u can use the MID response to dinamically get the batch size from controller
                                batchCount = 0;
                            break;

                        default:
                            status = "NOK";
                            break;
                    }

                    newChange = true; //Flag to indicate to WinForm that a new change arrived


                    break;

                case 65:
                    //Old tightening result upload reply
                    break;
                case 71:
                    //Alarm
                    break;
                case 74:
                    //Alarm acknowledged on controller
                    break;
                case 76:
                    //Alarm status
                    break;
                case 81:
                    //Read time upload reply
                    break;
                case 91:
                    //Multi-spindle status 
                    break;
                case 101:
                    //Multi - spindle result
                    break;
                case 106:
                    //Last Power MACS tightening result Station data
                    break;
                case 107:
                    //Last Power MACS tightening result Bolt data 
                    break;
                case 121:
                    //Job line control started 
                    break;
                case 122:
                    //Job line control alert 1
                    break;
                case 123:
                    //Job line control alert 2
                    break;
                case 124:
                    //Job line control done
                    break;
                case 152:
                    //Multiple identifiers work order
                    break;
                case 211:
                    //Status external monitored inputs 
                    break;
                case 9999:

                    break;

            }
        }

        public int angle = 0;
        public float torque = 0;
        public string status = "";
        public string torqueStatus = "";

        public int i = 0;
        public string receivedTelegram = "";
        public bool _connected = false;

        public void sendMID()
        {
            if (message.Length > 1)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                try
                {
                    client.SendAsync(messageBytes, SocketFlags.None);
                }
                catch
                {
                    _fatalError = true;
                }
            }
        }

        public void endServer()
        {
            if (client.Connected)
                client.Shutdown(SocketShutdown.Both);
        }

        public async Task<int> StartServer(string ip, int port)
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse(ip), port);

            client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(ipEndPoint);

            message = "002000010000      00\0";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);


            while (true)
            { //I don't know if there is a more "Clean Code" way to implement this loop, so i used a infinity while loop
                OPHeader header = new OPHeader();

                var payload = new byte[1024];
                var buffer = new byte[20];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);

                receivedTelegram = Encoding.UTF8.GetString(buffer, 0, received);
                header.Size = int.Parse(receivedTelegram.Substring(0, 4));
                header.MID = int.Parse(receivedTelegram.Substring(4, 4));
                header.Revision = int.Parse(receivedTelegram.Substring(8, 3).Replace(" ", "0"));

                MIDfromController = header.MID;
                string PayloadStr = "";

                while (PayloadStr.Length < header.Size - 20 + 1)
                {
                    var rx = await client.ReceiveAsync(payload, SocketFlags.None);
                    PayloadStr += Encoding.UTF8.GetString(payload, 0, rx);
                }

                PayloadStr = PayloadStr.Substring(0, PayloadStr.Length - 1);
                rTelegram = receivedTelegram + PayloadStr;

                HandleOpTelegram(header, PayloadStr);
            }
            client.Shutdown(SocketShutdown.Both); //The program never will be here, but u need to use this shutdown function to avoid some errors
            return 0;
        }
        Socket? client;
    }
}
