using LanguageLearning.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Common
{
    [TestClass]
    public class AbstractRepositoryTests
    {
        private Mock<Serializer<ObjectId>> serializer;
        private AbstractRepository<ObjectId> repository;

        [TestInitialize]
        public void Init()
        {
            serializer = new Mock<Serializer<ObjectId>>();
            repository = new HelperRepository(serializer.Object);
        }

        [TestMethod]
        public void TestGetPositive()
        {
            // Init
            ObjectId objectId = new ObjectId() { Id = "test" };
            List<ObjectId> objects = new List<ObjectId>() { objectId };
            serializer.Setup(x => x.Load()).Returns(objects);

            // Test
            ObjectId result = repository.Get("test");

            // Verify
            Assert.AreEqual(objectId, result);

            serializer.Verify(x => x.Load(), Times.Once);
        }

        [TestMethod]
        public void TestGetNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<ObjectId>());

            // Test
            ObjectId result = repository.Get("test");

            // Verify
            Assert.IsNull(result);

            serializer.Verify(x => x.Load(), Times.Once);
        }

        [TestMethod]
        public void TestCreatePositive()
        {
            // Init
            List<ObjectId> objects = new List<ObjectId>();
            ObjectId newObject = new ObjectId() { Id = "test" };
            serializer.Setup(x => x.Load()).Returns(objects);

            // Preverify
            Assert.IsFalse(objects.Contains(newObject));

            // Test
            repository.Create(newObject);

            // Verify
            Assert.IsTrue(objects.Contains(newObject));

            serializer.Verify(x => x.Load(), Times.Once);
            serializer.Verify(x => x.Save(objects), Times.Once);
        }

        [TestMethod]
        public void TestUpdatePositive()
        {
            // Init
            ObjectId preUpdate = new ObjectId() { Id = "test" };
            ObjectId postUpdate = new ObjectId() { Id = "test" };
            List<ObjectId> objects = new List<ObjectId>() { preUpdate };
            serializer.Setup(x => x.Load()).Returns(objects);

            // Preverify
            Assert.IsTrue(objects.Contains(preUpdate));
            Assert.IsFalse(objects.Contains(postUpdate));

            // Test
            repository.Update(postUpdate);

            // Verify
            Assert.IsFalse(objects.Contains(preUpdate));
            Assert.IsTrue(objects.Contains(postUpdate));

            serializer.Verify(x => x.Load(), Times.Once);
            serializer.Verify(x => x.Save(objects), Times.Once);
        }

        [TestMethod]
        public void TestDeletePositive()
        {
            // Init 
            ObjectId objectId = new ObjectId() { Id = "test" };
            List<ObjectId> objects = new List<ObjectId>() { objectId };
            serializer.Setup(x => x.Load()).Returns(objects);

            // Preverify
            Assert.IsTrue(objects.Contains(objectId));

            // Test
            repository.Delete(objectId);

            // Verify
            Assert.IsFalse(objects.Contains(objectId));

            serializer.Verify(x => x.Load(), Times.Once);
            serializer.Verify(x => x.Save(objects), Times.Once);
        }

        [TestCleanup]
        public void Final()
        {
            serializer.VerifyNoOtherCalls();
        }
    }

    public class HelperRepository : AbstractRepository<ObjectId>
    {
        public HelperRepository(Serializer<ObjectId> serializer)
        {
            this.serializer = serializer;
        }
    }

    public class ObjectId : IId
    {
        public string Id { get; init; }
    }
}
