using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO;

namespace TaRU_Jaster
{
    public partial class Form1 : MaterialForm
    {
        
        private JasterThreadExecutor _jasterExecutor;
        private bool _scriptTextChanged;
        private string _scriptFullPath;

        public Form1()
        {
            InitializeComponent();

            // Set the window color scheme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            // Update globals so this form's public functions can be called universally
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

            // The script has not been changed, there's no name for the initial script
            _scriptTextChanged = false;
            _scriptFullPath = "";

            // initialize the serial port list, maybe have a handler do this every X seconds?
            _materialComboBoxComPorts.Items.AddRange(_jasterExecutor.GetPortNames());
            _materialComboBoxComPorts.SelectedIndex = 0;

            // Handle drags
            _materialMultiLineTextBoxScript.AllowDrop = true;
            _materialMultiLineTextBoxScript.DragDrop += _materialMultiLineTextBoxScriptDragHandler;

            // Handle text edit in script box
            _materialMultiLineTextBoxScript.TextChanged += _materialMultiLineTextBoxScriptTextChangedHandler;

            // At the very start, disable serial functionality, require connection
            DisableSerialFunctionality();
        }

        public void DisableSerialFunctionality()
        {
            log_msg("Connect to serial for full functionality! Serial can be connected on settings page.");
            return;
        }

        public void EnableSerialFunctionality()
        {
            return;
        }

        private void _materialButtonComConnect_Click(object sender, EventArgs e)
        {
            if (!_jasterExecutor.ConnectSerial(_materialComboBoxComPorts.Text))
            {
                // fail
                MessageBox.Show("FAIL");
            }
        }

        private void _materialButtonComRefresh_Click(object sender, EventArgs e)
        {
            string[] ports = _jasterExecutor.GetPortNames();
            _materialComboBoxComPorts.Items.AddRange(ports);
            _materialComboBoxComPorts.SelectedIndex = 0;
            log_msg("Found " + ports.Length + " COM port(s).");
        }

        private void _materialButtonStartPauseScript_Click(object sender, EventArgs e)
        {
            _jasterExecutor.RunCommands(_materialMultiLineTextBoxScript.Text);
        }

        private void _materialButtonStopScript_Click(object sender, EventArgs e)
        {
            // TODO
        }


        /*
         * EVENT HANDLERS
         */

        private void _materialMultiLineTextBoxScriptDragHandler(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if(files != null && files.Length == 1)
            {
                log_msg(Path.GetFileName(files[0]));
                log_msg(Path.GetDirectoryName(files[0]));
                _materialMultiLineTextBoxScript.Text = System.IO.File.ReadAllText(files[0]);
                _scriptFullPath = files[0];
                _materialLabelScriptName.Text = Path.GetFileName(files[0]);
                _scriptTextChanged = false;
            }
        }

        private void _materialMultiLineTextBoxScriptTextChangedHandler(object sender, EventArgs e)
        {
            // special case for uninitialized scripts
            if(_materialMultiLineTextBoxScript.Text.Length == 0 && _scriptFullPath.Length == 0)
            {
                _scriptTextChanged = false;
                _materialLabelScriptName.Text = "Unsaved script";
            }

            else if(!_scriptTextChanged)
            {
                _scriptTextChanged = true;
                _materialLabelScriptName.Text = _materialLabelScriptName.Text + " *";
            }
        }

    }
}
