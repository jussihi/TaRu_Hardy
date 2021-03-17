
namespace TaRU_Jaster
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this._textBoxLog = new System.Windows.Forms.TextBox();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._materialButtonStopScript = new MaterialSkin.Controls.MaterialButton();
            this._materialButtonStartPauseScript = new MaterialSkin.Controls.MaterialButton();
            this._materialMultiLineTextBoxScript = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this._materialSwitchAutoResults = new MaterialSkin.Controls.MaterialSwitch();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this._materialButtonComRefresh = new MaterialSkin.Controls.MaterialButton();
            this._materialButtonComConnect = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this._materialComboBoxComPorts = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.materialContextMenuStrip1 = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this._materialButtonSaveScript = new MaterialSkin.Controls.MaterialButton();
            this._materialButtonSaveScriptAs = new MaterialSkin.Controls.MaterialButton();
            this._materialButtonLoadScript = new MaterialSkin.Controls.MaterialButton();
            this._materialButtonGetResults = new MaterialSkin.Controls.MaterialButton();
            this._materialSwitchResetResultsScriptStart = new MaterialSkin.Controls.MaterialSwitch();
            this._materialLabelScriptName = new MaterialSkin.Controls.MaterialLabel();
            this._materialLabelEditingFile = new MaterialSkin.Controls.MaterialLabel();
            this.materialTabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.materialCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _textBoxLog
            // 
            this._textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxLog.Location = new System.Drawing.Point(12, 613);
            this._textBoxLog.Multiline = true;
            this._textBoxLog.Name = "_textBoxLog";
            this._textBoxLog.ReadOnly = true;
            this._textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxLog.Size = new System.Drawing.Size(1004, 295);
            this._textBoxLog.TabIndex = 4;
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.ImageList = this.imageList1;
            this.materialTabControl1.Location = new System.Drawing.Point(12, 25);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(1004, 582);
            this.materialTabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.ImageKey = "avatar.png";
            this.tabPage1.Location = new System.Drawing.Point(4, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(996, 539);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this._materialLabelEditingFile);
            this.tabPage2.Controls.Add(this._materialLabelScriptName);
            this.tabPage2.Controls.Add(this._materialSwitchResetResultsScriptStart);
            this.tabPage2.Controls.Add(this._materialButtonGetResults);
            this.tabPage2.Controls.Add(this._materialButtonLoadScript);
            this.tabPage2.Controls.Add(this._materialButtonSaveScriptAs);
            this.tabPage2.Controls.Add(this._materialButtonSaveScript);
            this.tabPage2.Controls.Add(this._materialButtonStopScript);
            this.tabPage2.Controls.Add(this._materialButtonStartPauseScript);
            this.tabPage2.Controls.Add(this._materialMultiLineTextBoxScript);
            this.tabPage2.Controls.Add(this._materialSwitchAutoResults);
            this.tabPage2.ImageKey = "edit.png";
            this.tabPage2.Location = new System.Drawing.Point(4, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(996, 539);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // _materialButtonStopScript
            // 
            this._materialButtonStopScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonStopScript.AutoSize = false;
            this._materialButtonStopScript.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonStopScript.Depth = 0;
            this._materialButtonStopScript.DrawShadows = true;
            this._materialButtonStopScript.HighEmphasis = true;
            this._materialButtonStopScript.Icon = null;
            this._materialButtonStopScript.Location = new System.Drawing.Point(682, 427);
            this._materialButtonStopScript.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonStopScript.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonStopScript.Name = "_materialButtonStopScript";
            this._materialButtonStopScript.Size = new System.Drawing.Size(178, 36);
            this._materialButtonStopScript.TabIndex = 3;
            this._materialButtonStopScript.Text = "Stop";
            this._materialButtonStopScript.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonStopScript.UseAccentColor = false;
            this._materialButtonStopScript.UseVisualStyleBackColor = true;
            this._materialButtonStopScript.Click += new System.EventHandler(this._materialButtonStopScript_Click);
            // 
            // _materialButtonStartPauseScript
            // 
            this._materialButtonStartPauseScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonStartPauseScript.AutoSize = false;
            this._materialButtonStartPauseScript.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonStartPauseScript.Depth = 0;
            this._materialButtonStartPauseScript.DrawShadows = true;
            this._materialButtonStartPauseScript.HighEmphasis = true;
            this._materialButtonStartPauseScript.Icon = null;
            this._materialButtonStartPauseScript.Location = new System.Drawing.Point(682, 379);
            this._materialButtonStartPauseScript.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonStartPauseScript.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonStartPauseScript.Name = "_materialButtonStartPauseScript";
            this._materialButtonStartPauseScript.Size = new System.Drawing.Size(178, 36);
            this._materialButtonStartPauseScript.TabIndex = 2;
            this._materialButtonStartPauseScript.Text = "Start";
            this._materialButtonStartPauseScript.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonStartPauseScript.UseAccentColor = false;
            this._materialButtonStartPauseScript.UseVisualStyleBackColor = true;
            this._materialButtonStartPauseScript.Click += new System.EventHandler(this._materialButtonStartPauseScript_Click);
            // 
            // _materialMultiLineTextBoxScript
            // 
            this._materialMultiLineTextBoxScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._materialMultiLineTextBoxScript.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._materialMultiLineTextBoxScript.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._materialMultiLineTextBoxScript.Depth = 0;
            this._materialMultiLineTextBoxScript.DetectUrls = false;
            this._materialMultiLineTextBoxScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._materialMultiLineTextBoxScript.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._materialMultiLineTextBoxScript.Hint = "";
            this._materialMultiLineTextBoxScript.Location = new System.Drawing.Point(6, 39);
            this._materialMultiLineTextBoxScript.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialMultiLineTextBoxScript.Name = "_materialMultiLineTextBoxScript";
            this._materialMultiLineTextBoxScript.Size = new System.Drawing.Size(669, 472);
            this._materialMultiLineTextBoxScript.TabIndex = 1;
            this._materialMultiLineTextBoxScript.Text = "";
            // 
            // _materialSwitchAutoResults
            // 
            this._materialSwitchAutoResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialSwitchAutoResults.AutoSize = true;
            this._materialSwitchAutoResults.Depth = 0;
            this._materialSwitchAutoResults.Location = new System.Drawing.Point(678, 323);
            this._materialSwitchAutoResults.Margin = new System.Windows.Forms.Padding(0);
            this._materialSwitchAutoResults.MouseLocation = new System.Drawing.Point(-1, -1);
            this._materialSwitchAutoResults.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialSwitchAutoResults.Name = "_materialSwitchAutoResults";
            this._materialSwitchAutoResults.Ripple = true;
            this._materialSwitchAutoResults.Size = new System.Drawing.Size(196, 37);
            this._materialSwitchAutoResults.TabIndex = 0;
            this._materialSwitchAutoResults.Text = "Auto-collect results ";
            this._materialSwitchAutoResults.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.materialCard1);
            this.tabPage3.ImageKey = "settings.png";
            this.tabPage3.Location = new System.Drawing.Point(4, 39);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(996, 539);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // materialCard1
            // 
            this.materialCard1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this._materialButtonComRefresh);
            this.materialCard1.Controls.Add(this._materialButtonComConnect);
            this.materialCard1.Controls.Add(this.materialLabel2);
            this.materialCard1.Controls.Add(this._materialComboBoxComPorts);
            this.materialCard1.Controls.Add(this.materialLabel1);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(528, 17);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(454, 216);
            this.materialCard1.TabIndex = 1;
            // 
            // _materialButtonComRefresh
            // 
            this._materialButtonComRefresh.AutoSize = false;
            this._materialButtonComRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonComRefresh.Depth = 0;
            this._materialButtonComRefresh.DrawShadows = true;
            this._materialButtonComRefresh.HighEmphasis = true;
            this._materialButtonComRefresh.Icon = null;
            this._materialButtonComRefresh.Location = new System.Drawing.Point(278, 142);
            this._materialButtonComRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonComRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonComRefresh.Name = "_materialButtonComRefresh";
            this._materialButtonComRefresh.Size = new System.Drawing.Size(158, 48);
            this._materialButtonComRefresh.TabIndex = 3;
            this._materialButtonComRefresh.Text = "REfresh ports";
            this._materialButtonComRefresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonComRefresh.UseAccentColor = false;
            this._materialButtonComRefresh.UseVisualStyleBackColor = true;
            this._materialButtonComRefresh.Click += new System.EventHandler(this._materialButtonComRefresh_Click);
            // 
            // _materialButtonComConnect
            // 
            this._materialButtonComConnect.AutoSize = false;
            this._materialButtonComConnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonComConnect.Depth = 0;
            this._materialButtonComConnect.DrawShadows = true;
            this._materialButtonComConnect.HighEmphasis = true;
            this._materialButtonComConnect.Icon = null;
            this._materialButtonComConnect.Location = new System.Drawing.Point(278, 68);
            this._materialButtonComConnect.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonComConnect.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonComConnect.Name = "_materialButtonComConnect";
            this._materialButtonComConnect.Size = new System.Drawing.Size(158, 48);
            this._materialButtonComConnect.TabIndex = 2;
            this._materialButtonComConnect.Text = "Connect";
            this._materialButtonComConnect.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonComConnect.UseAccentColor = false;
            this._materialButtonComConnect.UseVisualStyleBackColor = true;
            this._materialButtonComConnect.Click += new System.EventHandler(this._materialButtonComConnect_Click);
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(33, 85);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(69, 19);
            this.materialLabel2.TabIndex = 2;
            this.materialLabel2.Text = "COM Port";
            // 
            // _materialComboBoxComPorts
            // 
            this._materialComboBoxComPorts.AutoResize = false;
            this._materialComboBoxComPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._materialComboBoxComPorts.Depth = 0;
            this._materialComboBoxComPorts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._materialComboBoxComPorts.DropDownHeight = 174;
            this._materialComboBoxComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._materialComboBoxComPorts.DropDownWidth = 121;
            this._materialComboBoxComPorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this._materialComboBoxComPorts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._materialComboBoxComPorts.FormattingEnabled = true;
            this._materialComboBoxComPorts.IntegralHeight = false;
            this._materialComboBoxComPorts.ItemHeight = 43;
            this._materialComboBoxComPorts.Location = new System.Drawing.Point(126, 68);
            this._materialComboBoxComPorts.MaxDropDownItems = 4;
            this._materialComboBoxComPorts.MouseState = MaterialSkin.MouseState.OUT;
            this._materialComboBoxComPorts.Name = "_materialComboBoxComPorts";
            this._materialComboBoxComPorts.Size = new System.Drawing.Size(130, 49);
            this._materialComboBoxComPorts.TabIndex = 1;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(17, 14);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(98, 19);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "COM Settings";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "avatar.png");
            this.imageList1.Images.SetKeyName(1, "edit.png");
            this.imageList1.Images.SetKeyName(2, "settings.png");
            // 
            // materialContextMenuStrip1
            // 
            this.materialContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.materialContextMenuStrip1.Depth = 0;
            this.materialContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.materialContextMenuStrip1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialContextMenuStrip1.Name = "materialContextMenuStrip1";
            this.materialContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // _materialButtonSaveScript
            // 
            this._materialButtonSaveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonSaveScript.AutoSize = false;
            this._materialButtonSaveScript.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonSaveScript.Depth = 0;
            this._materialButtonSaveScript.DrawShadows = true;
            this._materialButtonSaveScript.HighEmphasis = true;
            this._materialButtonSaveScript.Icon = null;
            this._materialButtonSaveScript.Location = new System.Drawing.Point(682, 39);
            this._materialButtonSaveScript.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonSaveScript.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonSaveScript.Name = "_materialButtonSaveScript";
            this._materialButtonSaveScript.Size = new System.Drawing.Size(178, 36);
            this._materialButtonSaveScript.TabIndex = 4;
            this._materialButtonSaveScript.Text = "Save";
            this._materialButtonSaveScript.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonSaveScript.UseAccentColor = false;
            this._materialButtonSaveScript.UseVisualStyleBackColor = true;
            // 
            // _materialButtonSaveScriptAs
            // 
            this._materialButtonSaveScriptAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonSaveScriptAs.AutoSize = false;
            this._materialButtonSaveScriptAs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonSaveScriptAs.Depth = 0;
            this._materialButtonSaveScriptAs.DrawShadows = true;
            this._materialButtonSaveScriptAs.HighEmphasis = true;
            this._materialButtonSaveScriptAs.Icon = null;
            this._materialButtonSaveScriptAs.Location = new System.Drawing.Point(682, 87);
            this._materialButtonSaveScriptAs.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonSaveScriptAs.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonSaveScriptAs.Name = "_materialButtonSaveScriptAs";
            this._materialButtonSaveScriptAs.Size = new System.Drawing.Size(178, 36);
            this._materialButtonSaveScriptAs.TabIndex = 5;
            this._materialButtonSaveScriptAs.Text = "Save As";
            this._materialButtonSaveScriptAs.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonSaveScriptAs.UseAccentColor = false;
            this._materialButtonSaveScriptAs.UseVisualStyleBackColor = true;
            // 
            // _materialButtonLoadScript
            // 
            this._materialButtonLoadScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonLoadScript.AutoSize = false;
            this._materialButtonLoadScript.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonLoadScript.Depth = 0;
            this._materialButtonLoadScript.DrawShadows = true;
            this._materialButtonLoadScript.HighEmphasis = true;
            this._materialButtonLoadScript.Icon = null;
            this._materialButtonLoadScript.Location = new System.Drawing.Point(682, 135);
            this._materialButtonLoadScript.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonLoadScript.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonLoadScript.Name = "_materialButtonLoadScript";
            this._materialButtonLoadScript.Size = new System.Drawing.Size(178, 36);
            this._materialButtonLoadScript.TabIndex = 6;
            this._materialButtonLoadScript.Text = "Load";
            this._materialButtonLoadScript.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonLoadScript.UseAccentColor = false;
            this._materialButtonLoadScript.UseVisualStyleBackColor = true;
            // 
            // _materialButtonGetResults
            // 
            this._materialButtonGetResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialButtonGetResults.AutoSize = false;
            this._materialButtonGetResults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._materialButtonGetResults.Depth = 0;
            this._materialButtonGetResults.DrawShadows = true;
            this._materialButtonGetResults.HighEmphasis = true;
            this._materialButtonGetResults.Icon = null;
            this._materialButtonGetResults.Location = new System.Drawing.Point(682, 475);
            this._materialButtonGetResults.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._materialButtonGetResults.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialButtonGetResults.Name = "_materialButtonGetResults";
            this._materialButtonGetResults.Size = new System.Drawing.Size(178, 36);
            this._materialButtonGetResults.TabIndex = 7;
            this._materialButtonGetResults.Text = "Get results";
            this._materialButtonGetResults.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._materialButtonGetResults.UseAccentColor = false;
            this._materialButtonGetResults.UseVisualStyleBackColor = true;
            // 
            // _materialSwitchResetResultsScriptStart
            // 
            this._materialSwitchResetResultsScriptStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._materialSwitchResetResultsScriptStart.AutoSize = true;
            this._materialSwitchResetResultsScriptStart.Checked = true;
            this._materialSwitchResetResultsScriptStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this._materialSwitchResetResultsScriptStart.Depth = 0;
            this._materialSwitchResetResultsScriptStart.Location = new System.Drawing.Point(678, 286);
            this._materialSwitchResetResultsScriptStart.Margin = new System.Windows.Forms.Padding(0);
            this._materialSwitchResetResultsScriptStart.MouseLocation = new System.Drawing.Point(-1, -1);
            this._materialSwitchResetResultsScriptStart.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialSwitchResetResultsScriptStart.Name = "_materialSwitchResetResultsScriptStart";
            this._materialSwitchResetResultsScriptStart.Ripple = true;
            this._materialSwitchResetResultsScriptStart.Size = new System.Drawing.Size(202, 37);
            this._materialSwitchResetResultsScriptStart.TabIndex = 8;
            this._materialSwitchResetResultsScriptStart.Text = "Reset results at start";
            this._materialSwitchResetResultsScriptStart.UseVisualStyleBackColor = true;
            // 
            // _materialLabelScriptName
            // 
            this._materialLabelScriptName.AutoSize = true;
            this._materialLabelScriptName.Depth = 0;
            this._materialLabelScriptName.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._materialLabelScriptName.Location = new System.Drawing.Point(92, 3);
            this._materialLabelScriptName.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialLabelScriptName.Name = "_materialLabelScriptName";
            this._materialLabelScriptName.Size = new System.Drawing.Size(105, 19);
            this._materialLabelScriptName.TabIndex = 9;
            this._materialLabelScriptName.Text = "Unsaved script";
            // 
            // _materialLabelEditingFile
            // 
            this._materialLabelEditingFile.AutoSize = true;
            this._materialLabelEditingFile.Depth = 0;
            this._materialLabelEditingFile.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._materialLabelEditingFile.Location = new System.Drawing.Point(6, 3);
            this._materialLabelEditingFile.MouseState = MaterialSkin.MouseState.HOVER;
            this._materialLabelEditingFile.Name = "_materialLabelEditingFile";
            this._materialLabelEditingFile.Size = new System.Drawing.Size(80, 19);
            this._materialLabelEditingFile.TabIndex = 10;
            this._materialLabelEditingFile.Text = "Editing file:";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1028, 920);
            this.Controls.Add(this.materialTabControl1);
            this.Controls.Add(this._textBoxLog);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.materialTabControl1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaRU Jaster Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox _textBoxLog;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialSwitch _materialSwitchAutoResults;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ImageList imageList1;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialContextMenuStrip materialContextMenuStrip1;
        private MaterialSkin.Controls.MaterialComboBox _materialComboBoxComPorts;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialButton _materialButtonComConnect;
        private MaterialSkin.Controls.MaterialButton _materialButtonComRefresh;
        private MaterialSkin.Controls.MaterialButton _materialButtonStopScript;
        private MaterialSkin.Controls.MaterialButton _materialButtonStartPauseScript;
        private MaterialSkin.Controls.MaterialMultiLineTextBox _materialMultiLineTextBoxScript;
        private MaterialSkin.Controls.MaterialButton _materialButtonLoadScript;
        private MaterialSkin.Controls.MaterialButton _materialButtonSaveScriptAs;
        private MaterialSkin.Controls.MaterialButton _materialButtonSaveScript;
        private MaterialSkin.Controls.MaterialButton _materialButtonGetResults;
        private MaterialSkin.Controls.MaterialSwitch _materialSwitchResetResultsScriptStart;
        private MaterialSkin.Controls.MaterialLabel _materialLabelScriptName;
        private MaterialSkin.Controls.MaterialLabel _materialLabelEditingFile;
    }
}

