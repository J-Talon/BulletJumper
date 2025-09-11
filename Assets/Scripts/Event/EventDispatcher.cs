using System;

namespace Event
{
    public class EventDispatcher<T>
    {
        public event Action<T> action;

        public void dispatchEvent(T data)
        {
            action?.Invoke(data);
        }
    }
}