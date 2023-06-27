namespace TestServiceBusTopics
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        string serviceBusConnectionString = "Endpoint=sb://akshayraut.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DGRx184lUgSrKym8p+z66jV+qJ2FMd7l0+ASbMrVlLs=";
        string topicName = "testsessiontopic";
        string subscriptionName = "TestSubscription";
        public List<dynamic> LogsList0 { get; set; }
        public List<dynamic> LogsList1 { get; set; }
        public List<dynamic> LogsList2 { get; set; }

        private readonly SynchronizationContext synchronizationContext;
        string timeStampFormat = "HH:mm:ss.fff";

        Dictionary<string, int> messages;
        ConcurrentDictionary<string, ServiceBusSessionProcessor> serviceBusSessionProcessors = new ConcurrentDictionary<string, ServiceBusSessionProcessor>();

        public Form1()
        {
            InitializeComponent();
            LogsList0 = new List<dynamic>();
            LogsList1 = new List<dynamic>();
            LogsList2 = new List<dynamic>();
            synchronizationContext = SynchronizationContext.Current; //context from UI thread
            //logsDataGridView0.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //logsDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //logsDataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
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
            Task.Run(SendMessagesSingles);
        }

        private async Task SendMessagesSingles()
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

            using (ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync())
            {
                #region Sending one message at a time
                try
                {
                    for (int i = 0; i < messages.Count; i++)
                    {
                        var serviceBusMessage = new ServiceBusMessage(messages.ElementAt(i).Key);
                        serviceBusMessage.SessionId = $"{messages.ElementAt(i).Value}";

                        await serviceBusSender.SendMessageAsync(serviceBusMessage);
                    }

                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        MessageBox.Show("Messages have been sent.");
                    }), null);
                }
                finally
                {
                    await serviceBusSender.DisposeAsync();
                    await serviceBusClient.DisposeAsync();
                }
                #endregion
            }
        }

        private async Task SendMessagesBatch()
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

            using (ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync())
            {
                #region Sending messages in a batch
                for (int i = 0; i < messages.Count; i++)
                {
                    var serviceBusMessage = new ServiceBusMessage(messages.ElementAt(i).Key);
                    serviceBusMessage.SessionId = $"{messages.ElementAt(i).Value}";

                    if (!serviceBusMessageBatch.TryAddMessage(serviceBusMessage))
                    {
                        throw new Exception("Message is too large for the topic");
                    }
                }
                try
                {
                    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        MessageBox.Show("Messages have been sent.");
                    }), null);
                }
                finally
                {
                    await serviceBusSender.DisposeAsync();
                    await serviceBusClient.DisposeAsync();
                }
                #endregion
            }
        }

        private void receive_Click(object sender, EventArgs e)
        {
            Task.Run(ReceiveMessages);
        }

        private async Task ReceiveMessages()
        {
            for (int i = 0; i < serviceBusSessionProcessors.Count; i++)
            {
                if (!serviceBusSessionProcessors.ElementAt(i).Value.IsClosed)
                    await serviceBusSessionProcessors.ElementAt(i).Value.StopProcessingAsync();
                if (!serviceBusSessionProcessors.ElementAt(i).Value.IsProcessing)
                    await serviceBusSessionProcessors.ElementAt(i).Value.CloseAsync();
            }

            ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 3;

            Parallel.For(0, 3, parallelOptions, async (i) =>
            {
                // create the options to use for configuring the processor
                var serviceBusSessionProcessorOptions = new ServiceBusSessionProcessorOptions()
                {
                    // By default after the message handler returns, the processor will complete the message
                    // If I want more fine-grained control over settlement, I can set this to false.
                    AutoCompleteMessages = true,

                    // I can also allow for processing multiple sessions
                    MaxConcurrentSessions = 1,

                    // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
                    // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
                    // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
                    MaxConcurrentCallsPerSession = 1,

                    // Processing can be optionally limited to a subset of session Ids.
                    SessionIds = { i.ToString() }
                };

                var serviceBusSessionProcessor = serviceBusClient.CreateSessionProcessor(topicName, subscriptionName, serviceBusSessionProcessorOptions);

                try
                {
                    if (i == 0)
                        serviceBusSessionProcessor.ProcessMessageAsync += ServiceBusProcessor0_ProcessMessageAsync;
                    else if (i == 1)
                        serviceBusSessionProcessor.ProcessMessageAsync += ServiceBusProcessor1_ProcessMessageAsync;
                    else
                        serviceBusSessionProcessor.ProcessMessageAsync += ServiceBusProcessor2_ProcessMessageAsync;

                    serviceBusSessionProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

                    await serviceBusSessionProcessor.StartProcessingAsync();
                    serviceBusSessionProcessors.TryAdd(serviceBusSessionProcessor.Identifier, serviceBusSessionProcessor);

                    await Task.Delay(600000);
                    await serviceBusSessionProcessor.StopProcessingAsync();

                    if (!serviceBusSessionProcessor.IsProcessing)
                    {
                        await serviceBusSessionProcessor.CloseAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await serviceBusSessionProcessor.DisposeAsync();
                }
                await serviceBusClient.DisposeAsync();
            });
        }

        private async Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            await Task.Run(async () =>
            {
                serviceBusSessionProcessors.TryGetValue(arg.Identifier, out ServiceBusSessionProcessor serviceBusSessionProcessor);

                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    MessageBox.Show(arg.Exception.Message);
                }), null);
                if (serviceBusSessionProcessor != null)
                {
                    await serviceBusSessionProcessor.StopProcessingAsync();
                }
            });
        }

        private async Task ServiceBusProcessor0_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    DisplayLogs(LogsList0, arg, logsDataGridView0);
                }), null);
            });
        }

        private async Task ServiceBusProcessor1_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    //DisplayLogs(LogsList1, arg, logsDataGridView1);
                    DisplayLogs(LogsList0, arg, logsDataGridView0);
                }), null);
            });
        }

        private async Task ServiceBusProcessor2_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    //DisplayLogs(LogsList2, arg, logsDataGridView2);
                    DisplayLogs(LogsList0, arg, logsDataGridView0);
                }), null);
            });
        }

        private void DisplayLogs(List<dynamic> LogsList, ProcessSessionMessageEventArgs arg, DataGridView logsDGV)
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
            logsDGV.DataSource = bs;
            logsDGV.Refresh();
        }

        private void clearLogs_Click(object sender, EventArgs e)
        {
            BindingSource bs0 = new BindingSource();
            bs0.DataSource = LogsList0;
            LogsList0 = new List<dynamic>();
            logsDataGridView0.DataSource = bs0;
            logsDataGridView0.Refresh();

            BindingSource bs1 = new BindingSource();
            bs1.DataSource = LogsList1;
            LogsList1 = new List<dynamic>();
            logsDataGridView1.DataSource = bs1;
            logsDataGridView1.Refresh();

            BindingSource bs2 = new BindingSource();
            bs2.DataSource = LogsList2;
            LogsList2 = new List<dynamic>();
            logsDataGridView2.DataSource = bs2;
            logsDataGridView2.Refresh();
        }
    }
}
