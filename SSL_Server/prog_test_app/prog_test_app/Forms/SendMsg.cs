using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace prog_test_app.Forms
{
    public partial class SendMsg : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public SendMsg()
        {
            InitializeComponent();
            LoadTheme();

            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 1000;
            myTimer.Start();
        }

        private void TimerEventProcessor(Object myObject,
                                        EventArgs myEventArgs)
        {
            myTimer.Stop();
            myTimer.Enabled = true;
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
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string msgToSend = txtBoxMsgToSend.Text;

            if (SSL_Connection_Class.sslServer != null)
            {
                bool returnMsg = SSL_Connection_Class.sslServer.SSL_SendMsg(msgToSend + "<EOF>");

                if (returnMsg == false)
                {
                    MessageBox.Show("Msg can't be send", "ERROR");
                }
            }
            else
            {
                MessageBox.Show("You need to connect with the server first", "ERROR");
            }
        }
    }
}
