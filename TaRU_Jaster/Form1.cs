﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace TaRU_Jaster
{
    public partial class Form1 : Form
    {
        
        private JasterThreadExecutor _jasterExecutor;

        public Form1()
        {
            InitializeComponent();
            Global.g_form1 = this;
        }

        public void log_msg(string msg)
        {
            _textBoxLog.AppendText("[ " + DateTime.Now.ToString("HH:mm:ss.fff") + " ]  " + msg + "\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // initialize the Jaster Executor class
            _jasterExecutor = new JasterThreadExecutor();
            
            // initialize the serial port list, maybe have a handler do this every X seconds?
            _comboBoxPort.Items.AddRange(_jasterExecutor.GetPortNames());
            _comboBoxPort.SelectedIndex = 0;
        }


        

        private void _buttonOpen_Click(object sender, EventArgs e)
        {
            if(!_jasterExecutor.ConnectSerial(_comboBoxPort.Text))
            {
                // fail
                MessageBox.Show("FAIL");
            }
        }

        private void _buttonStart_Click(object sender, EventArgs e)
        {
            _jasterExecutor.RunCommands(_textBoxCommands.Text);
        }

        private void _buttonStop_Click(object sender, EventArgs e)
        {
            if (_jasterExecutor.IsSerialOpen())
            {
                // reset Jasters
                if(!_jasterExecutor.CommandAllJastersReset())
                {
                    // fail
                    MessageBox.Show("fail");
                }
            }
        }
    }
}