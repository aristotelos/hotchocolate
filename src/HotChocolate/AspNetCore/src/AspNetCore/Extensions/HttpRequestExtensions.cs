using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace HotChocolate.AspNetCore;

internal static class HttpRequestExtensions
{
    internal static bool AcceptHeaderContainsHtml(this HttpRequest request)
    {
        return request.Headers.TryGetValue(HeaderNames.Accept, out var values) &&
            values.Count > 0 && (values[0]?.Contains("text/html") ?? false);
    }

    internal static bool IsGetOrHeadMethod(this HttpRequest request)
    {
        return HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method);
    }

    internal static bool PathEndsInSlash(this HttpRequest request)
    {
        return request.Path.Value?.EndsWith("/", StringComparison.Ordinal) ?? false;
    }

    internal static bool TryMatchPath(
        this HttpRequest request,
        PathString matchUrl,
        bool forDirectory,
        out PathString subPath)
    {
        var path = request.Path;

        if (forDirectory && !request.PathEndsInSlash())
        {
            path += new PathString("/");
        }

        if (path.StartsWithSegments(matchUrl, out subPath))
        {
            return true;
        }

        return false;
    }
}
