namespace AgileCoding.Library.Types.Exceptions
{
    using System;

    [Serializable]
    public class NoConnectionStringDefinedException : Exception
    {
        public NoConnectionStringDefinedException(string message) : base(message)
        {
        }
    }
}
