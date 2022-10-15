using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using HL.Loyalty.EventStore.Test.ValueObjects;

namespace HL.Loyalty.EventStore.Test
{
    [TestClass]
    public class AzureDocumentDbEventStoreTest
    {
        private IEventStore _eventStore;
        private IEventStore _eventStoreWithPartitions;

        [TestInitialize]
        public void Initialize()
        {
            _eventStore = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                "LoyaltyEventStore",
                "loyalty_test"
                );

            _eventStoreWithPartitions = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                "LoyaltyEventStore",
                "loyalty_test_5"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullEndPoint_ArgumentException()
        {
            _eventStore = new AzureDocumentDbEventStore(
                null,
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                "LoyaltyEventStore",
                "SaveReceiptRequest_EventStore"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullAuthKey_ArgumentException()
        {
            _eventStore = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                null,
                "LoyaltyEventStore",
                "SaveReceiptRequest_EventStore"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullDataBase_ArgumentException()
        {
            _eventStore = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                null,
                "SaveReceiptRequest_EventStore"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullCollectionId_ArgumentException()
        {
            _eventStore = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                "LoyaltyEventStore",
                null
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task SaveAsync_NullObject_ArgumentException()
        {
            var result = await _eventStore.SaveAsync<string>(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task SaveAsync_Valid_True()
        {
            var result = await _eventStore.SaveAsync(new KeyValuePair<string, string>("key", "value"));
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task SaveAsync_ValidDocument_True()
        {
            var person = new Person
            {
                SomeId = Guid.NewGuid().ToString()
            };

            var result = await _eventStore.SaveAsync(person);
            Assert.IsTrue(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(person.SomeId));
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task SaveAsync_ValidDocumentPartitionKey_True()
        {
            var person = new Person
            {
                SomeId = Guid.NewGuid().ToString(),
                Name = "Hi"
            };

            var result = await _eventStoreWithPartitions.SaveAsync(person, person.Name);
            Assert.IsTrue(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(person.SomeId));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Read_EmptyId_ArgumentException()
        {
            var result = _eventStore.Read<Person>("  ");
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Read_NullId_ArgumentException()
        {
            var result = _eventStore.Read<Person>(id: null);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Read_NullExpression_ArgumentException()
        {
            var result = _eventStore.Read<Person>(expression: null);
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Read_ValidId_NotEmpty()
        {
            var id = Guid.NewGuid().ToString();
            await _eventStore.SaveAsync(new Professor
            {
                Id = id,
                LastName = id,
                Name = "A S M"
            });

            var result = _eventStore.Read<Professor>(id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Read_ValidIdWithPartitionKey_NotEmpty()
        {
            var name = "A S M";
            var id = Guid.NewGuid().ToString();
            await _eventStoreWithPartitions.SaveAsync(new Professor
            {
                Id = id,
                LastName = id,
                Name = name
            }, name);

            var result = _eventStoreWithPartitions.Read<Professor>(id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Read_ValidExpression_NotEmpty()
        {
            var name = Guid.NewGuid().ToString();
            await _eventStore.SaveAsync(new Person
            {
                LastName = Guid.NewGuid().ToString(),
                Name = name
            });

            var result = _eventStore.Read<Person>(p => p.Name == name);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result[0].Name == name);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task SaveWithId_Valid_NotEmpty()
        {
            var id = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var newName = Guid.NewGuid().ToString();
            var person = new Person
            {
                SomeId = id,
                Name = name
            };

            await _eventStore.SaveAsync(person);
            Assert.IsTrue(person.SomeId == id);

            person.Name = newName;

            var result = await _eventStore.SaveAsync(person);
            var persons = _eventStore.Read<Person>(p => p.SomeId == id);
            Assert.IsNotNull(persons);
            Assert.IsTrue(persons.Count > 0);
            Assert.IsTrue(persons[0].Name == newName);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void CreateQADatabaseAndCollection()
        {
            var eventStore = new AzureDocumentDbEventStore(
                new Uri("https://loyalty.documents.azure.com:443"),
                "nLBA7HlaqWWnTKwCXN0K6yljf3inZ4OkTLtJ3qghJhMAJZC3Kv8hWEeU2ZizqtdWNQyQD1FASfG84qWcc9y3rA==",
                "LoyaltyEventStore",
                "loyalty_test_2"
                );

            eventStore.CreateDatabase("LoyaltyEventStore");
            eventStore.CreateCollection("LoyaltyEventStore", "loyalty_test_5", new string[] { "/Name" });
            eventStore.CreateCollection("LoyaltyEventStore", "LoyaltyCollection::Hl.Loyalty.ReceiptManager.ValueObjects.SaveReceiptRequest", null);
            eventStore.CreateCollection("LoyaltyEventStore", "LoyaltyCollection::Hl.Loyalty.ReceiptConversionManager.Common.Events.LoyaltySaveReceiptEvent", null);
            eventStore.CreateCollection("LoyaltyEventStore", "LoyaltyCollection::HL.Loyalty.CommandsManager.Common.Commands", null);
            eventStore.CreateCollection("LoyaltyEventStore", "LoyaltyCollection::HL.Loyalty.DataSyncManager.Common.Commands", null);
        }
    }
}
