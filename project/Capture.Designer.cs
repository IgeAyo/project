namespace UareUSampleCSharp
{
    partial class Capture
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
            lblPlaceFinger = new Label();
            btnBack = new Button();
            pbFingerprint = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).BeginInit();
            SuspendLayout();
            // 
            // lblPlaceFinger
            // 
            lblPlaceFinger.Location = new Point(2, 380);
            lblPlaceFinger.Name = "lblPlaceFinger";
            lblPlaceFinger.Size = new Size(187, 19);
            lblPlaceFinger.TabIndex = 5;
            lblPlaceFinger.Text = "Place a finger on the reader";
            // 
            // btnBack
            // 
            btnBack.Location = new Point(197, 375);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(56, 23);
            btnBack.TabIndex = 4;
            btnBack.Text = "Back";
            btnBack.Click += btnBack_Click;
            // 
            // pbFingerprint
            // 
            pbFingerprint.Location = new Point(3, 1);
            pbFingerprint.Name = "pbFingerprint";
            pbFingerprint.Size = new Size(256, 360);
            pbFingerprint.SizeMode = PictureBoxSizeMode.Zoom;
            pbFingerprint.TabIndex = 6;
            pbFingerprint.TabStop = false;
            pbFingerprint.Click += pbFingerprint_Click;
            // 
            // Capture
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(259, 401);
            Controls.Add(btnBack);
            Controls.Add(lblPlaceFinger);
            Controls.Add(pbFingerprint);
            MaximizeBox = false;
            MaximumSize = new Size(275, 440);
            MinimizeBox = false;
            MinimumSize = new Size(275, 440);
            Name = "Capture";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Capture";
            Closed += Capture_Closed;
            Load += Capture_Load;
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).EndInit();
            ResumeLayout(false);
        }

        #endregion

        internal Label lblPlaceFinger;
        internal Button btnBack;
        internal PictureBox pbFingerprint;
    }
}