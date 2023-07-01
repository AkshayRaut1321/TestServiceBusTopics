namespace TestServiceBusTopics.TestingScenarios
{
    using Azure.Messaging.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    internal class TestQueue
    {
        private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current; //context from UI thread

        QueueForm _form;

        public TestQueue(QueueForm form)
        {
            _form = form;
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

        public async Task ReceiveMessages(string serviceBusConnectionString, string queueName)
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 3;

            var serviceBusReceiverOptions = new ServiceBusReceiverOptions();
            serviceBusReceiverOptions.ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;

            var serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, serviceBusReceiverOptions);

            try
            {
                IAsyncEnumerable<ServiceBusReceivedMessage> receivedMessages = serviceBusReceiver.ReceiveMessagesAsync();

                await foreach (var item in receivedMessages)
                {
                    //Send the update to our UI thread
                    _form.DisplayLogsForSingle(item);
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
        }
    }
}