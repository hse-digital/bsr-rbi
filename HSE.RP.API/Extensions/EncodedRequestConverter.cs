using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HSE.RP.API.Extensions
{
    public class EncodedRequest
    {
        private readonly string? body;

        public EncodedRequest(string? body)
        {
            this.body = body;
        }

        public T? GetDecodedData<T>() where T : class
        {
            if (string.IsNullOrEmpty(body)) return null;

            var content = body;
            if (body.StartsWith("base64:"))
            {
                content = Encoding.UTF8.GetString(Convert.FromBase64String(content[7..]));
            }

            return JsonSerializer.Deserialize<T>(content);
        }
    }

    public class EncodedRequestConverter : IInputConverter
    {
        public async ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
        {
            try
            {
                var req = context!.FunctionContext.GetHttpRequestData()!;
                var body = await req.ReadAsStringAsync();

                return ConversionResult.Success(new EncodedRequest(body));
            }
            catch (Exception e)
            {
                return ConversionResult.Failed(e);
            }
        }
    }
}
