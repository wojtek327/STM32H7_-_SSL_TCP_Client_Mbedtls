namespace prog_test_app.Forms
{
    partial class SendMsg
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
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnCloseChildForm = new System.Windows.Forms.Button();
            this.txtBoxMsgToSend = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.Location = new System.Drawing.Point(314, 101);
            this.lblSerialNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(85, 17);
            this.lblSerialNumber.TabIndex = 13;
            this.lblSerialNumber.Text = "Msg to send";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMsg.Location = new System.Drawing.Point(137, 235);
            this.btnSendMsg.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(211, 85);
            this.btnSendMsg.TabIndex = 11;
            this.btnSendMsg.Text = "Wyslij";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnCloseChildForm
            // 
            this.btnCloseChildForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseChildForm.Location = new System.Drawing.Point(356, 235);
            this.btnCloseChildForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseChildForm.Name = "btnCloseChildForm";
            this.btnCloseChildForm.Size = new System.Drawing.Size(217, 85);
            this.btnCloseChildForm.TabIndex = 9;
            this.btnCloseChildForm.Text = "Zamknij";
            this.btnCloseChildForm.UseVisualStyleBackColor = true;
            this.btnCloseChildForm.Click += new System.EventHandler(this.btnCloseChildForm_Click);
            // 
            // txtBoxMsgToSend
            // 
            this.txtBoxMsgToSend.Location = new System.Drawing.Point(138, 131);
            this.txtBoxMsgToSend.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxMsgToSend.Name = "txtBoxMsgToSend";
            this.txtBoxMsgToSend.Size = new System.Drawing.Size(435, 22);
            this.txtBoxMsgToSend.TabIndex = 14;
            // 
            // SendMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 407);
            this.Controls.Add(this.txtBoxMsgToSend);
            this.Controls.Add(this.lblSerialNumber);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.btnCloseChildForm);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SendMsg";
            this.Text = "FormProgramCard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Button btnCloseChildForm;
        private System.Windows.Forms.TextBox txtBoxMsgToSend;
    }
}