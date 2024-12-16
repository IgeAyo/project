namespace UareUSampleCSharp
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false</param>
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
            txtReaderSelected = new TextBox();
            Label1 = new Label();
            btnEnroll = new Button();
            btnIdentify = new Button();
            btnVerify = new Button();
            btnStreaming = new Button();
            btnCapture = new Button();
            btnReaderSelect = new Button();
            btnEnrollmentControl = new Button();
            btnIdentificationControl = new Button();
            SuspendLayout();
            // 
            // txtReaderSelected
            // 
            txtReaderSelected.Font = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point);
            txtReaderSelected.Location = new Point(15, 27);
            txtReaderSelected.Name = "txtReaderSelected";
            txtReaderSelected.ReadOnly = true;
            txtReaderSelected.Size = new Size(233, 20);
            txtReaderSelected.TabIndex = 7;
            txtReaderSelected.TextChanged += txtReaderSelected_TextChanged;
            // 
            // Label1
            // 
            Label1.Location = new Point(12, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(236, 15);
            Label1.TabIndex = 18;
            Label1.Text = "Selected Reader:";
            Label1.Click += Label1_Click;
            // 
            // btnEnroll
            // 
            btnEnroll.Enabled = false;
            btnEnroll.Location = new Point(12, 111);
            btnEnroll.Name = "btnEnroll";
            btnEnroll.Size = new Size(115, 23);
            btnEnroll.TabIndex = 13;
            btnEnroll.Text = "Enrollment";
            btnEnroll.Click += btnEnroll_Click;
            // 
            // btnIdentify
            // 
            btnIdentify.Enabled = false;
            btnIdentify.Location = new Point(133, 82);
            btnIdentify.Name = "btnIdentify";
            btnIdentify.Size = new Size(115, 23);
            btnIdentify.TabIndex = 12;
            btnIdentify.Text = "Identification";
            btnIdentify.Click += btnIdentify_Click;
            // 
            // btnVerify
            // 
            btnVerify.Enabled = false;
            btnVerify.Location = new Point(12, 82);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(115, 23);
            btnVerify.TabIndex = 11;
            btnVerify.Text = "Verification";
            btnVerify.Click += btnVerify_Click;
            // 
            // btnStreaming
            // 
            btnStreaming.Enabled = false;
            btnStreaming.Location = new Point(133, 111);
            btnStreaming.Name = "btnStreaming";
            btnStreaming.Size = new Size(115, 23);
            btnStreaming.TabIndex = 15;
            btnStreaming.Text = "Streaming";
            btnStreaming.Click += btnStreaming_Click;
            // 
            // btnCapture
            // 
            btnCapture.Enabled = false;
            btnCapture.Location = new Point(133, 53);
            btnCapture.Name = "btnCapture";
            btnCapture.Size = new Size(115, 23);
            btnCapture.TabIndex = 9;
            btnCapture.Text = "Capture";
            btnCapture.Click += btnCapture_Click;
            // 
            // btnReaderSelect
            // 
            btnReaderSelect.Location = new Point(12, 53);
            btnReaderSelect.Name = "btnReaderSelect";
            btnReaderSelect.Size = new Size(115, 23);
            btnReaderSelect.TabIndex = 8;
            btnReaderSelect.Text = "Reader Selection";
            btnReaderSelect.Click += btnReaderSelect_Click;
            // 
            // btnEnrollmentControl
            // 
            btnEnrollmentControl.Enabled = false;
            btnEnrollmentControl.Location = new Point(12, 150);
            btnEnrollmentControl.Name = "btnEnrollmentControl";
            btnEnrollmentControl.Size = new Size(115, 23);
            btnEnrollmentControl.TabIndex = 16;
            btnEnrollmentControl.Text = "Enrollment GUI";
            btnEnrollmentControl.Click += btnEnrollmentControl_Click;
            // 
            // btnIdentificationControl
            // 
            btnIdentificationControl.Enabled = false;
            btnIdentificationControl.Location = new Point(133, 150);
            btnIdentificationControl.Name = "btnIdentificationControl";
            btnIdentificationControl.Size = new Size(115, 23);
            btnIdentificationControl.TabIndex = 17;
            btnIdentificationControl.Text = "Identification GUI";
            btnIdentificationControl.Click += btnIdentificationControl_Click;
            // 
            // Form_Main
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(261, 188);
            Controls.Add(btnIdentificationControl);
            Controls.Add(btnEnrollmentControl);
            Controls.Add(txtReaderSelected);
            Controls.Add(Label1);
            Controls.Add(btnEnroll);
            Controls.Add(btnIdentify);
            Controls.Add(btnVerify);
            Controls.Add(btnStreaming);
            Controls.Add(btnCapture);
            Controls.Add(btnReaderSelect);
            MaximizeBox = false;
            MaximumSize = new Size(277, 227);
            MinimizeBox = false;
            MinimumSize = new Size(277, 227);
            Name = "Form_Main";
            StartPosition = FormStartPosition.CenterParent;
            Text = "U.are.U Sample C#";
            Load += Form_Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        internal TextBox txtReaderSelected;
        internal Label Label1;
        internal Button btnEnroll;
        internal Button btnIdentify;
        internal Button btnVerify;
        internal Button btnStreaming;
        internal Button btnCapture;
        internal Button btnReaderSelect;
        internal Button btnEnrollmentControl;
        public Button btnIdentificationControl;
    }
}