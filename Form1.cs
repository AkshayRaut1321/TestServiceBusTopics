namespace TestServiceBusTopics
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using TestServiceBusTopics.TestingScenarios;

    public partial class Form1 : Form
    {
        string serviceBusConnectionString = "Endpoint=sb://akshayraut.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ge+2CAE0f+xd+clzKonx05m5xZdsAj/Mo+ASbM6KG6A=";
        string topicName = "testsessiontopic";

        string timeStampFormat = "HH:mm:ss.fff";
        SingleSubscription singleSubscription;

        Dictionary<string, int> messages;
        ConcurrentDictionary<string, ServiceBusSessionProcessor> serviceBusSessionProcessors = new ConcurrentDictionary<string, ServiceBusSessionProcessor>();

        public Form1()
        {
            InitializeComponent();
            singleSubscription = new SingleSubscription(this);
        }

        private void send_Click(object sender, EventArgs e)
        {
            messages = new Dictionary<string, int>
            {
                { "Message 0", 0 },
                { "Message 1", 1 },
                { "Message 2", 2 },
                { "Message 3", 0 },
                { "Message 4", 1 },
                { "Message 5", 2 },
                { "Message 6", 0 },
                { "Message 7", 1 },
                { "Message 8", 2 },
                { "Message 9", 0 },
                { "Message 10", 1 },
                { "Message 11", 2 }
            };

            Task.Run(() =>
            {
                singleSubscription.SendMessagesSingles(serviceBusConnectionString, topicName, messages);
            });
        }

        private void receive_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                singleSubscription.ReceiveMessages(serviceBusConnectionString, topicName);
            });
        }

        public void DisplayLogs(List<dynamic> LogsList, ProcessSessionMessageEventArgs arg)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = LogsList;

            LogsList.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
                SID = arg.SessionId,
                Entry = arg.Message.EnqueuedTime.LocalDateTime.ToString(timeStampFormat),
                Exit = DateTime.Now.ToString(timeStampFormat)
            });
            logsDataGridView0.DataSource = bs;
            logsDataGridView0.Refresh();
        }

        public void clearLogs_Click(object sender, EventArgs e)
        {
            singleSubscription.LogsList0 = new List<dynamic>();
            BindingSource bs0 = new BindingSource();
            bs0.DataSource = singleSubscription.LogsList0;
            logsDataGridView0.DataSource = bs0;
            logsDataGridView0.Refresh();
        }
    }
}
