using System;
using System.Collections.Generic;

namespace IngameScript
{
    public class CommandExecutor
    {
        private readonly ILoggable _logger;
        private readonly Dictionary<string, CommandModel> _commands = new Dictionary<string, CommandModel>();

        public CommandExecutor(ILoggable logger)
        {
            _logger = logger;
        }

        public void Add(CommandModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_commands.ContainsKey(command.Name))
                throw new InvalidOperationException($"A command with the name '{command.Name}' already exists.");

            _logger.LogLine($"Command '{command.Name}' added.");
            _commands[command.Name] = command;
        }
        
        public void Add(string name, Action<string[]> action) => Add(new CommandModel(name, action));

        public void Add(string name, Action action) => Add(name, (args) => action());

        public bool TryExecute(string name, params string[] parameters)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            CommandModel command;
            if (_commands.TryGetValue(name, out command))
            {
                _logger.LogLine($"Executing command '{name}' with parameters: {string.Join(", ", parameters)}");
                command.Action(parameters);
                _logger.LogLine($"Command '{name}' executed.");

                return true;
            }

            _logger.LogLine($"Command '{name}' not found.");
            return false;
        }

        public bool Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (!_commands.ContainsKey(name))
            {
                _logger.LogLine($"Command '{name}' not found.");
                return false;
            }
            else
            {
                _logger.LogLine($"Command '{name}' removed.");
                return _commands.Remove(name);
            }
        }
    }
}