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

public class DriverController : BaseApi
{
    public static string currentController = String.Format("{0}", MethodBase.GetCurrentMethod().ReflectedType.Name);

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(Autista))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage login([FromBody] InputLogin input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var driver = Autista.login(input.Username, input.Password);
            if (driver != null && driver.Abilitato)
            {
                if (driver.Note != null)
                    driver.Note.RemoveAll(x => !x.Pubblica);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(driver, jsonSettings));
            }
            else
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Username e/o password non validi!" });
            
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(Autista))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getInfo([FromBody] InputFilter input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.Id))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var driver = Autista.getDetail(input.Id);
            if (driver != null)
            {
                if (driver.Note != null)
                    driver.Note.RemoveAll(x => !x.Pubblica);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; 
                return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(driver, jsonSettings));
            }
            else
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "" });

        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}