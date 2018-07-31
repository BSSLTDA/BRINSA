using Microsoft.Web.WebSockets;
using System;
using System.Threading;
using System.Web.Script.Serialization;

namespace BSSRestPlanillaConso.Models
{
    public class ProcessWebSocketFacturas : WebSocketHandler
    {
        private static WebSocketCollection _procClients = new WebSocketCollection();
        //public string Pid { get; set; }
        public ProcessWebSocketFacturas()
        {
        }

        public override void OnClose()
        {
            //var strUsers = string.Empty;
            _procClients.Remove(this);
            //foreach (ProcessWebSocketFacturas handler in _procClients)
            //{
            //    strUsers += (strUsers == string.Empty ? "" : ",") + handler.Pid;
            //}
            //_procClients.Broadcast("user:" + strUsers);
        }

        public override void OnOpen()
        {
            if (!_procClients.Contains(this))
            {
                //this.Pid = this.WebSocketContext.QueryString["Pid"];
                _procClients.Add(this);
            }
            //var strUsers = string.Empty;
            //foreach (ProcessWebSocketFacturas handler in _procClients)
            //{
            //    strUsers += (strUsers == string.Empty ? "" : ",") + handler.Pid;
            //}
            //_procClients.Broadcast("user:" + strUsers);
            //foreach (ProcessWebSocketFacturas handler in _procClients)
            //{
            //    handler.Send("key:" + handler.WebSocketContext.SecWebSocketKey);
            //}
        }

        public override void OnMessage(string jsonData)
        {
            var javaScriptSer = new JavaScriptSerializer();
            ClientMessage value = (ClientMessage)javaScriptSer.Deserialize(jsonData, typeof(ClientMessage));
            string strBody = javaScriptSer.Serialize(value);

            foreach (ProcessWebSocketFacturas handler in _procClients)
            {
                handler.Send("Procesando...");
                Transactions trans = new Transactions();
                trans.SendToClient += SendToClientMessage;
                trans.StartProcess(handler, value.Tarea);
                trans.SendToClient -= SendToClientMessage;
                var envia = (RespuestaSometer400)javaScriptSer.Deserialize(System.Web.HttpContext.Current.Application["APIBRINSA_" + value.Tarea].ToString(), typeof(RespuestaSometer400));
                handler.Send(envia.TAPIRESULT);
            }
        }

        private void SendToClientMessage(object sender, DemoEventArgs e)
        {
            e.Handler.Send(e.Message);
        }
    }

    public class ClientMessage
    {
        public string Tarea { get; set; }
        public string ProcessType { get; set; }
    }

    public class Transactions
    {
        //IRFTASKRepository dRFTASK = new RFTASKRepository();
        //RFTASK mRFTASK = new RFTASK();

        public event EventHandler<DemoEventArgs> SendToClient;
        protected virtual void OnSendToClient(DemoEventArgs e)
        {
            this.SendToClient?.Invoke(this, e);
        }
        public void StartProcess(ProcessWebSocketFacturas handler, string tarea)
        {
            Thread.Sleep(1000);
            for (int i = 0; i < 2; i++)
            {
                if (System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea] != null)
                {
                    var proc = System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea];
                    System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea] = null;
                    i = 2;
                    DemoEventArgs e = new DemoEventArgs()
                    {
                        Handler = handler,
                        Message = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + " => " + tarea + " FACTURAS " + proc.ToString() + "."
                    };
                    this.OnSendToClient(e);
                }
                else
                {
                    i = 0;
                    DemoEventArgs e = new DemoEventArgs()
                    {
                        Handler = handler,
                        //DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss fffffff")
                        Message = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + " => " + tarea + " Procesando FACTURAS..., "
                    };
                    this.OnSendToClient(e);
                    Thread.Sleep(2000);
                }                    
            }
        }
    }

    public class DemoEventArgs
    {
        public ProcessWebSocketFacturas Handler { get; set; }
        public string Session { get; set; }
        public string Message { get; set; }
    }
}