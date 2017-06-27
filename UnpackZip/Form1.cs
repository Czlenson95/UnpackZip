using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnpackZip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string GetBytesToString(byte[] value)
        {
            SoapHexBinary shb = new SoapHexBinary(value);
            return shb.ToString();
        }


        Zipper zip = null;
        Socket listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

        private void btnServStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine(localEndPoint);
            logWindowUpdate(localEndPoint);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                Socket handler = listener.Accept();
                

                while (true)
                {
                    data = "";
                    dataRec = new byte[0];
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);

                        int oldLen = dataRec.Length;
                        Array.Resize(ref dataRec, dataRec.Length + bytesRec);
                        Array.Copy(bytes, 0, dataRec, oldLen, bytesRec);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("/rn/rn/rn") > -1)
                        {
                            break;
                        }
                    }
                    //Console.WriteLine(data);
                    //data = data.Substring(0, data.Length - 9);

                    if (data[0] == '1' && data[1] == 'x' && data[2] == '1' && data[3] == 'p')
                    {
                        string tempData = data;
                        int[] filesToUnpack;
                        tempData = tempData.Substring(4, tempData.Length - 13);


                        List<string> names = tempData.Split(';').ToList();
                        tempData = "";
                        foreach (string name in names)
                        {
                            int index = Convert.ToInt32(name); // INDEKS PLIKU DO WYPAKOWANIA
                            Console.WriteLine(name);
                            tempData += GetBytesToString(zip.arrFiles[index].fileData);
                            tempData += ";";
                        }

                        //tempData = "xDDDDDDDDDDDDDDD";

                        String responseToSend = tempData;
                        byte[] msg = Encoding.ASCII.GetBytes(responseToSend);
                        handler.Send(msg);
                    }
                    else
                    {
                        File.WriteAllBytes("temporary.zip", dataRec);
                        data = "";

                        zip = new Zipper("temporary.zip");

                        for (int i = 0; i < zip.filesCounter; ++i)
                        {
                            Console.WriteLine(zip.arrFiles[i].name);
                            data += (zip.arrFiles[i].name + ";");
                        }

                        data = data.Substring(0, data.Length - 1);

                        logWindowUpdate("Text received length: " + data.Length);
                        String responseToSend = data;
                        byte[] msg = Encoding.ASCII.GetBytes(responseToSend);



                        handler.Send(msg);
                    }


                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string data = null;
        public static byte[] dataRec = null;
        byte[] bytes = new Byte[1024];
        static IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        static IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        public void logWindowUpdate(dynamic toLog)
        {
            Console.WriteLine(toLog.ToString());
            lstServerInfoBox.Items.Add(toLog.ToString());
            lstServerInfoBox.Refresh();
        }
    }
}
