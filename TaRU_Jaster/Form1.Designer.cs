
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
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this._labelPort = new System.Windows.Forms.Label();
            this._comboBoxPort = new System.Windows.Forms.ComboBox();
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this._textBoxLog = new System.Windows.Forms.TextBox();
            this._buttonOpen = new System.Windows.Forms.Button();
            this._textBoxCommands = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _labelPort
            // 
            this._labelPort.AutoSize = true;
            this._labelPort.Location = new System.Drawing.Point(52, 46);
            this._labelPort.Name = "_labelPort";
            this._labelPort.Size = new System.Drawing.Size(42, 20);
            this._labelPort.TabIndex = 0;
            this._labelPort.Text = "Port:";
            // 
            // _comboBoxPort
            // 
            this._comboBoxPort.FormattingEnabled = true;
            this._comboBoxPort.Location = new System.Drawing.Point(109, 43);
            this._comboBoxPort.Name = "_comboBoxPort";
            this._comboBoxPort.Size = new System.Drawing.Size(121, 28);
            this._comboBoxPort.TabIndex = 1;
            // 
            // _buttonStart
            // 
            this._buttonStart.Location = new System.Drawing.Point(82, 99);
            this._buttonStart.Name = "_buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(108, 42);
            this._buttonStart.TabIndex = 2;
            this._buttonStart.Text = "Start";
            this._buttonStart.UseVisualStyleBackColor = true;
            this._buttonStart.Click += new System.EventHandler(this._buttonStart_Click);
            // 
            // _buttonStop
            // 
            this._buttonStop.Location = new System.Drawing.Point(82, 164);
            this._buttonStop.Name = "_buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(108, 42);
            this._buttonStop.TabIndex = 3;
            this._buttonStop.Text = "Stop";
            this._buttonStop.UseVisualStyleBackColor = true;
            this._buttonStop.Click += new System.EventHandler(this._buttonStop_Click);
            // 
            // _textBoxLog
            // 
            this._textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxLog.Location = new System.Drawing.Point(12, 425);
            this._textBoxLog.Multiline = true;
            this._textBoxLog.Name = "_textBoxLog";
            this._textBoxLog.ReadOnly = true;
            this._textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxLog.Size = new System.Drawing.Size(776, 357);
            this._textBoxLog.TabIndex = 4;
            // 
            // _buttonOpen
            // 
            this._buttonOpen.Location = new System.Drawing.Point(245, 34);
            this._buttonOpen.Name = "_buttonOpen";
            this._buttonOpen.Size = new System.Drawing.Size(75, 45);
            this._buttonOpen.TabIndex = 5;
            this._buttonOpen.Text = "Open";
            this._buttonOpen.UseVisualStyleBackColor = true;
            this._buttonOpen.Click += new System.EventHandler(this._buttonOpen_Click);
            // 
            // _textBoxCommands
            // 
            this._textBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxCommands.Location = new System.Drawing.Point(358, 46);
            this._textBoxCommands.Multiline = true;
            this._textBoxCommands.Name = "_textBoxCommands";
            this._textBoxCommands.Size = new System.Drawing.Size(430, 359);
            this._textBoxCommands.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 794);
            this.Controls.Add(this._textBoxCommands);
            this.Controls.Add(this._buttonOpen);
            this.Controls.Add(this._textBoxLog);
            this.Controls.Add(this._buttonStop);
            this.Controls.Add(this._buttonStart);
            this.Controls.Add(this._comboBoxPort);
            this.Controls.Add(this._labelPort);
            this.Name = "Form1";
            this.Text = "TaRU Jaster Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label _labelPort;
        private System.Windows.Forms.ComboBox _comboBoxPort;
        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.TextBox _textBoxLog;
        private System.Windows.Forms.Button _buttonOpen;
        private System.Windows.Forms.TextBox _textBoxCommands;
    }
}

