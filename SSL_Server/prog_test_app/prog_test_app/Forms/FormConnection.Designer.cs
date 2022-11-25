namespace prog_test_app.Forms
{
    partial class FormConnection
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
            this.btnCloseChildForm = new System.Windows.Forms.Button();
            this.btnConnectComPort = new System.Windows.Forms.Button();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.txtBoxServerPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCloseChildForm
            // 
            this.btnCloseChildForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseChildForm.Location = new System.Drawing.Point(391, 224);
            this.btnCloseChildForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseChildForm.Name = "btnCloseChildForm";
            this.btnCloseChildForm.Size = new System.Drawing.Size(236, 75);
            this.btnCloseChildForm.TabIndex = 0;
            this.btnCloseChildForm.Text = "Zamknij";
            this.btnCloseChildForm.UseVisualStyleBackColor = true;
            this.btnCloseChildForm.Click += new System.EventHandler(this.btnCloseChildForm_Click);
            // 
            // btnConnectComPort
            // 
            this.btnConnectComPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectComPort.Location = new System.Drawing.Point(124, 224);
            this.btnConnectComPort.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectComPort.Name = "btnConnectComPort";
            this.btnConnectComPort.Size = new System.Drawing.Size(236, 75);
            this.btnConnectComPort.TabIndex = 2;
            this.btnConnectComPort.Text = "Uruchom serwer";
            this.btnConnectComPort.UseVisualStyleBackColor = true;
            this.btnConnectComPort.Click += new System.EventHandler(this.btnConnectWithSSLServ_Click);
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelServerPort.Location = new System.Drawing.Point(91, 126);
            this.labelServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(38, 17);
            this.labelServerPort.TabIndex = 4;
            this.labelServerPort.Text = "Port";
            // 
            // txtBoxServerPort
            // 
            this.txtBoxServerPort.Location = new System.Drawing.Point(175, 124);
            this.txtBoxServerPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxServerPort.Name = "txtBoxServerPort";
            this.txtBoxServerPort.Size = new System.Drawing.Size(451, 22);
            this.txtBoxServerPort.TabIndex = 13;
            this.txtBoxServerPort.Text = "5555";
            // 
            // FormConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 407);
            this.Controls.Add(this.txtBoxServerPort);
            this.Controls.Add(this.btnCloseChildForm);
            this.Controls.Add(this.btnConnectComPort);
            this.Controls.Add(this.labelServerPort);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormConnection";
            this.Text = "FormConnection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseChildForm;
        private System.Windows.Forms.Button btnConnectComPort;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox txtBoxServerPort;
    }
}