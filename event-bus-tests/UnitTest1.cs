using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace YourNamespace
{
    [TestClass]
    public class EventBusTests
    {
        [TestMethod]
        public void Publish_EventHandledBySubscribers()
        {
            // Arrange
            var eventBus = new EventBus();
            var handlerMock1 = new Mock<Action<object, CustomEventArgs>>();
            var handlerMock2 = new Mock<Action<object, CustomEventArgs>>();
            eventBus.Subscribe<CustomEventArgs>(handlerMock1.Object);
            eventBus.Subscribe<CustomEventArgs>(handlerMock2.Object);

            var publisher = new object();
            var eventArgs1 = new CustomEventArgs("Test Event 1");
            var eventArgs2 = new CustomEventArgs("Test Event 2");

            // Act
            eventBus.Publish(publisher, eventArgs1);
            eventBus.Unsubscribe(handlerMock1.Object);
            eventBus.Publish(publisher, eventArgs2);
            eventBus.Unsubscribe(handlerMock2.Object);
            // Assert
            handlerMock1.Verify(h => h.Invoke(publisher, eventArgs1), Times.Once());
            handlerMock1.Verify(h => h.Invoke(publisher, eventArgs2), Times.Never());
            handlerMock2.Verify(h => h.Invoke(publisher, eventArgs1), Times.Once());
            handlerMock2.Verify(h => h.Invoke(publisher, eventArgs2), Times.Once());
        }
    }
}