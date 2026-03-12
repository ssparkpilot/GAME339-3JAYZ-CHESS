using System;
using System.Linq;
using Game339.Shared.Diagnostics;

namespace Game339.Shared.Services.Implementation
{
    public class StringService : IStringService
    {
        private readonly IGameLog _log;

        public StringService(IGameLog log)
        {
            _log = log;
        }

        public string Reverse(string input)
        {
            var output = new string(input.Reverse().ToArray());
            _log.Info($"{nameof(StringService)}.{nameof(Reverse)} - {nameof(input)}: {input} - {nameof(output)}: {output}");
            return output;
        }
        
        public string ReverseWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var output = string.Join(" ",
                input
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Reverse());

            _log.Info($"{nameof(StringService)}.{nameof(ReverseWords)} - {nameof(input)}: {input} - {nameof(output)}: {output}");

            return output;
        }
    }
}