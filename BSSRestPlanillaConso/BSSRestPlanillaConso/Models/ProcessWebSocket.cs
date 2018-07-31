using Microsoft.Web.WebSockets;
using System;
using System.Threading;
using System.Web.Script.Serialization;

namespace BSSRestPlanillaConso.Models
{
    public class ProcessWebSocket : WebSocketHandler
    {
        private static WebSocketCollection _procClients = new WebSocketCollection();
        //public string Pid { get; set; }
        RespuestaSometer400 proc;

        public ProcessWebSocket()
        {
        }

        public override void OnClose()
        {
            //var strPid = string.Empty;
            _procClients.Remove(this);
            //foreach (ProcessWebSocketPagos handler in _procClients)
            //{
            //    strPid += (strPid == string.Empty ? "" : ",") + handler.Pid;
            //}
            //_procClients.Broadcast("pid:" + strPid);
        }

        public override void OnOpen()
        {
            try
            {
                if (!_procClients.Contains(this))
                {
                    //this.Pid = this.WebSocketContext.QueryString["Pid"];
                    _procClients.Add(this);
                }
            }
            catch (Exception ex)
            {
                foreach (ProcessWebSocket handler in _procClients)
                {
                    handler.Send("ERROR: " + ex.Message);
                }
            }

            //var strPid = string.Empty;
            //foreach (ProcessWebSocketPagos handler in _procClients)
            //{
            //    strPid += (strPid == string.Empty ? "" : ",") + handler.Pid;
            //}
            //_procClients.Broadcast("pid:" + strPid);
            //foreach (ProcessWebSocketPagos handler in _procClients)
            //{
            //    handler.Send("key:" + handler.WebSocketContext.SecWebSocketKey);
            //}
        }

        public override void OnMessage(string jsonData)
        {
            var javaScriptSer = new JavaScriptSerializer();
            ClientMessage value = (ClientMessage)javaScriptSer.Deserialize(jsonData, typeof(ClientMessage));
            string strBody = javaScriptSer.Serialize(value);

            foreach (ProcessWebSocket handler in _procClients)
            {
                handler.Send("Procesando...");
                TransactionsP trans = new TransactionsP();
                trans.SendToClient += SendToClientMessage;
                trans.StartProcess(handler, value.Tarea, ref proc);
                trans.SendToClient -= SendToClientMessage;
                //RespuestaSometer400 s = (RespuestaSometer400)System.Web.HttpContext.Current.Application["APIBRINSA_" + value.Tarea];

                //RespuestaSometer400 proc = (RespuestaSometer400)System.Web.HttpContext.Current.Application["APIBRINSA_" + value.Tarea];                
                if (proc != null)
                {
                    handler.Send(proc.TAPIRESULT + "," + proc.TKEY + "," + proc.TKEYWORD);
                    OnClose();
                }
                
            }
        }

        private void SendToClientMessage(object sender, DemoEventArgsP e)
        {
            e.Handler.Send(e.Message);
        }
    }

    public class TransactionsP
    {
        //IRFTASKRepository dRFTASK = new RFTASKRepository();
        //RFTASK mRFTASK = new RFTASK();

        public event EventHandler<DemoEventArgsP> SendToClient;
        protected virtual void OnSendToClient(DemoEventArgsP e)
        {
            this.SendToClient?.Invoke(this, e);
        }
        public void StartProcess(ProcessWebSocket handler, string tarea, ref  RespuestaSometer400 pro)
        {
            Thread.Sleep(1000);

            for (int i = 0; i < 2; i++)
            {
                if (System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea] != null)
                {
                    pro = (RespuestaSometer400)System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea];
                    //System.Web.HttpContext.Current.Application["APIBRINSA_" + tarea] = null;
                    i = 2;
                    DemoEventArgsP e = new DemoEventArgsP()
                    {
                        Handler = handler,
                        Message = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + " => " + tarea + " " + pro.TKEYWORD + " " + pro.TAPIRESULT + "."
                    };
                    this.OnSendToClient(e);
                    break;
                }
                else
                {
                    i = 0;
                    DemoEventArgsP e = new DemoEventArgsP()
                    {
                        Handler = handler,
                        //DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss fffffff")
                        Message = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + " => " + tarea + " Procesando ..., "
                    };
                    this.OnSendToClient(e);
                    Thread.Sleep(2000);
                }
            }
        }
    }

    public class DemoEventArgsP
    {
        public ProcessWebSocket Handler { get; set; }
        public string Session { get; set; }
        public string Message { get; set; }
    }
}