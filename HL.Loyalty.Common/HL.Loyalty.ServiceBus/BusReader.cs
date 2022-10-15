using HL.Loyalty.ServiceBus.Interfaces;
using HL.Loyalty.ServiceBus.ValueObjects;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HL.Loyalty.ServiceBus
{
    public class BusReader:IBusReader
    {
        private static SubscriptionClient _subscriptionClient = null;
        private int waitTimeSeconds = 3;
        private string serviceBusTopicName = "";
        private string serviceBusSubscriptionName = "";
        private string connectionString = "";

        public SubscriptionClient subscriptionClient
        {
            get
            {
                if (_subscriptionClient == null)
                {
                    _subscriptionClient = SubscriptionClient.CreateFromConnectionString
                            (connectionString, serviceBusTopicName, serviceBusSubscriptionName);
                }
                return _subscriptionClient;
            }
        }

        public BusReader()
        {
            serviceBusTopicName = CloudConfigurationManager.GetSetting("TopicName");
            serviceBusSubscriptionName = CloudConfigurationManager.GetSetting("SubscriptionName");
            connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            waitTimeSeconds = Int32.TryParse(CloudConfigurationManager.GetSetting("ServiceBusResponseWaitSeconds"), out waitTimeSeconds) ? waitTimeSeconds : 3;
        }      

        /// <summary>
        /// Pass the specific Topic and Suscription that you want to use to getTheMessages, if null 
        /// takes the default values
        /// </summary>
        public BusReader(string Topic = null, string suscription = null, string connection=null)
        {
            connectionString = !string.IsNullOrEmpty(connection) ? connection : CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            serviceBusTopicName = !string.IsNullOrEmpty(Topic) ?  Topic: CloudConfigurationManager.GetSetting("TopicName");
            serviceBusSubscriptionName = !string.IsNullOrEmpty(suscription) ? suscription: CloudConfigurationManager.GetSetting("SubscriptionName");
            waitTimeSeconds = Int32.TryParse(CloudConfigurationManager.GetSetting("ServiceBusResponseWaitSeconds"), out waitTimeSeconds) ? waitTimeSeconds : 10;            
        }


        /// <summary>
        /// Get a specific message bassed in their SessionID value and receive the specific 
        /// Type T as BusReaderResponse.Response
        /// </summary>
        /// <param name="sessionId">SeesionID(Guid) that identifies the message</param>
        /// <returns></returns>
        public BusReaderReponse getMessageByID<T>(string sessionId)
        {
            BrokeredMessage message = null;
            var result = new BusReaderReponse();
            MessageSession messageSession = null;
            if(string.IsNullOrEmpty(serviceBusTopicName) || string.IsNullOrEmpty(serviceBusSubscriptionName) || string.IsNullOrEmpty(connectionString))
            {
                result.Status = StatusResponse.Missingconfiguration;
                throw new ArgumentException("Missing Settings configuration");                
            }

            try
            {
                messageSession = subscriptionClient.AcceptMessageSession(sessionId, TimeSpan.FromSeconds(waitTimeSeconds));
                message = messageSession.Receive();
                if (message == null)
                    throw new TimeoutException("Wait for message response exceeded");

                result.Response = message.GetBody<T>(new DataContractSerializer(typeof(T)));
                message.Complete();
                result.Status = StatusResponse.Success;
            }
            catch (TimeoutException ex)
            {
                result.Exception = ex;
                result.Status = StatusResponse.Timeout;
            }
            catch(ArgumentException ex)
            {
                result.Exception = ex;
                result.Status = StatusResponse.Missingconfiguration;
            }
            catch (Exception ex)
            {
                result.Exception = ex;                
                result.Status = StatusResponse.Failure;
            }
            finally
            {
                if (message != null)
                    message.Abandon();
                if (messageSession != null && !messageSession.IsClosed)
                    messageSession.Close();
            }

            return result;

        }

        /// <summary>
        /// Get the first message from responses
        /// </summary>
        public BusReaderReponse getMessage<T>()
        {
            var result = new BusReaderReponse();
            try
            {
                SubscriptionClient client = SubscriptionClient.CreateFromConnectionString
                            (connectionString, serviceBusTopicName, serviceBusSubscriptionName);

                var sessionClient = client.AcceptMessageSession(TimeSpan.FromSeconds(waitTimeSeconds));

                var message = sessionClient.Receive();
                if (message == null)
                    throw new TimeoutException("Wait for message response exceeded");

                result.Response = message.GetBody<T>(new DataContractSerializer(typeof(T)));
                message.Complete();
                result.Status = StatusResponse.Success;
            }
            catch (TimeoutException)
            {
                result.Status = StatusResponse.Timeout;
            }
            catch (Exception ex)
            {
                result.Status = StatusResponse.Failure;
            }

            return result;

        }

 
    }
}
