namespace TestServiceBusTopics
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using TestServiceBusTopics.TestingScenarios;

    public partial class QueueForm : Form
    {
        Dictionary<string, int> messages;
        public List<dynamic> LogsList0 { get; set; }
        string serviceBusConnectionString = "Endpoint=sb://akshayraut.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ge+2CAE0f+xd+clzKonx05m5xZdsAj/Mo+ASbM6KG6A=";
        string queueName = "TestQueue";
        string timeStampFormat = "HH:mm:ss.fff";
        private readonly SynchronizationContext synchronizationContext; //context from UI thread
        private TestQueue testQueue;

        public QueueForm()
        {
            InitializeComponent();
            LogsList0 = new List<dynamic>();
            synchronizationContext = SynchronizationContext.Current;
            testQueue = new TestQueue(this);
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
                testQueue.SendMessagesBatch(serviceBusConnectionString, queueName, messages);
            });
        }

        public void clearLogs_Click(object sender, EventArgs e)
        {
            LogsList0 = new List<dynamic>();
            BindingSource bs0 = new BindingSource();
            bs0.DataSource = LogsList0;
            logsDataGridView0.DataSource = bs0;
            logsDataGridView0.Refresh();
        }

        public void DisplayLogsForSingle(ServiceBusReceivedMessage receivedMessage)
        {
            LogsList0.Add(new
            {
                No = receivedMessage.SequenceNumber,
                ENo= receivedMessage.EnqueuedSequenceNumber,
                Entry = receivedMessage.EnqueuedTime.LocalDateTime.ToString(timeStampFormat),
                Exit = DateTime.Now.ToString(timeStampFormat)
            });

            BindingSource bs = new BindingSource();
            bs.DataSource = LogsList0;
            //Send the update to our UI thread
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                logsDataGridView0.DataSource = bs;
                logsDataGridView0.Refresh();
            }), null);
        }

        private void receive_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                testQueue.ReceiveMessages(serviceBusConnectionString, queueName);
            });
        }
    }
}
