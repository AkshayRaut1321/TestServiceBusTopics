namespace TestServiceBusTopics
{
    partial class QueueForm
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
            this.clearLogs = new System.Windows.Forms.Button();
            this.logsDataGridView0 = new System.Windows.Forms.DataGridView();
            this.receive = new System.Windows.Forms.Button();
            this.send = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView0)).BeginInit();
            this.SuspendLayout();
            // 
            // clearLogs
            // 
            this.clearLogs.Location = new System.Drawing.Point(30, 497);
            this.clearLogs.Name = "clearLogs";
            this.clearLogs.Size = new System.Drawing.Size(146, 86);
            this.clearLogs.TabIndex = 14;
            this.clearLogs.Text = "Clear logs";
            this.clearLogs.UseVisualStyleBackColor = true;
            this.clearLogs.Click += new System.EventHandler(this.clearLogs_Click);
            // 
            // logsDataGridView0
            // 
            this.logsDataGridView0.AllowUserToAddRows = false;
            this.logsDataGridView0.AllowUserToDeleteRows = false;
            this.logsDataGridView0.AllowUserToOrderColumns = true;
            this.logsDataGridView0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView0.Location = new System.Drawing.Point(195, 31);
            this.logsDataGridView0.Name = "logsDataGridView0";
            this.logsDataGridView0.ReadOnly = true;
            this.logsDataGridView0.RowHeadersWidth = 62;
            this.logsDataGridView0.RowTemplate.Height = 28;
            this.logsDataGridView0.Size = new System.Drawing.Size(632, 622);
            this.logsDataGridView0.TabIndex = 13;
            // 
            // receive
            // 
            this.receive.Location = new System.Drawing.Point(30, 212);
            this.receive.Name = "receive";
            this.receive.Size = new System.Drawing.Size(146, 86);
            this.receive.TabIndex = 12;
            this.receive.Text = "Receive";
            this.receive.UseVisualStyleBackColor = true;
            this.receive.Click += new System.EventHandler(this.receive_Click);
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(30, 31);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(146, 86);
            this.send.TabIndex = 11;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // QueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 749);
            this.Controls.Add(this.clearLogs);
            this.Controls.Add(this.logsDataGridView0);
            this.Controls.Add(this.receive);
            this.Controls.Add(this.send);
            this.Name = "QueueForm";
            this.Text = "TestQueue";
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView0)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button clearLogs;
        private System.Windows.Forms.DataGridView logsDataGridView0;
        private System.Windows.Forms.Button receive;
        private System.Windows.Forms.Button send;
    }
}