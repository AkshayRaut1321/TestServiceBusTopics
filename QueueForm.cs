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
        string serviceBusConnectionString = "Endpoint=sb://akshayraut.servicebus.windows.net/;SharedAccessKeyName=SendListen;SharedAccessKey=dam8dnfY4Kexf2eYa/L1Pst96SIqbgqtH+ASbIxWAcs=;EntityPath=testqueue";
        string queueName = "TestQueue";
        string timeStampFormat = "HH:mm:ss.fff";
        private readonly SynchronizationContext synchronizationContext; //context from UI thread
        private TestQueue testQueue;
        private TestQueueProcessor testQueueProcessor;

        public QueueForm()
        {
            InitializeComponent();
            LogsList0 = new List<dynamic>();
            synchronizationContext = SynchronizationContext.Current;
            testQueue = new TestQueue(this);
            testQueueProcessor = new TestQueueProcessor(this);
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
                if (asyncStreamRadio.Checked)
                    testQueue.SendMessagesBatch(serviceBusConnectionString, queueName, messages);
                else if (processorRadio.Checked)
                    testQueueProcessor.SendMessagesBatch(serviceBusConnectionString, queueName, messages);
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

        public void DisplayLogsAsyncStream(ServiceBusReceivedMessage receivedMessage)
        {
            LogsList0.Add(new
            {
                No = receivedMessage.SequenceNumber,
                ENo = receivedMessage.EnqueuedSequenceNumber,
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
                if (asyncStreamRadio.Checked)
                    testQueue.ReceiveMessages(serviceBusConnectionString, queueName);
                else if (processorRadio.Checked)
                    testQueueProcessor.ReceiveMessages(serviceBusConnectionString, queueName);
            });
        }

        public void DisplayLogsForSingle(ProcessMessageEventArgs arg)
        {
            LogsList0.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
                ENo = arg.Message.SequenceNumber,
                Entry = arg.Message.EnqueuedTime.LocalDateTime.ToString(timeStampFormat),
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

        private void stopReceive_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (processorRadio.Checked)
                    testQueueProcessor.StopReceiving();
            });
        }
    }
}
