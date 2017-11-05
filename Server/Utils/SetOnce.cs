using System;
namespace Coinche.Server.Utils
{
    public sealed class SetOnce<T>
    {
        private T _value;
        private bool _hasValue;

        public override string ToString()
        {
            return _hasValue ? Convert.ToString(_value) : "";
        }

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("Value not set");
                }
                return _value;
            }
            set
            {
                if (_hasValue)
                {
                    throw new InvalidOperationException("Value already set");
                }
                _value = value;
                _hasValue = true;
            }
        }

        public T ValueOrDefault { get { return _value; } }

        public static implicit operator T(SetOnce<T> value) { return value.Value; }
    }
}
