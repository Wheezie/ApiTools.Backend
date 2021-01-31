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
        public Task<string> ParseHtmlFromTemplate(string templateFile, IDictionary<string, string> stringsToParse, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                StringBuilder htmlBuilder;
                using (StreamReader reader = new StreamReader(templateFile))
                    htmlBuilder = new StringBuilder(await reader.ReadToEndAsync().ConfigureAwait(false));

                foreach (var kvPair in stringsToParse)
                    htmlBuilder.Replace(kvPair.Key, kvPair.Value);

                return htmlBuilder.ToString();
            });
        }
    }
}
