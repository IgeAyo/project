namespace project
{
    partial class FormDbEnrollment
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
            txtLedgerId = new TextBox();
            label1 = new Label();
            pbFingerprint = new PictureBox();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            comboBox1 = new ComboBox();
            button2 = new Button();
            listBox1 = new ListBox();
            Conn_btn = new Button();
            Disconn_btn = new Button();
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).BeginInit();
            SuspendLayout();
            // 
            // txtLedgerId
            // 
            txtLedgerId.Location = new Point(66, 9);
            txtLedgerId.Name = "txtLedgerId";
            txtLedgerId.Size = new Size(220, 23);
            txtLedgerId.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1, 12);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 1;
            label1.Text = "Ledger id:";
            // 
            // pbFingerprint
            // 
            pbFingerprint.Location = new Point(12, 41);
            pbFingerprint.Name = "pbFingerprint";
            pbFingerprint.Size = new Size(233, 207);
            pbFingerprint.TabIndex = 2;
            pbFingerprint.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 251);
            label2.Name = "label2";
            label2.Size = new Size(127, 15);
            label2.TabIndex = 3;
            label2.Text = "Place Finger on Reader";
            // 
            // button1
            // 
            button1.Location = new Point(12, 269);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.Location = new Point(12, 295);
            label3.Name = "label3";
            label3.Size = new Size(296, 15);
            label3.TabIndex = 14;
            label3.Text = "Select Reader:";
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox1.Location = new Point(12, 313);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(233, 21);
            comboBox1.TabIndex = 15;
            // 
            // button2
            // 
            button2.Location = new Point(115, 269);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 16;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(265, 49);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(187, 199);
            listBox1.TabIndex = 17;
            // 
            // Conn_btn
            // 
            Conn_btn.Location = new Point(265, 269);
            Conn_btn.Name = "Conn_btn";
            Conn_btn.Size = new Size(75, 23);
            Conn_btn.TabIndex = 18;
            Conn_btn.Text = "Connect";
            Conn_btn.UseVisualStyleBackColor = true;
            Conn_btn.Click += Conn_btn_Click;
            // 
            // Disconn_btn
            // 
            Disconn_btn.Location = new Point(373, 269);
            Disconn_btn.Name = "Disconn_btn";
            Disconn_btn.Size = new Size(75, 23);
            Disconn_btn.TabIndex = 19;
            Disconn_btn.Text = "Disconnect";
            Disconn_btn.UseVisualStyleBackColor = true;
            Disconn_btn.Click += Disconn_btn_Click;
            // 
            // FormDbEnrollment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 346);
            Controls.Add(Disconn_btn);
            Controls.Add(Conn_btn);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(pbFingerprint);
            Controls.Add(label1);
            Controls.Add(txtLedgerId);
            Name = "FormDbEnrollment";
            Text = "FormDbEnrollment";
            Load += FormDbEnrollment_Load;
            ((System.ComponentModel.ISupportInitialize)pbFingerprint).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLedgerId;
        private Label label1;
        private PictureBox pbFingerprint;
        private Label label2;
        private Button button1;
        internal Label label3;
        internal ComboBox comboBox1;
        private Button button2;
        private ListBox listBox1;
        private Button Conn_btn;
        private Button Disconn_btn;
    }
}