﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace BSSRestPlanillaConso.com.nygsoft.huella {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Servicio ingreso nuevo traficoBinding", Namespace="http://localhost:1001/webservice/")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(datos))]
    public partial class Servicioingresonuevotrafico : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback nuevoTraficoOperationCompleted;
        
        private System.Threading.SendOrPostCallback nuevoSeguimientoOperationCompleted;
        
        private System.Threading.SendOrPostCallback eliminarTraficoOperationCompleted;
        
        private System.Threading.SendOrPostCallback finalizarTraficoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Servicioingresonuevotrafico() {
            this.Url = global::BSSRestPlanillaConso.Properties.Settings.Default.BSSRestPlanillaConso_com_nygsoft_huella_Servicio_ingreso_nuevo_trafico;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event nuevoTraficoCompletedEventHandler nuevoTraficoCompleted;
        
        /// <remarks/>
        public event nuevoSeguimientoCompletedEventHandler nuevoSeguimientoCompleted;
        
        /// <remarks/>
        public event eliminarTraficoCompletedEventHandler eliminarTraficoCompleted;
        
        /// <remarks/>
        public event finalizarTraficoCompletedEventHandler finalizarTraficoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://huella.nygsoft.com/WsTransporte/wsCreacionTraficoBrinsa.php/nuevoTrafico", RequestNamespace="http://localhost:1001/webservice/", ResponseNamespace="http://localhost:1001/webservice/")]
        [return: System.Xml.Serialization.SoapElementAttribute("resultado")]
        public datos[] nuevoTrafico(
                    string placa, 
                    string manifiesto, 
                    string ccConductor, 
                    string NombreConductor, 
                    string celular, 
                    int origen, 
                    string destino, 
                    string observacion, 
                    [System.Xml.Serialization.SoapElementAttribute(DataType="date")] System.DateTime fechaInicio, 
                    [System.Xml.Serialization.SoapElementAttribute(DataType="time")] System.DateTime horaInicio, 
                    string UrlGps, 
                    string usuarioGps, 
                    string ContrasenaGps, 
                    string Usuario, 
                    string Clave, 
                    string item1, 
                    string item2, 
                    string item3, 
                    string generadorCarga, 
                    int estado, 
                    int consolidado, 
                    string planilla) {
            object[] results = this.Invoke("nuevoTrafico", new object[] {
                        placa,
                        manifiesto,
                        ccConductor,
                        NombreConductor,
                        celular,
                        origen,
                        destino,
                        observacion,
                        fechaInicio,
                        horaInicio,
                        UrlGps,
                        usuarioGps,
                        ContrasenaGps,
                        Usuario,
                        Clave,
                        item1,
                        item2,
                        item3,
                        generadorCarga,
                        estado,
                        consolidado,
                        planilla});
            return ((datos[])(results[0]));
        }
        
        /// <remarks/>
        public void nuevoTraficoAsync(
                    string placa, 
                    string manifiesto, 
                    string ccConductor, 
                    string NombreConductor, 
                    string celular, 
                    int origen, 
                    string destino, 
                    string observacion, 
                    System.DateTime fechaInicio, 
                    System.DateTime horaInicio, 
                    string UrlGps, 
                    string usuarioGps, 
                    string ContrasenaGps, 
                    string Usuario, 
                    string Clave, 
                    string item1, 
                    string item2, 
                    string item3, 
                    string generadorCarga, 
                    int estado, 
                    int consolidado, 
                    string planilla) {
            this.nuevoTraficoAsync(placa, manifiesto, ccConductor, NombreConductor, celular, origen, destino, observacion, fechaInicio, horaInicio, UrlGps, usuarioGps, ContrasenaGps, Usuario, Clave, item1, item2, item3, generadorCarga, estado, consolidado, planilla, null);
        }
        
        /// <remarks/>
        public void nuevoTraficoAsync(
                    string placa, 
                    string manifiesto, 
                    string ccConductor, 
                    string NombreConductor, 
                    string celular, 
                    int origen, 
                    string destino, 
                    string observacion, 
                    System.DateTime fechaInicio, 
                    System.DateTime horaInicio, 
                    string UrlGps, 
                    string usuarioGps, 
                    string ContrasenaGps, 
                    string Usuario, 
                    string Clave, 
                    string item1, 
                    string item2, 
                    string item3, 
                    string generadorCarga, 
                    int estado, 
                    int consolidado, 
                    string planilla, 
                    object userState) {
            if ((this.nuevoTraficoOperationCompleted == null)) {
                this.nuevoTraficoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnnuevoTraficoOperationCompleted);
            }
            this.InvokeAsync("nuevoTrafico", new object[] {
                        placa,
                        manifiesto,
                        ccConductor,
                        NombreConductor,
                        celular,
                        origen,
                        destino,
                        observacion,
                        fechaInicio,
                        horaInicio,
                        UrlGps,
                        usuarioGps,
                        ContrasenaGps,
                        Usuario,
                        Clave,
                        item1,
                        item2,
                        item3,
                        generadorCarga,
                        estado,
                        consolidado,
                        planilla}, this.nuevoTraficoOperationCompleted, userState);
        }
        
        private void OnnuevoTraficoOperationCompleted(object arg) {
            if ((this.nuevoTraficoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.nuevoTraficoCompleted(this, new nuevoTraficoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://huella.nygsoft.com/WsTransporte/wsCreacionTraficoBrinsa.php/nuevoSeguimien" +
            "to", RequestNamespace="http://localhost:1001/webservice/", ResponseNamespace="http://localhost:1001/webservice/")]
        [return: System.Xml.Serialization.SoapElementAttribute("resultado")]
        public datos[] nuevoSeguimiento(string placa, string manifiesto, string mensaje, [System.Xml.Serialization.SoapElementAttribute(DataType="date")] System.DateTime fechaMensaje, [System.Xml.Serialization.SoapElementAttribute(DataType="time")] System.DateTime horaMensaje, int novedad, int estadoActual, string Usuario, string Clave, int Consolidado, string Planilla) {
            object[] results = this.Invoke("nuevoSeguimiento", new object[] {
                        placa,
                        manifiesto,
                        mensaje,
                        fechaMensaje,
                        horaMensaje,
                        novedad,
                        estadoActual,
                        Usuario,
                        Clave,
                        Consolidado,
                        Planilla});
            return ((datos[])(results[0]));
        }
        
        /// <remarks/>
        public void nuevoSeguimientoAsync(string placa, string manifiesto, string mensaje, System.DateTime fechaMensaje, System.DateTime horaMensaje, int novedad, int estadoActual, string Usuario, string Clave, int Consolidado, string Planilla) {
            this.nuevoSeguimientoAsync(placa, manifiesto, mensaje, fechaMensaje, horaMensaje, novedad, estadoActual, Usuario, Clave, Consolidado, Planilla, null);
        }
        
        /// <remarks/>
        public void nuevoSeguimientoAsync(string placa, string manifiesto, string mensaje, System.DateTime fechaMensaje, System.DateTime horaMensaje, int novedad, int estadoActual, string Usuario, string Clave, int Consolidado, string Planilla, object userState) {
            if ((this.nuevoSeguimientoOperationCompleted == null)) {
                this.nuevoSeguimientoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnnuevoSeguimientoOperationCompleted);
            }
            this.InvokeAsync("nuevoSeguimiento", new object[] {
                        placa,
                        manifiesto,
                        mensaje,
                        fechaMensaje,
                        horaMensaje,
                        novedad,
                        estadoActual,
                        Usuario,
                        Clave,
                        Consolidado,
                        Planilla}, this.nuevoSeguimientoOperationCompleted, userState);
        }
        
        private void OnnuevoSeguimientoOperationCompleted(object arg) {
            if ((this.nuevoSeguimientoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.nuevoSeguimientoCompleted(this, new nuevoSeguimientoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://huella.nygsoft.com/WsTransporte/wsCreacionTraficoBrinsa.php/eliminarTrafic" +
            "o", RequestNamespace="http://localhost:1001/webservice/", ResponseNamespace="http://localhost:1001/webservice/")]
        [return: System.Xml.Serialization.SoapElementAttribute("resultado")]
        public datos[] eliminarTrafico(string placa, string manifiesto, string Usuario, string Clave) {
            object[] results = this.Invoke("eliminarTrafico", new object[] {
                        placa,
                        manifiesto,
                        Usuario,
                        Clave});
            return ((datos[])(results[0]));
        }
        
        /// <remarks/>
        public void eliminarTraficoAsync(string placa, string manifiesto, string Usuario, string Clave) {
            this.eliminarTraficoAsync(placa, manifiesto, Usuario, Clave, null);
        }
        
        /// <remarks/>
        public void eliminarTraficoAsync(string placa, string manifiesto, string Usuario, string Clave, object userState) {
            if ((this.eliminarTraficoOperationCompleted == null)) {
                this.eliminarTraficoOperationCompleted = new System.Threading.SendOrPostCallback(this.OneliminarTraficoOperationCompleted);
            }
            this.InvokeAsync("eliminarTrafico", new object[] {
                        placa,
                        manifiesto,
                        Usuario,
                        Clave}, this.eliminarTraficoOperationCompleted, userState);
        }
        
        private void OneliminarTraficoOperationCompleted(object arg) {
            if ((this.eliminarTraficoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.eliminarTraficoCompleted(this, new eliminarTraficoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://huella.nygsoft.com/WsTransporte/wsCreacionTraficoBrinsa.php/finalizarTrafi" +
            "co", RequestNamespace="http://localhost:1001/webservice/", ResponseNamespace="http://localhost:1001/webservice/")]
        [return: System.Xml.Serialization.SoapElementAttribute("resultado")]
        public datos[] finalizarTrafico(string placa, string manifiesto, string Usuario, string Clave, int Consolidado) {
            object[] results = this.Invoke("finalizarTrafico", new object[] {
                        placa,
                        manifiesto,
                        Usuario,
                        Clave,
                        Consolidado});
            return ((datos[])(results[0]));
        }
        
        /// <remarks/>
        public void finalizarTraficoAsync(string placa, string manifiesto, string Usuario, string Clave, int Consolidado) {
            this.finalizarTraficoAsync(placa, manifiesto, Usuario, Clave, Consolidado, null);
        }
        
        /// <remarks/>
        public void finalizarTraficoAsync(string placa, string manifiesto, string Usuario, string Clave, int Consolidado, object userState) {
            if ((this.finalizarTraficoOperationCompleted == null)) {
                this.finalizarTraficoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfinalizarTraficoOperationCompleted);
            }
            this.InvokeAsync("finalizarTrafico", new object[] {
                        placa,
                        manifiesto,
                        Usuario,
                        Clave,
                        Consolidado}, this.finalizarTraficoOperationCompleted, userState);
        }
        
        private void OnfinalizarTraficoOperationCompleted(object arg) {
            if ((this.finalizarTraficoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.finalizarTraficoCompleted(this, new finalizarTraficoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://localhost:1001/webservice/")]
    public partial class datos {
        
        private int codigoField;
        
        private string mensajeField;
        
        /// <comentarios/>
        public int codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
            }
        }
        
        /// <comentarios/>
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void nuevoTraficoCompletedEventHandler(object sender, nuevoTraficoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class nuevoTraficoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal nuevoTraficoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public datos[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((datos[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void nuevoSeguimientoCompletedEventHandler(object sender, nuevoSeguimientoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class nuevoSeguimientoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal nuevoSeguimientoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public datos[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((datos[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void eliminarTraficoCompletedEventHandler(object sender, eliminarTraficoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class eliminarTraficoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal eliminarTraficoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public datos[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((datos[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void finalizarTraficoCompletedEventHandler(object sender, finalizarTraficoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class finalizarTraficoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal finalizarTraficoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public datos[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((datos[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591