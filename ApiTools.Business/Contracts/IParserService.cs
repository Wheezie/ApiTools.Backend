using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business.Contracts
{
    public interface IParserService
    {
        /// <summary>
        /// Parse a template file and by relacing specific key interpolated strings with the provided values.
        /// </summary>
        /// <param name="templateFile">Template file path to read from.</param>
        /// <param name="stringsToParse">Interpolated keys to be replaced with the values in the template.</param>
        /// <param name="cancellationToken">Taks cancellationtoken</param>
        /// <exception cref="ArgumentException">When invalid arguments are provided</exception>
        /// <exception cref="IOException">An error occurred parsing/reading the template</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        /// <returns>The parsed HTML string</returns>
        Task<string> ParseFromTemplateAsync(string templateFile, IDictionary<string, string> stringsToParse, CancellationToken cancellationToken = default);
        /// <summary>
        /// Parse a template file and by relacing specific key interpolated strings with the provided values.
        /// </summary>
        /// <param name="templateFile">Template file path to read from.</param>
        /// <param name="stringsToParse">Interpolated keys to be replaced with the values in the template.</param>
        /// <param name="cancellationToken">Taks cancellationtoken</param>
        /// <exception cref="ArgumentException">When invalid arguments are provided</exception>
        /// <exception cref="IOException">An error occurred parsing/reading the template</exception>
        /// <exception cref="OperationCanceledException">Operation token was cancelled</exception>
        /// <returns>The parsed HTML string</returns>Z
        Task<string> ParseFromTemplateAsync(StreamReader contentStream, IDictionary<string, string> stringsToParse, CancellationToken cancellationToken = default);
    }
}
