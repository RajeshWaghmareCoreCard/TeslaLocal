using System;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.Exceptions
{
    class DuplicateStanException : Exception
    {
        public string DuplicateStanValue { get; private set; }
        public DuplicateStanException(string duplicateStanValue) : base($"Duplicate Stan value is not allowed. Duplicate value: {duplicateStanValue} ")
        {
            DuplicateStanValue = duplicateStanValue;
        }
    }
}