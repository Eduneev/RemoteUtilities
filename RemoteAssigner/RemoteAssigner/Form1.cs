using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteAssigner
{
    public partial class Form1 : Form
    {
        Byte ComNo = 3;
        int DeviceType = 1;
        int maxId = 400;
        int minId = 1;

        RF21xDevice rf;
        RF21xMessage message;


        public Form1()
        {
            InitializeComponent();
        }

        private void ClearInfo()
        {
            lblTitle.Text = "";
            lblID.Text = "";
            lblContent.Text = "";
            labelStatus.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rf = new RF21xDevice();
            message = new RF21xMessage();

            comboBoxComNo.SelectedItem = "Hid";
            //comboBoxDevice.SelectedIndex = 0;
            ClearInfo();
        }

        /*      private void buttonAdvance_Click(object sender, EventArgs e)
              {
                  groupBoxAdvance.Visible = true;
                  buttonAdvance.Visible = false;
              }
              */

        //    connect and disconnect
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ClearInfo();
            maxId = int.Parse(textBoxMaxId.Text);
            minId = int.Parse(textBoxMinId.Text);
            if (maxId <= 0 || minId > 400 || minId <= 0 || maxId < minId)
            {
                MessageBox.Show("Input error!");
                return;
            }

            ComNo = Convert.ToByte(comboBoxComNo.SelectedIndex);
            /*
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
            }*/
            DeviceType = rf21x.RF21X_DT_RF217;

            if (ComNo > 0)
            {
                if (rf.open(DeviceType, "serial://Com" + ComNo.ToString(), minId, maxId))
                {
                    MessageBox.Show("Connect Receiver OK!");
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Connect Receiver Error!");
                }
            }
            else
            {
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


        // activate and stop receiver
        private void buttonActivate_Click(object sender, EventArgs e)
        {
            ClearInfo();

            //  if (Activate_Receiver(TempComNo, TempQuType, TempQuPara, TempGroupNo, TempQuNo, tempNewOrRepeat, tempIsItemNumber) == 1)
            //     if (Activate_Receiver(Convert.ToByte(ComNo), Convert.ToByte(textBoxQuType.Text), Convert.ToByte(textBoxQuPara.Text),Convert.ToByte(textBoxGroupNo.Text),Convert.ToByte(textBoxQuNo.Text),tempNewOrRepeat,tempIsItemNumber ) == 1)

            if (rf.startQuiz(rf21x.RF21X_QT_Single))//  rf21x.RF21X_QT_Single)) 
            {
                //                MessageBox.Show("Activate receiver OK!");
            }
            else
            {
                MessageBox.Show("Activate Error!");
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            ClearInfo();
            if (rf.stopQuiz())// if (Stop_Receiver(ComNo) == 1)
            {
                //              MessageBox.Show("Stop receiver OK!");
            }
            else
            {
                MessageBox.Show("Stop Error!");
            }
        }


        //  set Id and set teacher remote
        private void buttonIdSet_Click(object sender, EventArgs e)
        {
            ClearInfo();
            int SetID;
            if (txtID.Text == "")
            {
                return;
            }
            SetID = int.Parse(txtID.Text);
            if (SetID <= 0)
            {
                MessageBox.Show("SetID value Error!");
                return;
            }
            if (rf.setKeypadId(SetID))// if (Set_ID(ComNo, SetID) != 1)
            {
            }
            else
            {
                MessageBox.Show("Set Id Error!");
            }
        }

        private void buttonTeacherSet_Click(object sender, EventArgs e)
        {
            ClearInfo();
            //           if (Set_Teacher(ComNo) != 1)
            //             MessageBox.Show("Set Teacher Error");
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            ClearInfo();
            //       if (Open_Receiver(ComNo,1) != 1)
            //         MessageBox.Show("Open Error");
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
                if (message.messageType == rf21x.RF21X_MT_Teacher)
                {
                    lblTitle.Text = "Teacher";
                    lblContent.Text = message.data;
                    lblID.Text = "";
                }
                else if (message.messageType == rf21x.RF21X_MT_Student)
                {
                    lblTitle.Text = "Student";
                    lblID.Text = message.keypadId.ToString();
                    lblContent.Text = message.data;
                }
                else if (message.messageType == rf21x.RF21X_MT_SetId)
                {
                    lblTitle.Text = "Set Id";
                    lblID.Text = message.keypadId.ToString();
                    lblContent.Text = message.data;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
