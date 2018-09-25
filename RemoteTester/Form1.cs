using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteTester
{
    public partial class Form1 : Form
    {
        int DeviceType = 1;
        static int numLabels = 42;
        int minId = 1;
        int maxId = numLabels+1;

        RF21xDevice rf;
        RF21xMessage message;
        

        public Form1()
        {
            InitializeComponent();
        }
        private void ClearInfo()
        {
            for (int i = 1; i <= numLabels; i++)
            {
                string str = "id" + i;
                TextBox tbx = this.Controls.Find(str, true).FirstOrDefault() as TextBox;
                tbx.BackColor = System.Drawing.SystemColors.Window;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            rf = new RF21xDevice();
            message = new RF21xMessage();

            comboBoxDevice.SelectedIndex = 0;
            ClearInfo();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ClearInfo();

            switch (comboBoxDevice.SelectedIndex)
            {
                case 0: //rf217
                    {
                        DeviceType = rf21x.RF21X_DT_RF217;
                        break;
                    }
                case 1: //"rf218":
                    {
                        DeviceType = rf21x.RF21X_DT_RF218;
                        break;
                    }
                case 2: //"rf219":
                    {
                        DeviceType = rf21x.RF21X_DT_RF219;
                        break;
                    }
                case 3:// "rf215":
                    {
                        DeviceType = rf21x.RF21X_DT_RF215;
                        break;
                    }
            }

            if (rf.open(DeviceType, "hid://", minId, maxId))
            {
                MessageBox.Show("Connect Receiver OK!");
                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Connect Receiver Error!");
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            ClearInfo();

            if (rf.close())
            {
                MessageBox.Show("Disconnect OK!");
            }
            else
            {
                MessageBox.Show("error!");
            }
        }

        private void buttonActivate_Click(object sender, EventArgs e)
        {
            ClearInfo();

            if (rf.startQuiz(rf21x.RF21X_QT_Single)) 
            {
                //MessageBox.Show("Activate receiver OK!");
            }
            else
            {
                MessageBox.Show("Activate Error!");
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearInfo();
            if (rf.stopQuiz())
            {
                //MessageBox.Show("Stop receiver OK!");
            }
            else
            {
                MessageBox.Show("Stop Error!");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //bool a = CloseHnd();
            rf.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rf.getMessage(message))
            {
                if (message.messageType == rf21x.RF21X_MT_Student)
                {
                    string str = "id" + message.keypadId;
                    TextBox tbx = this.Controls.Find(str, true).FirstOrDefault() as TextBox;
                    tbx.BackColor = System.Drawing.Color.LimeGreen;
                }
            }
        }
    }
}
