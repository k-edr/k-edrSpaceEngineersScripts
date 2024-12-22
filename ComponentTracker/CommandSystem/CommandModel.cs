using System;

namespace IngameScript
{
    public class CommandModel
    {
        public string Name { get; }
        public Action<string[]> Action { get; }

        public CommandModel(string name, Action<string[]> action)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Command name cannot be null or empty.", nameof(name));

            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;
            if (action == null) throw new ArgumentNullException(nameof(action));
            Action = action;
        }
    }
}