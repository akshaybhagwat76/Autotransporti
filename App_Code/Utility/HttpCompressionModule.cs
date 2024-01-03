using System;
using System.Web;
using System.IO.Compression;

public class HttpCompressionModule : IHttpModule
{
    private const string AcceptEncodingHeader = "Accept-Encoding";
    private const string ContentEncodingHeader = "Content-Encoding";
    private const string GZipContentEncoding = "gzip";
    private const string DeflateContentEncoding = "deflate";
    public HttpCompressionModule() { }
    public void Dispose() { }

    public String ModuleName
    {
        get { return "HttpCompressionModule"; }
    }

    // In the Init function, register for HttpApplication 
    // events by adding your handlers.
    public void Init(HttpApplication application)
    {
        application.BeginRequest +=
            (new EventHandler(this.Application_BeginRequest));
    }

    private void Application_BeginRequest(Object source,
         EventArgs e)
    {
        // Create HttpApplication and HttpContext objects to access
        // request and response properties.
        HttpApplication application = (HttpApplication)source;
        HttpContext context = application.Context;

        if (HttpContext.Current.Handler is DefaultHttpHandler)
        {
            return;
        }
        String acceptEncoding = HttpContext.Current.Request.Headers[AcceptEncodingHeader];
        if (string.IsNullOrEmpty(acceptEncoding))
        {
            return;
        }
        HttpResponse response = HttpContext.Current.Response;
        acceptEncoding = acceptEncoding.ToLowerInvariant();
        if (acceptEncoding.Contains(GZipContentEncoding))
        {
            response.AddHeader(ContentEncodingHeader, GZipContentEncoding);
            response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
        }
        else if (acceptEncoding.Contains(DeflateContentEncoding))
        {
            response.AddHeader(ContentEncodingHeader, DeflateContentEncoding);
            response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
        }
    }

}