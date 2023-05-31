using System;
using System.Collections.Generic;

// Event arguments for a custom event
public class CustomEventArgs : EventArgs
{
    public string Message { get; set; }

    public CustomEventArgs(string message)
    {
        Message = message;
    }
}

// Event publisher class
public class EventPublisher
{
    // Event declaration
    public event EventHandler<CustomEventArgs>? CustomEvent;

    // Method to raise the custom event
    public void RaiseEvent(string message)
    {
        // Create event arguments
        var eventArgs = new CustomEventArgs(message);

        // Raise the event
        OnCustomEvent(eventArgs);
    }

    // Event raising method
    protected virtual void OnCustomEvent(CustomEventArgs e)
    {
        CustomEvent?.Invoke(this, e);
    }
}

// Event subscriber class
public class EventSubscriber
{
    // Event handler method
    public void HandleEvent(object sender, CustomEventArgs e)
    {
        Console.WriteLine($"Event received: {e.Message}");
    }
}

// Event bus class
public class EventBus
{
    private Dictionary<Type, List<Delegate>> eventHandlers;

    public EventBus()
    {
        eventHandlers = new Dictionary<Type, List<Delegate>>();
    }

    // Subscribe to an event type with a handler method
    public void Subscribe<TEvent>(Action<object, TEvent> handler) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);

        if (!eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType] = new List<Delegate>();
        }

        eventHandlers[eventType].Add(handler);
    }

    // Unsubscribe from an event type with a handler method
    public void Unsubscribe<TEvent>(Action<object, TEvent> handler) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);

        if (eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType].Remove(handler);
        }
    }

    // Publish an event
    public void Publish<TEvent>(object sender, TEvent e) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);

        if (eventHandlers.ContainsKey(eventType))
        {
            foreach (var handler in eventHandlers[eventType])
            {
                handler.DynamicInvoke(sender, e);
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Create instances of the publisher, subscriber, and event bus
        var publisher = new EventPublisher();
        var subscriber = new EventSubscriber();
        var eventBus = new EventBus();

        // Subscribe the subscriber to the publisher's event via the event bus
        eventBus.Subscribe<CustomEventArgs>(subscriber.HandleEvent);

        // Publish an event using the publisher
        publisher.RaiseEvent("Hello, event subscribers!");

        // Publish the same event using the event bus
        eventBus.Publish(publisher, new CustomEventArgs("Hello again, event subscribers!"));

        // Unsubscribe the subscriber from the publisher's event via the event bus
        eventBus.Unsubscribe<CustomEventArgs>(subscriber.HandleEvent);

        // Publish an event again to test the unsubscribe functionality
        publisher.RaiseEvent("Event after unsubscribe");

        Console.ReadLine();
    }
}
