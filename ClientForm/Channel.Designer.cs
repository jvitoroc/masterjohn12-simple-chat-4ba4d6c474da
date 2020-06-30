namespace ClientForm
{
    partial class Channel
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("sd");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("asdasdasdasdasdasdasdas");
            this.lvUsers = new System.Windows.Forms.ListView();
            this.txtMessageText = new System.Windows.Forms.TextBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.txtMessageLog = new System.Windows.Forms.RichTextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtChannelID = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlChannel = new System.Windows.Forms.Panel();
            this.pnlRight.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvUsers
            // 
            this.lvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvUsers.Location = new System.Drawing.Point(0, 0);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(234, 422);
            this.lvUsers.TabIndex = 0;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Tile;
            // 
            // txtMessageText
            // 
            this.txtMessageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageText.Location = new System.Drawing.Point(0, 0);
            this.txtMessageText.Multiline = true;
            this.txtMessageText.Name = "txtMessageText";
            this.txtMessageText.Size = new System.Drawing.Size(566, 62);
            this.txtMessageText.TabIndex = 1;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.txtMessageLog);
            this.pnlRight.Controls.Add(this.pnlBottom);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(234, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(566, 422);
            this.pnlRight.TabIndex = 3;
            // 
            // txtMessageLog
            // 
            this.txtMessageLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageLog.Location = new System.Drawing.Point(0, 0);
            this.txtMessageLog.Name = "txtMessageLog";
            this.txtMessageLog.ReadOnly = true;
            this.txtMessageLog.Size = new System.Drawing.Size(566, 360);
            this.txtMessageLog.TabIndex = 5;
            this.txtMessageLog.Text = "";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSend);
            this.pnlBottom.Controls.Add(this.txtMessageText);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 360);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(566, 62);
            this.pnlBottom.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Location = new System.Drawing.Point(464, 0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(102, 62);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Enviar";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtChannelID
            // 
            this.txtChannelID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChannelID.Location = new System.Drawing.Point(0, 0);
            this.txtChannelID.Name = "txtChannelID";
            this.txtChannelID.Size = new System.Drawing.Size(800, 28);
            this.txtChannelID.TabIndex = 6;
            this.txtChannelID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtChannelID);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(800, 28);
            this.pnlTop.TabIndex = 6;
            // 
            // pnlChannel
            // 
            this.pnlChannel.Controls.Add(this.lvUsers);
            this.pnlChannel.Controls.Add(this.pnlRight);
            this.pnlChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChannel.Location = new System.Drawing.Point(0, 28);
            this.pnlChannel.Name = "pnlChannel";
            this.pnlChannel.Size = new System.Drawing.Size(800, 422);
            this.pnlChannel.TabIndex = 7;
            // 
            // Channel
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlChannel);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Channel";
            this.Text = "Channel";
            this.Shown += new System.EventHandler(this.Channel_Shown);
            this.pnlRight.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlChannel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.TextBox txtMessageText;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtMessageLog;
        private System.Windows.Forms.Label txtChannelID;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlChannel;
    }
}