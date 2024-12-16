namespace project
{
    partial class FormDbVerification
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
            comboBox1 = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            pbFingerprint = new PictureBox();
            listBox1 = new ListBox();
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).BeginInit();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox1.Location = new Point(11, 251);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(202, 21);
            comboBox1.TabIndex = 20;
            // 
            // label3
            // 
            label3.Location = new Point(12, 232);
            label3.Name = "label3";
            label3.Size = new Size(265, 16);
            label3.TabIndex = 19;
            label3.Text = "Select Reader:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 189);
            label2.Name = "label2";
            label2.Size = new Size(127, 15);
            label2.TabIndex = 17;
            label2.Text = "Place Finger on Reader";
            // 
            // pbFingerprint
            // 
            pbFingerprint.Location = new Point(12, 12);
            pbFingerprint.Name = "pbFingerprint";
            pbFingerprint.Size = new Size(192, 174);
            pbFingerprint.TabIndex = 16;
            pbFingerprint.TabStop = false;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(223, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(164, 169);
            listBox1.TabIndex = 21;
            // 
            // FormDbVerification
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(399, 323);
            Controls.Add(listBox1);
            Controls.Add(comboBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pbFingerprint);
            Name = "FormDbVerification";
            Text = "FormDbVerification";
            Load += FormDbVerification_Load;
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal ComboBox comboBox1;
        internal Label label3;
        private Label label2;
        private PictureBox pbFingerprint;
        private ListBox listBox1;
    }
}