namespace TestServiceBusTopics
{
    partial class MainForm
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
            this.testTopics = new System.Windows.Forms.Button();
            this.testQueues = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testTopics
            // 
            this.testTopics.Location = new System.Drawing.Point(63, 55);
            this.testTopics.Name = "testTopics";
            this.testTopics.Size = new System.Drawing.Size(125, 80);
            this.testTopics.TabIndex = 0;
            this.testTopics.Text = "Test Topics";
            this.testTopics.UseVisualStyleBackColor = true;
            this.testTopics.Click += new System.EventHandler(this.testTopics_Click);
            // 
            // testQueues
            // 
            this.testQueues.Location = new System.Drawing.Point(259, 55);
            this.testQueues.Name = "testQueues";
            this.testQueues.Size = new System.Drawing.Size(125, 80);
            this.testQueues.TabIndex = 1;
            this.testQueues.Text = "Test Queues";
            this.testQueues.UseVisualStyleBackColor = true;
            this.testQueues.Click += new System.EventHandler(this.testQueues_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.testQueues);
            this.Controls.Add(this.testTopics);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button testTopics;
        private System.Windows.Forms.Button testQueues;
    }
}