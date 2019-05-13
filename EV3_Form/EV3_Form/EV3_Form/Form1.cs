using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EV3MessengerLib;
using System.IO.Ports;

namespace EV3_Form {
    public partial class Form1 : Form {
        string temp = "";
        int output = -1;

        private EV3Messenger messenger;
        public Form1() {
            InitializeComponent();
            messenger = new EV3Messenger();
            fillSerialPortSelectionBoxWithAvailablePorts();
            updateFormGUI();
            timer1.Enabled = true;
            timer1.Interval = 50;
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            fillSerialPortSelectionBoxWithAvailablePorts();
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (portListBox.SelectedIndex > -1) {
                string port = portListBox.SelectedItem.ToString().ToUpper();
                if (messenger.Connect(port)) {
                    updateFormGUI();
                    MessageBox.Show("Connected!");
                }
                else {
                    MessageBox.Show("Failed to connect to serial port " + port);
                }
            }
            else {
                MessageBox.Show("Please select a port for the bluetooth connection");
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e) {
            messenger.Disconnect();
            updateFormGUI();
        }

        private void fillSerialPortSelectionBoxWithAvailablePorts() {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);

            portListBox.Items.Clear();
            foreach (String port in ports) {
                portListBox.Items.Add(port);
            }
        }

        private void updateFormGUI() {
            if (messenger.IsConnected) {
                refreshButton.Enabled = false;
                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
            }
            else {
                refreshButton.Enabled = true;
                connectButton.Enabled = true;
                disconnectButton.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            EV3Message msg = messenger.ReadMessage();
            if (msg != null) temp = msg.ValueAsText;
            if (!string.IsNullOrEmpty(temp)) output = int.Parse(temp);
            if (output == 1) tbStatus.Text = "Running Follow Path Algorithm";
            else if (output == 2) tbStatus.Text = "Running Object Pickup Algorithm";
            else if (output == 3) tbStatus.Text = "Running DropOff Algorithm";
            else if (output == 4) tbStatus.Text = "Running Return To Start Algorithm";
            else if (output == 5) tbStatus.Text = "Finished";
        }

        private void btnEmergency_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Stop");
        }

        private void btnClaw_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Claw");
        }

        private void btnStart_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Start");
            btnLeft.Visible = false;
            btnRight.Visible = false;
            btnForward.Visible = false;
            btnBackward.Visible = false;
        }

        private void btnForward_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Forward");
        }

        private void btnBackward_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Backward");
        }

        private void btnLeft_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Right");
        }

        private void btnRight_Click(object sender, EventArgs e) {
            messenger.SendMessage("abc", "Left");
        }
    }
}
