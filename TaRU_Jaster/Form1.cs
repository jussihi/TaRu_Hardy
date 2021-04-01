using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using System.Threading;
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO;

using System.Reflection;

namespace TaRU_Jaster
{
    public partial class Form1 : MaterialForm
    {

        /*
         * STRUCT ETC DATATYPE DEFINITIONS
         */
        struct TargetSettings
        {
            public MaterialButton buttonInstance;
            public bool enabled;
            public int sensitivity;
            public int hitsToFall;
            public bool lightOn;
            public bool motionOn;
        }

        

        private JasterThreadExecutor _jasterExecutor;
        private bool _scriptTextChanged;
        private string _scriptFullPath;
        private MaterialSkinManager _materialSkinManager;
        private TargetSettings[] _targetSettings;
        

        public Form1()
        {
            InitializeComponent();

            // Set the window color scheme
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            _materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            // Update globals so this form's public functions can be called universally
            Global.g_form1 = this;

            // Initialize target settings
            _targetSettings = new TargetSettings[30];

            // initialize the default target settings
            for (int i = 0; i < _targetSettings.Length; i++)
            {
                _targetSettings[i].enabled = false;
                _targetSettings[i].sensitivity = 50;
                _targetSettings[i].hitsToFall = 1;
                _targetSettings[i].motionOn = false;
                _targetSettings[i].lightOn = false;
            }

            //_targetSettings[3].buttonInstance.ForeColor = Color.White;
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
            if(_materialComboBoxComPorts.Items.Count > 0)
                _materialComboBoxComPorts.SelectedIndex = 0;

            // Handle drags
            _materialMultiLineTextBoxScript.AllowDrop = true;
            _materialMultiLineTextBoxScript.DragDrop += _materialMultiLineTextBoxScriptDragHandler;

            // Handle text edit in script box
            _materialMultiLineTextBoxScript.TextChanged += _materialMultiLineTextBoxScriptTextChangedHandler;

            // At the very start, disable serial functionality, require connection
            DisableSerialFunctionality();

            // Set com status changed handler
            _jasterExecutor.ComStatusChanged += ComStatusChangedHandler;

            // Set the button instances to TargetSettings struct
            for (int i = 1; i <= 30; i++)
            {
                Control c = GetControlByName(this, "_materialButtonSelectTargetSimple" + i);
                if (c != null)
                {
                    MaterialButton b = c as MaterialButton;
                    if (b == null)
                    {
                        MessageBox.Show("fail");
                        continue;
                    }
                    _targetSettings[i - 1].buttonInstance = b;
                }
            }

        }

        private Control GetControlByName(Control ParentCntl, string NameToSearch)
        {
            if (ParentCntl.Name == NameToSearch)
                return ParentCntl;

            foreach (Control ChildCntl in ParentCntl.Controls)
            {
                Control ResultCntl = GetControlByName(ChildCntl, NameToSearch);
                if (ResultCntl != null)
                    return ResultCntl;
            }
            return null;
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

        private void ComStatusChangedHandler(object sender, EventArgs e)
        {
            MessageBox.Show("COM status changed");
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
            _materialComboBoxComPorts.Items.Clear();
            _materialComboBoxComPorts.Items.AddRange(ports);
            if(_materialComboBoxComPorts.Items.Count > 0)
                _materialComboBoxComPorts.SelectedIndex = 0;
            log_msg("Found " + ports.Length + " COM port(s).");
        }

        private async void _materialButtonStartPauseScript_Click(object sender, EventArgs e)
        {
            await _jasterExecutor.RunCommands(_materialMultiLineTextBoxScript.Text);
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



        /*
         * TARGET BUTTON CLICKS
         */

        private void ToggleSingleTargetSimple(int w_targetNo)
        {
            _targetSettings[w_targetNo].buttonInstance.UseAccentColor = !_targetSettings[w_targetNo].buttonInstance.UseAccentColor;
        }

        private void _selectAllTargetsSimple_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 30; i++)
            {
                _targetSettings[i].buttonInstance.UseAccentColor = true;
            }
        }

        private void _deselectAllTargetsSimple_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 30; i++)
            {
                _targetSettings[i].buttonInstance.UseAccentColor = false;
            }
        }

        private void ShowTargetToolTip(int w_targetNo, IWin32Window w_window)
        {
            w_targetNo = w_targetNo - 1;
            toolTip1.Show("Target  " + (w_targetNo + 1).ToString() + "\n" +
                "Sensitivity: " + _targetSettings[w_targetNo].sensitivity.ToString() + "\n" +
                "Hits to fall: " + _targetSettings[w_targetNo].hitsToFall.ToString() + "\n" +
                "Light on: " + _targetSettings[w_targetNo].lightOn.ToString() + "\n" +
                "Motion on: " + _targetSettings[w_targetNo].motionOn.ToString()
                , _targetSettings[w_targetNo].buttonInstance);
        }



        private void _materialButtonSelectTargetSimple1_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(0);
        }

        private void _materialButtonSelectTargetSimple2_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(1);
        }

        private void _materialButtonSelectTargetSimple3_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(2);
        }

        private void _materialButtonSelectTargetSimple4_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(3);
        }

        private void _materialButtonSelectTargetSimple5_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(4);
        }

        private void _materialButtonSelectTargetSimple6_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(5);
        }

        private void _materialButtonSelectTargetSimple7_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(6);
        }

        private void _materialButtonSelectTargetSimple8_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(7);
        }

        private void _materialButtonSelectTargetSimple9_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(8);
        }

        private void _materialButtonSelectTargetSimple10_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(9);
        }

        private void _materialButtonSelectTargetSimple11_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(10);
        }

        private void _materialButtonSelectTargetSimple12_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(11);
        }

        private void _materialButtonSelectTargetSimple13_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(12);
        }

        private void _materialButtonSelectTargetSimple14_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(13);
        }

        private void _materialButtonSelectTargetSimple15_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(14);
        }

        private void _materialButtonSelectTargetSimple16_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(15);
        }

        private void _materialButtonSelectTargetSimple17_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(16);
        }

        private void _materialButtonSelectTargetSimple18_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(17);
        }

        private void _materialButtonSelectTargetSimple19_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(18);
        }

        private void _materialButtonSelectTargetSimple20_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(19);
        }

        private void _materialButtonSelectTargetSimple21_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(20);
        }

        private void _materialButtonSelectTargetSimple22_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(21);
        }

        private void _materialButtonSelectTargetSimple23_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(22);
        }

        private void _materialButtonSelectTargetSimple24_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(23);
        }

        private void _materialButtonSelectTargetSimple25_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(24);
        }

        private void _materialButtonSelectTargetSimple26_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(25);
        }

        private void _materialButtonSelectTargetSimple27_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(26);
        }

        private void _materialButtonSelectTargetSimple28_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(27);
        }

        private void _materialButtonSelectTargetSimple29_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(28);
        }

        private void _materialButtonSelectTargetSimple30_Click(object sender, EventArgs e)
        {
            ToggleSingleTargetSimple(29);
        }

        private void _materialButtonSelectTargetSimple1_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(1, _materialButtonSelectTargetSimple1);
        }

        private void _materialButtonSelectTargetSimple2_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(2, _materialButtonSelectTargetSimple2);
        }

        private void _materialButtonSelectTargetSimple3_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(3, _materialButtonSelectTargetSimple3);
        }

        private void _materialButtonSelectTargetSimple4_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(4, _materialButtonSelectTargetSimple4);
        }

        private void _materialButtonSelectTargetSimple5_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(5, _materialButtonSelectTargetSimple5);
        }

        private void _materialButtonSelectTargetSimple6_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(6, _materialButtonSelectTargetSimple6);
        }

        private void _materialButtonSelectTargetSimple7_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(7, _materialButtonSelectTargetSimple7);
        }

        private void _materialButtonSelectTargetSimple8_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(8, _materialButtonSelectTargetSimple8);
        }

        private void _materialButtonSelectTargetSimple9_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(9, _materialButtonSelectTargetSimple9);
        }

        private void _materialButtonSelectTargetSimple10_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(10, _materialButtonSelectTargetSimple10);
        }

        private void _materialButtonSelectTargetSimple11_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(11, _materialButtonSelectTargetSimple11);
        }

        private void _materialButtonSelectTargetSimple12_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(12, _materialButtonSelectTargetSimple12);
        }

        private void _materialButtonSelectTargetSimple13_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(13, _materialButtonSelectTargetSimple13);
        }

        private void _materialButtonSelectTargetSimple14_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(14, _materialButtonSelectTargetSimple14);
        }

        private void _materialButtonSelectTargetSimple15_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(15, _materialButtonSelectTargetSimple15);
        }

        private void _materialButtonSelectTargetSimple16_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(16, _materialButtonSelectTargetSimple16);
        }

        private void _materialButtonSelectTargetSimple17_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(17, _materialButtonSelectTargetSimple17);
        }

        private void _materialButtonSelectTargetSimple18_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(18, _materialButtonSelectTargetSimple18);
        }

        private void _materialButtonSelectTargetSimple19_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(19, _materialButtonSelectTargetSimple19);
        }

        private void _materialButtonSelectTargetSimple20_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(20, _materialButtonSelectTargetSimple20);
        }

        private void _materialButtonSelectTargetSimple21_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(21, _materialButtonSelectTargetSimple21);
        }

        private void _materialButtonSelectTargetSimple22_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(22, _materialButtonSelectTargetSimple22);
        }

        private void _materialButtonSelectTargetSimple23_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(23, _materialButtonSelectTargetSimple23);
        }

        private void _materialButtonSelectTargetSimple24_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(24, _materialButtonSelectTargetSimple24);
        }

        private void _materialButtonSelectTargetSimple25_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(25, _materialButtonSelectTargetSimple25);
        }

        private void _materialButtonSelectTargetSimple26_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(26, _materialButtonSelectTargetSimple26);
        }

        private void _materialButtonSelectTargetSimple27_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(27, _materialButtonSelectTargetSimple27);
        }

        private void _materialButtonSelectTargetSimple28_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(28, _materialButtonSelectTargetSimple28);
        }

        private void _materialButtonSelectTargetSimple29_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(29, _materialButtonSelectTargetSimple29);
        }

        private void _materialButtonSelectTargetSimple30_MouseHover(object sender, EventArgs e)
        {
            ShowTargetToolTip(30, _materialButtonSelectTargetSimple30);
        }

    }
}
