namespace ClientForm
{
    partial class ConnectServer
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
            this.txtPortaTCP = new System.Windows.Forms.TextBox();
            this.txtPortaUDP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPortaTCP = new System.Windows.Forms.Label();
            this.lblPortaUDP = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtPortaTCP
            // 
            this.txtPortaTCP.Location = new System.Drawing.Point(118, 48);
            this.txtPortaTCP.Name = "txtPortaTCP";
            this.txtPortaTCP.Size = new System.Drawing.Size(80, 20);
            this.txtPortaTCP.TabIndex = 2;
            this.txtPortaTCP.Text = "13000";
            this.txtPortaTCP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPorta_KeyPress);
            // 
            // txtPortaUDP
            // 
            this.txtPortaUDP.Location = new System.Drawing.Point(204, 48);
            this.txtPortaUDP.Name = "txtPortaUDP";
            this.txtPortaUDP.Size = new System.Drawing.Size(80, 20);
            this.txtPortaUDP.TabIndex = 3;
            this.txtPortaUDP.Text = "13001";
            this.txtPortaUDP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPorta_KeyPress);
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(9, 32);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(17, 13);
            this.lblIP.TabIndex = 3;
            this.lblIP.Text = "IP";
            // 
            // lblPortaTCP
            // 
            this.lblPortaTCP.AutoSize = true;
            this.lblPortaTCP.Location = new System.Drawing.Point(115, 32);
            this.lblPortaTCP.Name = "lblPortaTCP";
            this.lblPortaTCP.Size = new System.Drawing.Size(56, 13);
            this.lblPortaTCP.TabIndex = 4;
            this.lblPortaTCP.Text = "Porta TCP";
            // 
            // lblPortaUDP
            // 
            this.lblPortaUDP.AutoSize = true;
            this.lblPortaUDP.Location = new System.Drawing.Point(201, 32);
            this.lblPortaUDP.Name = "lblPortaUDP";
            this.lblPortaUDP.Size = new System.Drawing.Size(58, 13);
            this.lblPortaUDP.TabIndex = 5;
            this.lblPortaUDP.Text = "Porta UDP";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(209, 104);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Conectar";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(12, 48);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            // 
            // ConnectServer
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 138);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblPortaUDP);
            this.Controls.Add(this.lblPortaTCP);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtPortaUDP);
            this.Controls.Add(this.txtPortaTCP);
            this.Name = "ConnectServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conectar a um servidor";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtPortaTCP;
        private System.Windows.Forms.TextBox txtPortaUDP;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPortaTCP;
        private System.Windows.Forms.Label lblPortaUDP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtIP;
    }
}