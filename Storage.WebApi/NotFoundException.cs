using System;

namespace Storage.WebApi.Exceptions
{
    public class NotFoundException : Exception
    {
        #region Init
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
    }
}
