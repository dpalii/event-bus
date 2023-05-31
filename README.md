# Event-bus

In this example, we define an EventBus class that serves as a central hub for events. It maintains a dictionary where the key is the event type and the value is a list of subscribers interested in that event. Subscribers implement the ISubscriber interface, and publishers implement the IPublisher interface.

The EventBus provides methods for subscribing, unsubscribing, and publishing events. When an event is published, the EventBus notifies all the subscribers that are interested in that event.

In the Main method, we create an instance of the EventBus and two Subscriber instances. We subscribe the subscribers to the EventData event type and then publish events using the event bus. Finally, we demonstrate how to unsubscribe a subscriber from receiving further events.

When you run the program, you'll see that both subscribers receive the events they are subscribed to. When the first subscriber is unsubscribed, it no longer receives events.
