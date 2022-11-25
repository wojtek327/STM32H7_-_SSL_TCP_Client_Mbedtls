using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prog_test_app.Forms
{
    public partial class FormConnection : Form
    {
        public FormConnection()
        {
            InitializeComponent();
            LoadTheme();
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = Color.FromArgb(51, 51, 76);
                    btn.ForeColor = Color.Gainsboro;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }

            labelServerPort.ForeColor = ThemeColor.PrimaryColor;
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int ConvertStringPortToInt(string portStr)
        {
            int serverPort = 0;
            bool result = int.TryParse(portStr, out serverPort);

            return serverPort;
        }

        private void btnConnectWithSSLServ_Click(object sender, EventArgs e)
        {
            string portNumber = txtBoxServerPort.Text;

            SSL_Connection_Class.sslServer = new SSLServer(ConvertStringPortToInt(portNumber));
            new Thread(SSL_Connection_Class.sslServer.RunServer).Start();
        }
    }
}
