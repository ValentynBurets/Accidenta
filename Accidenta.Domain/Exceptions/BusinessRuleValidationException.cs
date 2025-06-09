using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Exceptions
{
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException() { }

        public BusinessRuleValidationException(string message) : base(message) { }

        public BusinessRuleValidationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
