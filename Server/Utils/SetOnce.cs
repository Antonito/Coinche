using System;
namespace Coinche.Server.Utils
{
    /// <summary>
    /// Allows a value to be set only once
    /// </summary>
    public sealed class SetOnce<T>
    {
        /// <summary>
        /// The value.
        /// </summary>
        private T _value;

        /// <summary>
        /// Has the value been set ?
        /// </summary>
        private bool _hasValue;

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Coinche.Server.Utils.SetOnce`1"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Coinche.Server.Utils.SetOnce`1"/>.</returns>
        public override string ToString()
        {
            return _hasValue ? Convert.ToString(_value) : "";
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
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
