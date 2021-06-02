﻿using System;
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

using System.Diagnostics;

using System.IO;

using System.Reflection;

using System.Windows.Forms;

using System.Drawing;
using ScintillaNET;

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
            public bool up;
            public float battery;
            public DateTime lastUpdate;
        }

        

        private JasterThreadExecutor _jasterExecutor;
        private bool _scriptTextChanged;
        private string _scriptFullPath;
        private MaterialSkinManager _materialSkinManager;
        private TargetSettings[] _targetSettings;
        private ListViewColumnSorter _lvwColumnSorter;


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
                _targetSettings[i].sensitivity = 0;
                _targetSettings[i].hitsToFall = 0;
                _targetSettings[i].motionOn = false;
                _targetSettings[i].lightOn = false;
                _targetSettings[i].up = false;
                _targetSettings[i].battery = 0.0f;
                _targetSettings[i].lastUpdate = DateTime.MinValue;
            }

            // initialize target list sorter
            _lvwColumnSorter = new ListViewColumnSorter();
            this.materialListView1.ListViewItemSorter = _lvwColumnSorter;

            
            // initialize tab images
            string[] tab_images = new string[]{ "avatar.png", "edit.png", "settings.png" };

            foreach (string tab_image in tab_images)
            {
                var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TaRU_Jaster.Resources." + tab_image);
                if (imageStream != null)
                {
                    var image = Image.FromStream(imageStream);
                    imageList1.Images.Add(image);
                }
                else
                    Debug.WriteLine("null tabanme");
            }

            // Set the image list for the tablayout
            materialTabControl1.ImageList = imageList1;

            // initialize connection status icons
            string[] conn_images = new string[] { "OK.png", "PENDING.png", "cancel.png" };

            foreach (string conn_image in conn_images)
            {
                var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TaRU_Jaster.Resources." + conn_image);
                if (imageStream != null)
                {
                    var image = Image.FromStream(imageStream);
                    imageList2.Images.Add(image);
                }
                else
                    Debug.WriteLine("null conname\n");
            }


            //_targetSettings[3].buttonInstance.ForeColor = Color.White;

            InitializeCodeBox();

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
                        log_msg("ERROR: could not find Button for target " + i);
                        continue;
                    }
                    _targetSettings[i - 1].buttonInstance = b;
                }
            }

            // Update / add targets to the target list
            for(int i = 0; i < 30; i++)
            {
                UpdateTargetList(i);
            }

            // Set initial status for connection
            materialLabel14.Text = "Disconnected";
            pictureBox1.Image = imageList2.Images[2];
        }

        private void InitializeCodeBox()
        {
            // Initialize the code box
            CodeTextBox.Styles[Style.Default].Font = "Consolas";
            CodeTextBox.Styles[Style.Default].Size = 12;
            //CodeTextBox.Styles[Style.Default].BackColor = IntToColor(0x212121);
            //CodeTextBox.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);

            // Configure the CPP (C#) lexer styles
            CodeTextBox.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0x2a2a2a);
            CodeTextBox.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            CodeTextBox.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            CodeTextBox.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            CodeTextBox.Styles[Style.Cpp.Number].ForeColor = IntToColor(0x00750a);
            CodeTextBox.Styles[Style.Cpp.String].ForeColor = IntToColor(0x00750a);
            CodeTextBox.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            CodeTextBox.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            CodeTextBox.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0x2a2a2a);
            CodeTextBox.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            CodeTextBox.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            CodeTextBox.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            CodeTextBox.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            CodeTextBox.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            CodeTextBox.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            CodeTextBox.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            CodeTextBox.Lexer = Lexer.Cpp;

            CodeTextBox.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            CodeTextBox.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

            // init line numbering
            var nums = CodeTextBox.Margins[1];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            // Handle changing text in CodeTextBox
            CodeTextBox.TextChanged += CodeTextBoxScriptTextChangedHandler;

            // Handle drags
            CodeTextBox.AllowDrop = true;
            CodeTextBox.DragEnter += delegate (object sender, DragEventArgs e) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            CodeTextBox.DragDrop += CodeTextBoxScriptDragHandler;
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

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void ComStatusChangedHandler(object sender, EventArgs e)
        {
            switch(_jasterExecutor.pComStatus)
            {
                case JasterThreadExecutor.ComStatus.Connected:
                    materialLabel14.Text = "Connected";
                    pictureBox1.Image = imageList2.Images[0];
                    break;

                case JasterThreadExecutor.ComStatus.Disconnected:
                    materialLabel14.Text = "Disconnected";
                    pictureBox1.Image = imageList2.Images[2];
                    break;

                case JasterThreadExecutor.ComStatus.Sending:
                    materialLabel14.Text = "Sending";
                    pictureBox1.Image = imageList2.Images[1];
                    break;

                case JasterThreadExecutor.ComStatus.Receiving:
                    materialLabel14.Text = "Receiving";
                    pictureBox1.Image = imageList2.Images[1];
                    break;
            }
        }

        private void _materialButtonComConnect_Click(object sender, EventArgs e)
        {
            if (!_jasterExecutor.ConnectSerial(_materialComboBoxComPorts.Text))
            {
                // fail
                MessageBox.Show("Could not connect to serial!");
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
            await _jasterExecutor.RunCommands(CodeTextBox.Text);
        }

        private void _materialButtonStopScript_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void UpdateTargetList(int w_targetNo)
        {
            // Check if it exists already
            foreach(ListViewItem entry in materialListView1.Items)
            {
                if(entry.Text.Equals((w_targetNo + 1).ToString()))
                {
                    entry.Remove();
                }
            }

            ListViewItem item = new ListViewItem((w_targetNo + 1).ToString());
            item.SubItems.Add(_targetSettings[w_targetNo].lastUpdate == DateTime.MinValue ? "N/A" : _targetSettings[w_targetNo].lastUpdate.ToString());
            item.SubItems.Add(_targetSettings[w_targetNo].lightOn ? "Yes" : "No");
            item.SubItems.Add(_targetSettings[w_targetNo].hitsToFall.ToString());
            item.SubItems.Add(_targetSettings[w_targetNo].battery.ToString());
            item.SubItems.Add(_targetSettings[w_targetNo].sensitivity.ToString());
            materialListView1.Items.Add(item);

            materialListView1.Sort();
        }


        /*
         * EVENT HANDLERS
         */
        private void CodeTextBoxScriptDragHandler(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files != null && files.Length == 1)
            {
                log_msg(Path.GetFileName(files[0]));
                log_msg(Path.GetDirectoryName(files[0]));
                CodeTextBox.Text = System.IO.File.ReadAllText(files[0]);
                _scriptFullPath = files[0];
                _materialLabelScriptName.Text = Path.GetFileName(files[0]);
                _scriptTextChanged = false;
            }
        }

        private void CodeTextBoxScriptTextChangedHandler(object sender, EventArgs e)
        {
            // special case for uninitialized scripts
            if (CodeTextBox.Text.Length == 0 && _scriptFullPath.Length == 0)
            {
                _scriptTextChanged = false;
                _materialLabelScriptName.Text = "Unsaved script";
            }

            else if (!_scriptTextChanged)
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

        private byte[] addTargetNumberToAddress(int w_target, byte[] w_addressBytes)
        {
            // place the dummy to right position to create the address
            w_addressBytes[(w_target - 1) / 7] |= (byte)(0b_0000_0001 << ((w_target - 1) % 7));

            // return address array
            return w_addressBytes;
        }

        enum OneShotCommand
        {
            Up,
            Down,
            SetSensitivity,
            SetHitsToFall,
            SetLightsOn,
            SetLightsOff,
            SetMotionOn,
            SetMotionOff,
            SetProgramHitsToFall,
            SetProgramTimeUp,
            SetProgramTimeDown,
            StartProgram,
            EndProgram,
            Reset
        }


        private async Task<bool> oneShotTargetsSimpleExecute(OneShotCommand w_command, byte w_param = 0xFF)
        {
            // Initialize the needed byte buffers
            byte[] command   = null;
            byte[] address   = new byte[5];
            byte[] nullBytes = null;

            // Set up target list from currently activated targets
            var targetList = new List<int>();

            for (int i = 1; i < 31; i++)
            {
                if (_targetSettings[i - 1].buttonInstance.UseAccentColor)
                    targetList.Add(i);
            }

            bool allTargets = (targetList.Count == 30 || targetList.Count == 0);
            

            // Set target array (unset if needed in command switch!)
            foreach (int targetNo in targetList)
            {
                address = addTargetNumberToAddress(targetNo, address);
            }

            // Set up the command-specific commands
            switch (w_command)
            {
                case OneShotCommand.Up:
                    if(allTargets)
                    {
                        command = new byte[] { 0x80 };
                        address = null;
                        break;
                    }
                    command   = new byte[] { 0x82 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.Down:
                    if (allTargets)
                    {
                        command = new byte[] { 0x81 };
                        address = null;
                        break;
                    }
                    command   = new byte[] { 0x83 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetSensitivity:
                    // Check that the sensitivity is within limits
                    if(w_param > 99)
                    {
                        MessageBox.Show("Sensitivity must be between 0-99!");
                        log_msg("Sensitivity value not set or invalid: " + w_param);
                        return false;
                    }
                    if(allTargets)
                    {
                        command   = new byte[] { 0x92 };
                        address   = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command   = new byte[] { 0x92 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetHitsToFall:
                    // Check that the hitstofall is within limits
                    if (w_param > 99 || w_param == 0)
                    {
                        MessageBox.Show("Number of hits to fall must be between 1-99!");
                        log_msg("Number of hits to fall value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command   = new byte[] { 0x8C };
                        address   = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command   = new byte[] { 0x8C };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetLightsOn:
                    if (allTargets)
                    {
                        command = new byte[] { 0x88 };
                        address = null;
                        break;
                    }
                    command   = new byte[] { 0x9C };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetLightsOff:
                    if (allTargets)
                    {
                        command = new byte[] { 0x89 };
                        address = null;
                        break;
                    }
                    command   = new byte[] { 0x9D };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetMotionOn:
                    if (allTargets)
                    {
                        command   = new byte[] { 0x9E };
                        address   = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command   = new byte[] { 0x9E };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetMotionOff:
                    if (allTargets)
                    {
                        command   = new byte[] { 0x9F };
                        address   = null;
                        nullBytes = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F,
                                                 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F };
                        break;
                    }
                    command   = new byte[] { 0x9F };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramHitsToFall:
                    // Check that the hitstofall is within limits
                    if (w_param > 99 || w_param == 0)
                    {
                        MessageBox.Show("Number of hits to fall must be between 1-99!");
                        log_msg("Number of hits to fall value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x8F };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x8F };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramTimeUp:
                    // Check that the hitstofall is within limits
                    if (w_param > 99)
                    {
                        MessageBox.Show("Value of timeup must be between 0-99!");
                        log_msg("Timeup value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x93 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x93 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.SetProgramTimeDown:
                    // Check that the hitstofall is within limits
                    if (w_param > 99)
                    {
                        MessageBox.Show("Value of timedown must be between 0-99!");
                        log_msg("Timedown value not set or invalid: " + w_param);
                        return false;
                    }
                    if (allTargets)
                    {
                        command = new byte[] { 0x94 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x94 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.StartProgram:
                    if (allTargets)
                    {
                        command = new byte[] { 0x95 };
                        address = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x95 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.EndProgram:
                    if (allTargets)
                    {
                        command = new byte[] { 0x8F };
                        address = new byte[] { 0x01, 0x7F, 0x7F, 0x7F, 0x7F, 0x03 };
                        nullBytes = new byte[8];
                        break;
                    }
                    command = new byte[] { 0x96 };
                    nullBytes = new byte[8];
                    break;
                case OneShotCommand.Reset:
                    if (allTargets)
                    {
                        command = new byte[] { 0x91 };
                        address = null;
                        break;
                    }
                    command = new byte[] { 0x9B };
                    nullBytes = new byte[8];
                    break;
            }

            // compile the final command and send it out
            if(command == null)
            {
                MessageBox.Show("No command given !");
                log_msg("ERROR: No command given!");
            }

            // append param if it's present
            if(w_param != 0xFF)
            {
                command = command.Concat(new byte[] { w_param }).ToArray();
            }

            // append address if it's present
            if (address != null)
            {
                command = command.Concat(address).ToArray();
            }

            // append null bytes if it's present
            if (nullBytes != null)
            {
                command = command.Concat(nullBytes).ToArray();
            }

            // Finally, execute the command
            await _jasterExecutor.SendSerial(command);
            return true;
        }

        private async void _upSelectedTargetsSimple_Click(object sender, EventArgs e)
        {
            await oneShotTargetsSimpleExecute(OneShotCommand.Up);
            return;
        }

        private async void _downSelectedTargetsSimple_Click(object sender, EventArgs e)
        {
            await oneShotTargetsSimpleExecute(OneShotCommand.Down);
            return;
        }

        private async void _resetSelectedTargetsSimple_Click(object sender, EventArgs e)
        {
            await oneShotTargetsSimpleExecute(OneShotCommand.Reset);
            return;
        }

        private async void _materialButtonGoQuickProgram_Click(object sender, EventArgs e)
        {
            int hitsToFall = 0, timeUp = 0, timeDown = 0;

            try
            {
                hitsToFall = int.Parse(materialTextBox3.Text);
                if (hitsToFall < 1 || hitsToFall > 20)
                    throw new ArgumentException("Hits to fall must be between 1 and 19!");
            }
            catch(Exception ex)
            {
                log_msg("ERROR: trying to parse integer value hits to fall, exception: " + ex.Message);
                return;
            }

            try
            {
                timeUp = int.Parse(materialTextBox4.Text);
                if (timeUp < 0 || timeUp > 99)
                    throw new ArgumentException("Time up must be between 0 and 99 (RND)!");
            }
            catch (Exception ex)
            {
                log_msg("ERROR: trying to parse integer value time up, exception: " + ex.Message);
                return;
            }

            try
            {
                timeDown = int.Parse(materialTextBox5.Text);
                if (timeDown < 0 || timeDown > 99)
                    throw new ArgumentException("Time down must be between 0 and 99 (RND)!");
            }
            catch (Exception ex)
            {
                log_msg("ERROR: trying to parse integer value time down, exception: " + ex.Message);
                return;
            }

            // all values are validated, go on ...

            await oneShotTargetsSimpleExecute(OneShotCommand.SetProgramHitsToFall, (byte)hitsToFall);
            await oneShotTargetsSimpleExecute(OneShotCommand.SetProgramTimeUp, (byte)timeUp);
            await oneShotTargetsSimpleExecute(OneShotCommand.SetProgramTimeDown, (byte)timeDown);
            await oneShotTargetsSimpleExecute(OneShotCommand.StartProgram);
        }

        private async void _materialButtonEndQuickProgram_Click(object sender, EventArgs e)
        {
            await oneShotTargetsSimpleExecute(OneShotCommand.EndProgram);
        }

        private async void _materialButtonSetQuickConfig_Click(object sender, EventArgs e)
        {
            int sensitivity = 0, hitsToFall = 0;

            try
            {
                sensitivity = int.Parse(materialTextBox1.Text);
                if (sensitivity < 1 || sensitivity > 20)
                    throw new ArgumentException("Hits to fall must be between 0 and 99!");
                await oneShotTargetsSimpleExecute(OneShotCommand.SetSensitivity, (byte)sensitivity);
            }
            catch (Exception ex)
            {
                log_msg("WARNING: trying to parse integer value sensitivity, exception: " + ex.Message + ". Not setting value.");
            }

            try
            {
                hitsToFall = int.Parse(materialTextBox2.Text);
                if (hitsToFall < 1 || hitsToFall > 20)
                    throw new ArgumentException("Hits to fall must be between 1 and 19!");
                await oneShotTargetsSimpleExecute(OneShotCommand.SetHitsToFall, (byte)hitsToFall);
            }
            catch (Exception ex)
            {
                log_msg("WARNING: trying to parse integer value hits to fall, exception: " + ex.Message + ". Not setting value.");
            }
            
            switch(materialCheckbox2.Checked)
            {
                case true:
                    await oneShotTargetsSimpleExecute(OneShotCommand.SetLightsOn);
                    break;
                default:
                    await oneShotTargetsSimpleExecute(OneShotCommand.SetLightsOff);
                    break;
            }

            switch (materialCheckbox1.Checked)
            {
                case true:
                    await oneShotTargetsSimpleExecute(OneShotCommand.SetMotionOn);
                    break;
                default:
                    await oneShotTargetsSimpleExecute(OneShotCommand.SetMotionOff);
                    break;
            }


        }

        private async void _materialButtonAskStats_Click(object sender, EventArgs e)
        {
            var targetList = new List<int>();

            for (int i = 1; i < 31; i++)
            {
                if (_targetSettings[i - 1].buttonInstance.UseAccentColor)
                    targetList.Add(i);
            }

            // Add all targets if none selected
            if(targetList.Count == 0)
            {
                targetList.AddRange(Enumerable.Range(1, 30));
            }

            int targetResponses = 0;

            foreach (int targetNo in targetList)
            {
                byte[] command = { 0x90, 0x00 };
                command[1] = (byte)targetNo;
                
                await _jasterExecutor.SendSerial(command);
                byte[] res = await _jasterExecutor.ReadSerial(6);
                if (res == null)
                    continue;

                // Set states
                bool help;
                help = (res[0] & 0x20) == 0x20 ? (_targetSettings[targetNo - 1].up = true) : (_targetSettings[targetNo - 1].up = false);
                help = (res[0] & 0x08) == 0x08 ? (_targetSettings[targetNo - 1].lightOn = true) : (_targetSettings[targetNo - 1].lightOn = false);

                // Set hits to fall
                _targetSettings[targetNo - 1].hitsToFall = res[1];

                // Bytes 2-3 unknown ???

                // Set battery level
                _targetSettings[targetNo - 1].battery = ((float)((int)res[4]) / 10.0f) + 3.0f;

                // Set sensitivity
                _targetSettings[targetNo - 1].sensitivity = res[5];

                // Set target enabled (since we were able to connect) and set latest timestamp
                _targetSettings[targetNo - 1].enabled = true;
                _targetSettings[targetNo - 1].lastUpdate = DateTime.Now;

                // Update target
                UpdateTargetList(targetNo - 1);
                targetResponses++;

                log_msg("Successfully received serial data from target " + targetNo + ": " + ByteArrayToString(res));
            }

            if(targetResponses > 0)
                materialTabControl1.SelectedTab = materialTabControl1.TabPages["tabPage3"];
        }

        private async void _materialButtonAskHits_Click(object sender, EventArgs e)
        {
            var targetList = new List<int>();

            for (int i = 1; i < 31; i++)
            {
                if (_targetSettings[i - 1].buttonInstance.UseAccentColor)
                    targetList.Add(i);
            }

            // Add all targets if none selected
            if (targetList.Count == 0)
            {
                targetList.AddRange(Enumerable.Range(1, 30));
            }

            var targetHitsList = new List<HitsForm.TargetHits>();

            foreach (int targetNo in targetList)
            {
                byte[] command = { 0x86, 0x00 };
                command[1] = (byte)targetNo;

                await _jasterExecutor.SendSerial(command);
                byte[] res = await _jasterExecutor.ReadSerial(6);
                if (res == null)
                    continue;
                // TODO: do something with data
                log_msg("Successfully received serial data from target " + targetNo + ": " + ByteArrayToString(res));

                HitsForm.TargetHits entry = new HitsForm.TargetHits {  
                                                targetNo     = targetNo, 
                                                overallHits  = (short)(res[0] | (res[1] << 8)), 
                                                riseCount    = (short)(res[2] | (res[3] << 8)), 
                                                hitFallCount = (short)(res[4] | (res[5] << 8))
                };

                targetHitsList.Add(entry);
            }

            //if(targetHitsList.Count == 0)
            //{
            //    MessageBox.Show("No targets answered to HITS request!");
            //    return;
            //}

            HitsForm f = new HitsForm();
            f.SetTargetHitsList(targetHitsList);
            f.Show();
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("0x{0:x2} ", b);
            return hex.ToString();
        }

        private void ShowTargetToolTip(int w_targetNo, IWin32Window w_window)
        {
            w_targetNo = w_targetNo - 1;
            if(_targetSettings[w_targetNo].enabled)
            {
                toolTip1.Show("Target  " + (w_targetNo + 1).ToString() + "\n" +
                    "Sensitivity: " + _targetSettings[w_targetNo].sensitivity.ToString() + "\n" +
                    "Hits to fall: " + _targetSettings[w_targetNo].hitsToFall.ToString() + "\n" +
                    "Light on: " + _targetSettings[w_targetNo].lightOn.ToString() + "\n" +
                    "Motion on: " + _targetSettings[w_targetNo].motionOn.ToString() + "\n" +
                    "State: " + (_targetSettings[w_targetNo].up ? "up" : "down"), 
                    _targetSettings[w_targetNo].buttonInstance);
                return;
            }
            toolTip1.Show("Target  " + (w_targetNo + 1).ToString() + "\n" +
                    "No target information, please fetch",
                    _targetSettings[w_targetNo].buttonInstance);
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

        private void materialListView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            if (e.Column == _lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    _lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _lvwColumnSorter.SortColumn = e.Column;
                _lvwColumnSorter.Order = SortOrder.Ascending;
            }

            materialListView1.Sort();
        }

    }
}
