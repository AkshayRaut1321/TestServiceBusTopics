namespace TestServiceBusTopics
{
    partial class TopicForm
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
            this.send = new System.Windows.Forms.Button();
            this.receive = new System.Windows.Forms.Button();
            this.logsDataGridView0 = new System.Windows.Forms.DataGridView();
            this.clearLogs = new System.Windows.Forms.Button();
            this.logsDataGridView1 = new System.Windows.Forms.DataGridView();
            this.logsDataGridView2 = new System.Windows.Forms.DataGridView();
            this.singleRadio = new System.Windows.Forms.RadioButton();
            this.multipleRadio = new System.Windows.Forms.RadioButton();
            this.stopReceive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(12, 12);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(146, 86);
            this.send.TabIndex = 0;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // receive
            // 
            this.receive.Location = new System.Drawing.Point(12, 193);
            this.receive.Name = "receive";
            this.receive.Size = new System.Drawing.Size(146, 86);
            this.receive.TabIndex = 1;
            this.receive.Text = "Receive";
            this.receive.UseVisualStyleBackColor = true;
            this.receive.Click += new System.EventHandler(this.receive_Click);
            // 
            // logsDataGridView0
            // 
            this.logsDataGridView0.AllowUserToAddRows = false;
            this.logsDataGridView0.AllowUserToDeleteRows = false;
            this.logsDataGridView0.AllowUserToOrderColumns = true;
            this.logsDataGridView0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView0.Location = new System.Drawing.Point(177, 12);
            this.logsDataGridView0.Name = "logsDataGridView0";
            this.logsDataGridView0.ReadOnly = true;
            this.logsDataGridView0.RowHeadersWidth = 62;
            this.logsDataGridView0.RowTemplate.Height = 28;
            this.logsDataGridView0.Size = new System.Drawing.Size(632, 622);
            this.logsDataGridView0.TabIndex = 2;
            // 
            // clearLogs
            // 
            this.clearLogs.Location = new System.Drawing.Point(12, 478);
            this.clearLogs.Name = "clearLogs";
            this.clearLogs.Size = new System.Drawing.Size(146, 86);
            this.clearLogs.TabIndex = 5;
            this.clearLogs.Text = "Clear logs";
            this.clearLogs.UseVisualStyleBackColor = true;
            this.clearLogs.Click += new System.EventHandler(this.clearLogs_Click);
            // 
            // logsDataGridView1
            // 
            this.logsDataGridView1.AllowUserToAddRows = false;
            this.logsDataGridView1.AllowUserToDeleteRows = false;
            this.logsDataGridView1.AllowUserToOrderColumns = true;
            this.logsDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView1.Location = new System.Drawing.Point(829, 12);
            this.logsDataGridView1.Name = "logsDataGridView1";
            this.logsDataGridView1.ReadOnly = true;
            this.logsDataGridView1.RowHeadersWidth = 62;
            this.logsDataGridView1.RowTemplate.Height = 28;
            this.logsDataGridView1.Size = new System.Drawing.Size(632, 622);
            this.logsDataGridView1.TabIndex = 6;
            // 
            // logsDataGridView2
            // 
            this.logsDataGridView2.AllowUserToAddRows = false;
            this.logsDataGridView2.AllowUserToDeleteRows = false;
            this.logsDataGridView2.AllowUserToOrderColumns = true;
            this.logsDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView2.Location = new System.Drawing.Point(1486, 12);
            this.logsDataGridView2.Name = "logsDataGridView2";
            this.logsDataGridView2.ReadOnly = true;
            this.logsDataGridView2.RowHeadersWidth = 62;
            this.logsDataGridView2.RowTemplate.Height = 28;
            this.logsDataGridView2.Size = new System.Drawing.Size(632, 622);
            this.logsDataGridView2.TabIndex = 7;
            // 
            // singleRadio
            // 
            this.singleRadio.AutoSize = true;
            this.singleRadio.Location = new System.Drawing.Point(23, 104);
            this.singleRadio.Name = "singleRadio";
            this.singleRadio.Size = new System.Drawing.Size(78, 24);
            this.singleRadio.TabIndex = 8;
            this.singleRadio.TabStop = true;
            this.singleRadio.Text = "Single";
            this.singleRadio.UseVisualStyleBackColor = true;
            // 
            // multipleRadio
            // 
            this.multipleRadio.AutoSize = true;
            this.multipleRadio.Location = new System.Drawing.Point(23, 134);
            this.multipleRadio.Name = "multipleRadio";
            this.multipleRadio.Size = new System.Drawing.Size(88, 24);
            this.multipleRadio.TabIndex = 9;
            this.multipleRadio.TabStop = true;
            this.multipleRadio.Text = "Multiple";
            this.multipleRadio.UseVisualStyleBackColor = true;
            // 
            // stopReceive
            // 
            this.stopReceive.Location = new System.Drawing.Point(12, 327);
            this.stopReceive.Name = "stopReceive";
            this.stopReceive.Size = new System.Drawing.Size(146, 86);
            this.stopReceive.TabIndex = 10;
            this.stopReceive.Text = "Stop receiving";
            this.stopReceive.UseVisualStyleBackColor = true;
            this.stopReceive.Click += new System.EventHandler(this.stopReceive_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1673, 659);
            this.Controls.Add(this.stopReceive);
            this.Controls.Add(this.multipleRadio);
            this.Controls.Add(this.singleRadio);
            this.Controls.Add(this.logsDataGridView2);
            this.Controls.Add(this.logsDataGridView1);
            this.Controls.Add(this.clearLogs);
            this.Controls.Add(this.logsDataGridView0);
            this.Controls.Add(this.receive);
            this.Controls.Add(this.send);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button receive;
        private System.Windows.Forms.DataGridView logsDataGridView0;
        private System.Windows.Forms.Button clearLogs;
        private System.Windows.Forms.DataGridView logsDataGridView1;
        private System.Windows.Forms.DataGridView logsDataGridView2;
        private System.Windows.Forms.RadioButton singleRadio;
        private System.Windows.Forms.RadioButton multipleRadio;
        private System.Windows.Forms.Button stopReceive;
    }
}

