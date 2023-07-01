namespace TestServiceBusTopics.TestingScenarios
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    internal class TestSingleSubscription
    {
        string subscriptionName = "TestSubscription";

        private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current; //context from UI thread

        TopicForm _form;
        List<ServiceBusSessionProcessor> processors;

        public TestSingleSubscription(TopicForm form)
        {
            _form = form;
            processors = new List<ServiceBusSessionProcessor>();
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

        public async Task SendMessagesBatch(string serviceBusConnectionString, string topicName, Dictionary<string, int> messages)
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

        public void ReceiveMessages(string serviceBusConnectionString, string topicName)
        {
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
                    await serviceBusSessionProcessor.DisposeAsync();
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

        private async Task ServiceBusProcessor0_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                _form.DisplayLogsForSingle(arg);
            });
        }

        private async Task ServiceBusProcessor1_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                _form.DisplayLogsForSingle(arg);
            });
        }

        private async Task ServiceBusProcessor2_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            await Task.Run(() =>
            {
                //Send the update to our UI thread
                _form.DisplayLogsForSingle(arg);
            });
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
            processors = new List<ServiceBusSessionProcessor>();
        }
    }
}