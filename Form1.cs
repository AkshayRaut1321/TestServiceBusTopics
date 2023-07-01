namespace TestServiceBusTopics
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using TestServiceBusTopics.TestingScenarios;

    public partial class Form1 : Form
    {
        string serviceBusConnectionString = "Endpoint=sb://akshayraut.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ge+2CAE0f+xd+clzKonx05m5xZdsAj/Mo+ASbM6KG6A=";
        string topicName = "testsessiontopic";

        string timeStampFormat = "HH:mm:ss.fff";
        SingleSubscription singleSubscription;
        MultipleSubscription multipleSubscription;

        private readonly SynchronizationContext synchronizationContext; //context from UI thread

        Dictionary<string, int> messages;
        public List<dynamic> LogsList0 { get; set; }
        public List<dynamic> LogsList1 { get; set; }
        public List<dynamic> LogsList2 { get; set; }

        public Form1()
        {
            InitializeComponent();
            singleSubscription = new SingleSubscription(this);
            multipleSubscription = new MultipleSubscription(this);
            singleRadio.Checked = true;
            LogsList0 = new List<dynamic>();
            LogsList1 = new List<dynamic>();
            LogsList2 = new List<dynamic>();
            synchronizationContext = SynchronizationContext.Current;
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
                if (singleRadio.Checked)
                    singleSubscription.SendMessagesBatch(serviceBusConnectionString, topicName, messages);
                else
                    multipleSubscription.SendMessagesBatch(serviceBusConnectionString, topicName, messages);
            });
        }

        private void receive_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (singleRadio.Checked)
                    singleSubscription.ReceiveMessages(serviceBusConnectionString, topicName);
                else
                    multipleSubscription.ReceiveMessages(serviceBusConnectionString, topicName);
            });
        }

        public void DisplayLogsForSingle(ProcessSessionMessageEventArgs arg)
        {
            LogsList0.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
                SID = arg.SessionId,
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

        public void DisplayLogsForMultiSub0(ProcessSessionMessageEventArgs arg)
        {
            LogsList0.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
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

        public void DisplayLogsForMultiSub1(ProcessSessionMessageEventArgs arg)
        {
            LogsList1.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
                Entry = arg.Message.EnqueuedTime.LocalDateTime.ToString(timeStampFormat),
                Exit = DateTime.Now.ToString(timeStampFormat)
            });

            BindingSource bs = new BindingSource();
            bs.DataSource = LogsList1;
            //Send the update to our UI thread
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                logsDataGridView1.DataSource = bs;
                logsDataGridView1.Refresh();
            }), null);
        }

        public void DisplayLogsForMultiSub2(ProcessSessionMessageEventArgs arg)
        {
            LogsList2.Add(new
            {
                No = arg.Message.EnqueuedSequenceNumber,
                Entry = arg.Message.EnqueuedTime.LocalDateTime.ToString(timeStampFormat),
                Exit = DateTime.Now.ToString(timeStampFormat)
            });

            BindingSource bs = new BindingSource();
            bs.DataSource = LogsList2;
            //Send the update to our UI thread
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                logsDataGridView2.DataSource = bs;
                logsDataGridView2.Refresh();
            }), null);
        }

        public void clearLogs_Click(object sender, EventArgs e)
        {
            LogsList0 = new List<dynamic>();
            BindingSource bs0 = new BindingSource();
            bs0.DataSource = LogsList0;
            logsDataGridView0.DataSource = bs0;
            logsDataGridView0.Refresh();

            LogsList1 = new List<dynamic>();
            BindingSource bs1 = new BindingSource();
            bs1.DataSource = LogsList1;
            logsDataGridView1.DataSource = bs1;
            logsDataGridView1.Refresh();

            LogsList2 = new List<dynamic>();
            BindingSource bs2 = new BindingSource();
            bs2.DataSource = LogsList2;
            logsDataGridView2.DataSource = bs2;
            logsDataGridView2.Refresh();
        }

        private void stopReceive_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (singleRadio.Checked)
                    singleSubscription.StopReceiving();
                else
                    multipleSubscription.StopReceiving();
            });
        }
    }
}
