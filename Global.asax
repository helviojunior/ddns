<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        RegisterRoutes(RouteTable.Routes);
    }

    void RegisterRoutes(RouteCollection routes)
    {
        string[] allowedMethods = { "GET", "POST" };
        HttpMethodConstraint methodConstraints = new HttpMethodConstraint(allowedMethods);
        RouteValueDictionary vd = new RouteValueDictionary { { "httpMethod", methodConstraints } };

        routes.MapPageRoute("Update", "update/{host_id}/{auth_key}/", "~/update.aspx", true, new RouteValueDictionary { { "host_id", "" }, { "auth_key", "" }}, vd);
    }
       
</script>
