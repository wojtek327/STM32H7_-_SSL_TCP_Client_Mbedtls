using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.NetworkInformation;
using System.ComponentModel;

namespace prog_test_app
{
    public class SSLServer
    {
        private string ServerCertificateFile = @"C:\\Users\\wojte\\OneDrive\\Pulpit\\PROJEKTY_WAZNE_W_HUJ\\_ssl\\_testowe_certyfikaty\\server_cert.pfx";
        private string ServerCertificatePassword = "test1324@";
        private int ServerPort;
        private SslStream sslStream;
        private TcpListener listener;
        private TcpClient client;

        public SSLServer(int serverPort)
        {
            this.ServerPort = serverPort;
        }

        public SSLServer(int serverPort, string clientCertificateFilePath, string clientCertificatePassword)
        {
            this.ServerPort = serverPort;
            this.ServerCertificateFile = clientCertificateFilePath;
            this.ServerCertificatePassword = clientCertificatePassword;
        }

        public int GetServerPort()
        {
            return ServerPort;
        }

        public bool SSL_SendMsg(string msg)
        {
            byte[] messsage = Encoding.UTF8.GetBytes(msg);

            try
            {
                sslStream.Write(messsage);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool App_CertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) { return true; }
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) { return true; }
            Debug.WriteLine("---");
            Debug.WriteLine("*** Błd certyfikatu SSL: " + sslPolicyErrors.ToString());
            Debug.WriteLine("---");
            Debug.WriteLine("---");
            Debug.WriteLine("*** Błd certyfikatu SSL: " + sslPolicyErrors.ToString() + Environment.NewLine);
            Debug.WriteLine("---");
            return false;
        }

        public bool CheckIfPortIsAvailable(int port)
        {
            IPEndPoint[] test = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

            foreach (IPEndPoint ip in test)
            {
                if(ip.Port == port)
                {
                    return false;
                }
            }
            return true;
        }

        public void RunServer()
        {
            bool checkIfPortIsFree = CheckIfPortIsAvailable(ServerPort);

            if(checkIfPortIsFree == false)
            {
                Debug.WriteLine("Port is not available, Check connection.");
                return;
            }

            var serverCertificate = new X509Certificate2(ServerCertificateFile, ServerCertificatePassword);
            listener = new TcpListener(IPAddress.Any, ServerPort);
            listener.Start();

            try
            {
                using (client = listener.AcceptTcpClient())
                using (sslStream = new SslStream(client.GetStream(), false, App_CertificateValidation))
                {
                    sslStream.AuthenticateAsServer(serverCertificate, true, SslProtocols.Tls12, false);
                    Debug.WriteLine("---");
                    Debug.WriteLine("Połaczenie:              " + client.Client.RemoteEndPoint.ToString());
                    Debug.WriteLine("Lokalny certyfikat:      " + sslStream.LocalCertificate.Subject);
                    Debug.WriteLine("Zdalny certyfikat:       " + sslStream.RemoteCertificate.Subject);
                    Debug.WriteLine("Na porcie:               " + ServerPort);
                    Debug.WriteLine("---");

                    sslStream.ReadTimeout = 60000;
                    sslStream.WriteTimeout = 60000;

                    byte[] inputBuffer = new byte[256];
                    int inputBytes = 0;
                    Decoder decoder = Encoding.UTF8.GetDecoder();

                    while (true)
                    {
                        inputBytes = -1;
                        Array.Clear(inputBuffer, 0, inputBuffer.Length);
                        if (client.Available > 0)
                        {
                            do
                            {
                                inputBytes = sslStream.Read(inputBuffer, 0, inputBuffer.Length);
                            }
                            while (inputBytes == 0);

                            char[] chars = new char[decoder.GetCharCount(inputBuffer, 0, inputBytes)];
                            decoder.GetChars(inputBuffer, 0, inputBytes, chars, 0);

                            Debug.Write("Receive from client: ");

                            for (int i = 0; i < chars.Length; i++)
                            {
                                Debug.Write(chars[i].ToString());
                            }

                            //sslStream.Write(inputBuffer, 0, inputBytes);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("---");
                Debug.WriteLine("*** Problem" + ex.GetType().Name + "  " + ex.Message);
                Debug.WriteLine("---");
            }
        }

   }
}
