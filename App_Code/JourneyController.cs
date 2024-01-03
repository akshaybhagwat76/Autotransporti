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
using System.Globalization;
using Utility;

public class JourneyController : BaseApi
{
    public static string currentController = String.Format("{0}", MethodBase.GetCurrentMethod().ReflectedType.Name);

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(List<Manutenzione>))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getMaintenance([FromBody] InputFilter input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.PlateCode))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var result = new List<Manutenzione>();
            var plate = Targa.getList(input.PlateCode).FirstOrDefault();
            if (plate != null)
            {
                result.AddRange(Manutenzione.getList(plate.KmTotali));
                if (plate.Assi != null)
                    foreach (var axis in plate.Assi)
                        foreach (var tyre in axis.Pneumatici)
                            result.AddRange(Manutenzione.getList(tyre.KmTotali));
            }

            result = result.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(result));
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(List<Targa>))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getPlates()
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            var source = Targa.getList(null, true).ToList();
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
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(Viaggio))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage startJourney([FromBody] InputJourney input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.DriverId) || string.IsNullOrEmpty(input.PlateCode))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");
            var driver = Autista.getDetail(input.DriverId);
            var item = new Viaggio()
            {
                IdAutista = input.DriverId,
                Inizio = DateTime.Now.ToUniversalTime(),
                Targa = input.PlateCode,
                KmInizialiTarga = input.PlateKm,
                TargaRimorchio = input.TrailerCode,
                KmInizialiRimorchio = input.TrailerKm,
                Giornate = new List<GiornataLavorativa>() { new GiornataLavorativa()
                {
                    Data = DateTime.Now,
                    Tipo = TipoGiornata.Partenza,
                    Tariffa = driver.Tariffe.First(x => x.Tipo == TipoGiornata.Partenza).Valore
                } }
            };
            
            item.insertOrUpdate();

            //*************************************************
            //Aggiornamento automatico dei km delle targhe associate e degli pneumatici attivi
            var mainPlate = Targa.getList(input.PlateCode).FirstOrDefault();
            var addingKm = input.PlateKm - mainPlate.KmTotali;
            if (mainPlate != null)
                mainPlate.updateTotalKM(addingKm);
            if (!string.IsNullOrEmpty(input.TrailerCode))
            {
                var secondaryPlate = Targa.getList(input.TrailerCode).FirstOrDefault();
                if (secondaryPlate != null)
                {
                    addingKm = input.TrailerKm - secondaryPlate.KmTotali;
                    secondaryPlate.updateTotalKM(addingKm);
                }
            }

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(item, jsonSettings));
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
    [ApiKeyAuth]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(Viaggio))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage getJourneyDetail([FromBody] InputFilter input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.Id))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var item = Viaggio.getDetail(input.Id);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return setResponseMessage(HttpStatusCode.OK, JsonConvert.SerializeObject(item, jsonSettings));
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
    public HttpResponseMessage addRefuel([FromBody] InputRefuel input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.JourneyId) || input.Km <= 0 || input.Lt <= 0)
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var journey = Viaggio.getDetail(input.JourneyId);
            if (journey == null)
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Impossibile trovare dati viaggio!" });

            var refuel = new Rifornimento()
            {
                Data = DateTime.Now.ToUniversalTime(),
                Km = input.Km,
                CostoCarburante = input.Cost,
                LtCarburante = input.Lt
            };
            if (journey.Rifornimenti == null)
                journey.Rifornimenti = new List<Rifornimento>();
            journey.Rifornimenti.Add(refuel);
            journey.insertOrUpdate();

            //*************************************************
            //Aggiornamento automatico dei km delle targhe associate e degli pneumatici attivi
            var addingKm = journey.Rifornimenti.Count == 1 ? input.Km - journey.KmInizialiTarga : input.Km - journey.Rifornimenti[journey.Rifornimenti.Count - 2].Km;
            var mainPlate = Targa.getList(journey.Targa).FirstOrDefault();
            if (mainPlate != null)
                mainPlate.updateTotalKM(addingKm);
            if (!string.IsNullOrEmpty(journey.TargaRimorchio))
            {
                var secondaryPlate = Targa.getList(journey.TargaRimorchio).FirstOrDefault();
                if (secondaryPlate != null)
                    secondaryPlate.updateTotalKM(addingKm);
            }

            return setResponseMessage(HttpStatusCode.OK, new StringResponse() { Code = 200, Message = ""});
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
    public HttpResponseMessage addWorkday([FromBody] InputWorkday input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.JourneyId) || string.IsNullOrEmpty(input.WorkdayType))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var journey = Viaggio.getDetail(input.JourneyId);
            if (journey == null)
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Impossibile trovare dati viaggio!" });

            var driver = Autista.getDetail(journey.IdAutista);
            var fee = driver.Tariffe.FirstOrDefault(x => x.Tipo == input.WorkdayType);
            if (fee == null)
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Tipologia giornata lavorativa non valida!" });

            if (journey.Giornate == null)
                journey.Giornate = new List<GiornataLavorativa>();
            journey.Giornate.Add(new GiornataLavorativa()
            {
                Data = DateTime.Now.ToUniversalTime(),
                Tariffa = fee.Valore,
                Tipo = input.WorkdayType
            });
            journey.insertOrUpdate();

            return setResponseMessage(HttpStatusCode.OK, new StringResponse() { Code = 200, Message = "" });
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [SwaggerResponse(HttpStatusCode.OK, Description = "Procedura terminata con successo", Type = typeof(StringResponse))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "API key mancante o non valida")]
    [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Dati input mancanti o errati")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Errore generico del server")]
    public HttpResponseMessage finishJourney([FromBody] InputFilter input)
    {
        try
        {
            if (!isApiKeyValid())
                return setResponseMessage(HttpStatusCode.Unauthorized, "");

            if (input == null || string.IsNullOrEmpty(input.Id))
                return setResponseMessage(HttpStatusCode.BadRequest, "Formato dati input non validi");

            var journey = Viaggio.getDetail(input.Id);
            if (journey == null)
                return setResponseMessage(HttpStatusCode.BadRequest, new StringResponse() { Code = 400, Message = "Impossibile trovare dati viaggio!" });

            var driver = Autista.getDetail(journey.IdAutista);
            
            journey.Fine = DateTime.Now.ToUniversalTime();
            if (journey.Giornate == null)
                journey.Giornate = new List<GiornataLavorativa>();

            //*******************************************************
            //Viene inserita l'ultima giornata lavorativa, attribuita come rientro nel caso in cui avvenga entro le ore 13
            var lastdayType = DateTime.Now.Hour <= 13 ? TipoGiornata.Rientro : TipoGiornata.Feriale;
            journey.Giornate.Add(new GiornataLavorativa()
            {
                Data = DateTime.Now.ToUniversalTime(),
                Tipo = lastdayType,
                Tariffa = driver.Tariffe.First(x => x.Tipo == lastdayType).Valore
            });

            //*******************************************************
            //Controllo ed inserimento di tutte le giornate lavorative feriali non ancora conteggiate
            var standardFee = driver.Tariffe.First(x => x.Tipo == TipoGiornata.Feriale).Valore;
            var date = journey.Inizio.Date;
            while(date <= journey.Fine.Value.Date)
            {
                var f = journey.Giornate.FirstOrDefault(x => x.Data.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"));
                if (f == null)
                    journey.Giornate.Add(new GiornataLavorativa()
                    {
                        Data = date.ToUniversalTime(),
                        Tipo = TipoGiornata.Feriale,
                        Tariffa = standardFee
                    });

                date = date.AddDays(1);
            }
            journey.insertOrUpdate();

            return setResponseMessage(HttpStatusCode.OK, new StringResponse() { Code = 200, Message = "" });
        }
        catch (Exception ex)
        {
            APILogs.insert(new APILogs() { EventType = "ERROR", Message = ex.ToString(), Controller = currentController, Method = MethodBase.GetCurrentMethod().Name });
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}