namespace Kubeless.Core.Exceptions
{
    using Microsoft.CodeAnalysis;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    [Serializable]
    public class CompilationException : Exception
    {
        public CompilationException() { }

        public CompilationException(string message) : base(message) { }

        public CompilationException(string message, Exception inner) : base(message, inner) { }

        protected CompilationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public CompilationException(IEnumerable<Diagnostic> errors)
        {
            this.Errors = errors;
        }

        private IEnumerable<Diagnostic> Errors { get; set; }

        public override string Message
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine($"Compilation has failed.");
                builder.AppendLine($"Error quantity: {Errors.Count()}");
                foreach (string message in this.FormatErrors(Errors))
                {
                    builder.AppendLine(message);
                }
                return builder.ToString();
            }
        }

        private IEnumerable<string> FormatErrors(IEnumerable<Diagnostic> errors)
        {
            return from e in errors
                   select $"{e.Id}: {e.GetMessage()}";
        }
    }
}
