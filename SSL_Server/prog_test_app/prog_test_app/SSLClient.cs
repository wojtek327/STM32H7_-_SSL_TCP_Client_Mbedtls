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
    public class SSLClient
    {
        BackgroundWorker backgroundWorker1;

        private string ClientCertificateFile = @"cert_path";//plik certyfikatu
        private string ClientCertificatePassword = "cert_pass";
        private int ServerPort;
        private IPAddress ServerIpAddress;
        private string TargetHost = "";
        private SslStream sslStream;
        private TcpClient client;

        public SSLClient(string serverIpAddress, int serverPort, string targetHost)
        {
            this.ServerIpAddress = IPAddress.Parse(serverIpAddress);
            this.ServerPort = serverPort;
            this.TargetHost = targetHost;
        }

        public SSLClient(string serverIpAddress, int serverPort, string targetHost, string clientCertificateFilePath, string clientCertificatePassword)
        {
            this.ServerIpAddress = IPAddress.Parse(serverIpAddress);
            this.ServerPort = serverPort;
            this.TargetHost = targetHost;
            this.ClientCertificateFile = clientCertificateFilePath;
            this.ClientCertificatePassword = clientCertificatePassword;
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

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }

        private void DisableBackgroundWorker()
        {
            backgroundWorker1.DoWork -= backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted -= backgroundWorker1_RunWorkerCompleted;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = ReceiveResponse();
        }

        private int ReceiveResponse()
        {
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                try
                {
                    bytes = sslStream.Read(buffer, 0, buffer.Length);
                    sslStream.Write(buffer, 0, bytes);
                }
                catch
                {
                    client.Close();
                    return -1;
                }

                Debug.WriteLine(buffer.ToString());
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                for (int i = 0; i < chars.Length; i++)
                {
                    Debug.Write(chars[i].ToString());
                }

                if (chars[0] == 0x01 && chars[1] == 0xA5 && chars[2] == 0x55)    //Check if close msg
                {
                    client.Close();
                }

            } while (bytes != 0);

            return 0;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        public bool CheckIfPortIsAvailable(int port)
        {
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        public string GetServerIP()
        {
            return ServerIpAddress.ToString();
        }

        public int GetServerPort()
        {
            return ServerPort;
        }

        public static bool CertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) { return true; }
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) { return true; }
            Debug.Write("---" + Environment.NewLine);
            Debug.Write("*** Błd certyfikatu SSL: " + sslPolicyErrors.ToString() + Environment.NewLine);
            Debug.Write("---" + Environment.NewLine);
            Debug.Write("---" + Environment.NewLine);
            Debug.Write("*** Błd certyfikatu SSL: " + sslPolicyErrors.ToString() + Environment.NewLine);
            Debug.Write("---" + Environment.NewLine);
            return false;
        }

        public void RunClient()
        {
            var clientCertificate = new X509Certificate2(ClientCertificateFile, ClientCertificatePassword);
            X509CertificateCollection cert = new X509Certificate2Collection(clientCertificate);
            client = new TcpClient(ServerIpAddress.ToString(), ServerPort);
            Debug.WriteLine("Client connected.");
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(CertificateValidation), null);

            try
            {
                sslStream.AuthenticateAsClient(TargetHost, cert, SslProtocols.Tls12, false);
            }
            catch (AuthenticationException e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Debug.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Debug.WriteLine("Authentication failed - closing the connection.");
                client.Close();
                return;
            }

            InitializeBackgroundWorker();
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
