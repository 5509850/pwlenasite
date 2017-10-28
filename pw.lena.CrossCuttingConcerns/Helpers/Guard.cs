using System;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public static class Guard
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> exception if the passed object equals null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <exception cref="ArgumentNullException">
        ///     The supplied object <paramref name="obj"/> equals to <see langword="null"/>.
        /// </exception>
        public static void ThrowIfNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Empty);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> exception if the passed object equals null.
        /// </summary>
        /// <param name="obj">>The object to check.</param>
        /// <param name="parameterName">Name of the parameter. Will be displayed with the exception details.</param>
        /// <exception cref="ArgumentNullException">
        ///     The supplied object <paramref name="obj"/> equals to <see langword="null"/>.
        /// </exception>
        public static void ThrowIfNull(object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> exception if the passed object equals null.
        /// </summary>
        /// <param name="obj">>The object to check.</param>
        /// <param name="parameterName">Name of the parameter. Will be displayed with the exception details.</param>
        /// <param name="message">Exception message to give when value is invalid.</param>
        /// <exception cref="ArgumentNullException">
        ///     The supplied object <paramref name="obj"/> equals to <see langword="null"/>.
        /// </exception>
        public static void ThrowIfNull(object obj, string parameterName, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the passed string is an empty string.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="str"/> is <see langword="null"/> or empty.
        /// </exception>
        public static void ThrowIfEmptyString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(string.Empty);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the passed string is an empty string.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="parameterName">Name of the parameter. Will be displayed with the exception details.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="str"/> is <see langword="null"/> or empty.
        /// </exception>
        public static void ThrowIfEmptyString(string str, string parameterName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the passed string is an empty string.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="parameterName">Name of the parameter. Will be displayed with the exception details.</param>
        /// <param name="message">Exception message to give when value is invalid.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="str"/> is <see langword="null"/> or empty.
        /// </exception>
        public static void ThrowIfEmptyString(string str, string parameterName, string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }
    }
}
