using Microsoft.Owin;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Common {
    public class HealthCheckMiddleware : OwinMiddleware {
        private const string _endpointUrl = "/_healthcheck_";

        public HealthCheckMiddleware(OwinMiddleware next) : base(next) {
        }

        public override async Task Invoke(IOwinContext context) {
            if (context.Request.Path.Value.StartsWith(_endpointUrl, StringComparison.OrdinalIgnoreCase)) {
                var response = GetSignature();
                await context.Response.WriteAsync(response);
            } else {
                await Next.Invoke(context);
            }
        }

        public static string GetSignature() {
            var sb = new StringBuilder(127);
            sb.AppendLine("<pre>");
            sb.AppendLine(Environment.MachineName);
            sb.AppendLine(CultureInfo.CurrentCulture.Name);
            sb.AppendLine(DateTime.UtcNow.ToString("O"));
            sb.AppendLine("</pre>");
            return sb.ToString();
        }
    }
}
