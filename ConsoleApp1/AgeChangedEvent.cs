namespace ConsoleApp1
{
    class AgeChangedEvent : Event
    {
        public Person Target { get; }
        public int OldValue { get; }
        public int NewValue { get; }

        public AgeChangedEvent(Person target, int oldValue, int newValue)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override string ToString()
        {
            return $"age changed from {OldValue} to {NewValue}";
        }
    }
}