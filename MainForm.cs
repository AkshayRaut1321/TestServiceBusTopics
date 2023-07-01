namespace TestServiceBusTopics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void testTopics_Click(object sender, EventArgs e)
        {
            TopicForm topicForm = new TopicForm();
            topicForm.ShowDialog(this);
        }

        private void testQueues_Click(object sender, EventArgs e)
        {
            QueueForm queueForm = new QueueForm();
            queueForm.ShowDialog(this);
        }
    }
}
