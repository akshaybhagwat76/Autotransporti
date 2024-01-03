using System;
using System.Web;

namespace Utility
{
    public static class Compression
    {
        public static void GZipEncodePage(HttpResponse response)
        {
            
            if (IsGZipSupported())
            {

                string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"].ToString();
                if (acceptEncoding.Contains("deflate"))
                {
                    response.Filter = new System.IO.Compression.DeflateStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.AppendHeader("Content-Encoding", "deflate");
                }
                else
                {
                    response.Filter = new System.IO.Compression.GZipStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.AppendHeader("Content-Encoding", "gzip");
                }
            }

            response.AppendHeader("Vary", "Content-Encoding");
        }


        public static bool IsGZipSupported()
        {

            string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"].ToString();

            if (!String.IsNullOrEmpty(acceptEncoding) && (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
                return true;

            return false;
        }
    }

}