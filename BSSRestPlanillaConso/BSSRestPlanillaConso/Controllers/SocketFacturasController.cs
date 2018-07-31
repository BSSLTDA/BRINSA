using BSSRestPlanillaConso.Models;
using Microsoft.Web.WebSockets;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class SocketFacturasController : ApiController
    {
        public HttpResponseMessage Get()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                
                System.Web.HttpContext.Current.AcceptWebSocketRequest(new ProcessWebSocketFacturas());
                return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
