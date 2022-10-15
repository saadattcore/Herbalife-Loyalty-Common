using HL.Loyalty.ServiceBus.ValueObjects;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HL.Loyalty.ServiceBus
{
    public class BusPublisher
    {
        private string serviceBusTopicName = CloudConfigurationManager.GetSetting("TopicName");
        private string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

        public BusPublisher()
        {
            connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        }

        public BusPublisher(string Topic)
        {
            serviceBusTopicName = Topic;
            connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        }

        public BusPublisher(string connectionStr, string topic)
        {
            connectionString = connectionStr;
            serviceBusTopicName = topic;
        }

        /// <summary>
        /// Publish a Generic Object and returns the ID of the message
        /// </summary>
        public BusPublisherResponse Publish<T>(T value, string MessageType = null, string id = null)
        {
            var result = new BusPublisherResponse();
            try
            {
                if (string.IsNullOrEmpty(id))
                    id = Guid.NewGuid().ToString();

                TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, serviceBusTopicName);

                DataContractSerializer dcs = new DataContractSerializer(typeof(T));

                // Create message, passing our model
                BrokeredMessage message = new BrokeredMessage(value, dcs);
                message.ContentType = typeof(T).ToString();

                if (!string.IsNullOrEmpty(MessageType))
                    message.Properties["MessageType"] = MessageType;

                message.SessionId = id;

                // Send message to the topic
                Client.Send(message);

                result.sessionID = id;
                result.Status = StatusResponse.Success;

            }
            catch (Exception ex)
            {
                result.Response = ex.ToString();
                if (ex.InnerException != null)
                    result.Response += ex.InnerException.ToString();
                result.Status = StatusResponse.Failure;
            }
            return result;
        }

        internal void CreateTopic(string topic)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.TopicExists(topic))
            {
                namespaceManager.CreateTopic(topic);
            }
        }

        internal void CreateTopicSubscription(string topic, string subscription)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.SubscriptionExists(topic, subscription))
            {
                namespaceManager.CreateSubscription(topic, subscription);
            }
        }
    }
}
