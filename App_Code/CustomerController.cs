using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
using API;
using Newtonsoft.Json;
using System.Web.Hosting;

public class CustomerController : BaseApi
{
    public static string currentController = String.Format("{0}", MethodBase.GetCurrentMethod().ReflectedType.Name);

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(List<Cliente>))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getList()
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            var source = Cliente.getList();
            return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(source));

        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(StringResponse))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage uploadDocument([FromBody] InputDDT input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.CustomerId) || string.IsNullOrEmpty(input.DriverId) || string.IsNullOrEmpty(input.File))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var customer = Cliente.getDetail(input.CustomerId);
            if (customer == null)
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Cliente non trovato!" });

            if (customer.DDT == null)
                customer.DDT = new List<DDT>();

            string filename = string.Format("{0}.{1}", DateTime.Now.ToString("ddMMyyyyHHmmss"), input.FileExt);
            System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + "/media/" + filename, Convert.FromBase64String(input.File));

            customer.DDT.Add(new DDT()
            {
                Data = DateTime.Now.ToUniversalTime(),
                IdAutista = input.DriverId,
                File = filename
            });
            customer.insertOrUpdate();

            return setResponseMessage(HttpStatusCode.OK, new StringResponse() { Code = 200, Message = "" });
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(List<DDT>))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getDriverDocuments([FromBody] InputFilter input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.Id))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var customer = Cliente.getList().Where(x => x.DDT != null && x.DDT.Any(d => d.IdAutista == input.Id)).ToList();
            foreach (var c in customer)
                c.DDT.RemoveAll(x => x.IdAutista != input.Id);

            var source = customer.SelectMany(x => x.DDT);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; 
            return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(source, jsonSettings));
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}