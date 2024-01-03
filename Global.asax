<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Net.Http.Formatting" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        GlobalConfiguration.Configuration.Formatters.Clear();
        GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

        RegisterRoutes(RouteTable.Routes);
        SwaggerConfig.Register();
    }

    void RegisterRoutes(RouteCollection routes)
    {
        routes.MapHttpRoute(
            name: "APIService",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = System.Web.Http.RouteParameter.Optional, action = "list" }
        );
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        HttpApplication app = sender as HttpApplication;
        if (!(app.Context.CurrentHandler is Page || app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
        app.Request["HTTP_X_MICROSOFTAJAX"] != null)
            return;

        if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("ckeditor"))
            return;

        Utility.Compression.GZipEncodePage(app.Response);
    }


    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError();
        Console.WriteLine(ex.Message);
    }

    void Session_Start(object sender, EventArgs e)
    {
        Session.Timeout = 60;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
