namespace TestServiceBusTopics.TestingScenarios
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    internal class TestQueueProcessor
    {
        private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current; //context from UI thread

        QueueForm _form;
        List<ServiceBusProcessor> processors;
        bool withDeadLetter = false;

        public TestQueueProcessor(QueueForm form)
        {
            _form = form;
            processors = new List<ServiceBusProcessor>();
        }

        public async Task SendMessagesSingles(string serviceBusConnectionString, string topicName, Dictionary<string, int> messages)
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

        public async Task SendMessagesBatch(string serviceBusConnectionString, string queueName, Dictionary<string, int> messages)
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);

            using (ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync())
            {
                #region Sending messages in a batch
                for (int i = 0; i < messages.Count; i++)
                {
                    var serviceBusMessage = new ServiceBusMessage(messages.ElementAt(i).Key);
                    serviceBusMessage.SessionId = $"{messages.ElementAt(i).Value}";
                    if (withDeadLetter)
                        serviceBusMessage.TimeToLive = TimeSpan.FromSeconds(10);

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

        public async Task ReceiveMessages(string serviceBusConnectionString, string queueName)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 2;

            Parallel.For(0, 2, parallelOptions, async (i) =>
            {
                ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

                var serviceBusReceiverOptions = new ServiceBusReceiverOptions();
                serviceBusReceiverOptions.ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;

                var serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, serviceBusReceiverOptions);
                try
                {
                    // create the options to use for configuring the processor
                    var serviceBusProcessorOptions = new ServiceBusProcessorOptions()
                    {
                        // By default after the message handler returns, the processor will complete the message
                        // If I want more fine-grained control over settlement, I can set this to false.
                        AutoCompleteMessages = false,
                        ReceiveMode = ServiceBusReceiveMode.PeekLock
                    };

                    var serviceBusSessionProcessor = serviceBusClient.CreateProcessor(queueName, serviceBusProcessorOptions);
                    if (i == 0)
                        serviceBusSessionProcessor.ProcessMessageAsync += ServiceBusProcessor_ProcessMessageAsync0;
                    else
                        serviceBusSessionProcessor.ProcessMessageAsync += ServiceBusProcessor_ProcessMessageAsync1;

                    serviceBusSessionProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

                    await serviceBusSessionProcessor.StartProcessingAsync();
                    processors.Add(serviceBusSessionProcessor);

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
                    await serviceBusReceiver.DisposeAsync();
                }
                await serviceBusClient.DisposeAsync();
            });
        }

        private async Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            await Task.Run(() =>
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    MessageBox.Show(arg.Exception.Message);
                }), null);
            });
        }

        private async Task ServiceBusProcessor_ProcessMessageAsync0(ProcessMessageEventArgs arg)
        {
            //Send the update to our UI thread
            _form.DisplayLogsForSingle(arg);
            await Task.Delay(1000);
            //Thread.Sleep(60000);
            await arg.CompleteMessageAsync(arg.Message);

            //await Task.Run(() =>
            //{
            //});
        }

        private async Task ServiceBusProcessor_ProcessMessageAsync1(ProcessMessageEventArgs arg)
        {
            //Send the update to our UI thread
            _form.DisplayLogsForSingle(arg);
            await Task.Delay(1000);
            //Thread.Sleep(60000);
            await arg.CompleteMessageAsync(arg.Message);

            //await Task.Run(() =>
            //{
            //});
        }

        public void StopReceiving()
        {
            processors.ForEach(async (a) =>
            {
                await a.StopProcessingAsync();

                if (!a.IsProcessing)
                {
                    await a.CloseAsync();
                }
            });
            processors = new List<ServiceBusProcessor>();
        }
    }
}