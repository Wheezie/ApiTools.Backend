using ApiTools.Business.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTools.Business
{
    internal class ParserService : IParserService
    {
        public Task<string> ParseFromTemplateAsync(string templateFile, IDictionary<string, string> stringsToParse, CancellationToken cancellationToken = default)
        {
            StreamReader reader = new StreamReader(templateFile);
            return ParseFromTemplateAsync(reader, stringsToParse, cancellationToken);
        }

        public Task<string> ParseFromTemplateAsync(StreamReader contentStream, IDictionary<string, string> stringsToParse, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                StringBuilder htmlBuilder = new StringBuilder();
                using (contentStream)
                {
                    char[] buff = new char[256];
                    int count = buff.Length;
                    while (count > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        count = contentStream.Read(buff, 0, buff.Length);
                        htmlBuilder.Append(buff, 0, count);
                    }
                }

                if (stringsToParse != null && stringsToParse.Count > 0)
                {
                    foreach (var kvPair in stringsToParse)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        htmlBuilder.Replace($"{{{kvPair.Key}}}", kvPair.Value);
                    }
                }

                return htmlBuilder.ToString();
            }, cancellationToken);
        }
    }
}
