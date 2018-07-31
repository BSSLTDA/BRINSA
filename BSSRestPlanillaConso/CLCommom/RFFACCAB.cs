using System;
using System.ComponentModel.DataAnnotations;

namespace CLCommon
{
    public class RFFACCAB
    {        
        [Display(Name = "Prefijo")]        
        public string FPREFIJ { get; set; }
        [Display(Name = "Factura")]
        public string FFACTUR { get; set; }
        [Display(Name = "Fecha Factura")]
        public string FFECFAC { get; set; }
        [Display(Name = "Fecha Vencimiento")]
        public string FFECVEN { get; set; }
        [Display(Name = "Fecha Requerido")]
        public string FFECREQ { get; set; }
        [Display(Name = "Pedido")]
        public string FPEDIDO { get; set; }
        [Display(Name = "Planilla")]
        public string FPLANIL { get; set; }
        [Display(Name = "Consolidado")]
        public string FCONSOL { get; set; }
        [Display(Name = "Bodega")]
        public string FBODEGA { get; set; }
        [Display(Name = "Terminos de Pago")]
        public string FTERMIN { get; set; }
        [Display(Name = "Orden de Compra")]
        public string FORDCOM { get; set; }
        [Display(Name = "Moneda")]
        public string FMONEDA { get; set; }
        [Display(Name = "Cliente")]
        public string FCLIENT { get; set; }
        [Display(Name = "Nombre Cliente")]
        public string FNOMCLI { get; set; }
        [Display(Name = "Direccion Cliente1")]
        public string FDIRCLI1 { get; set; }
        [Display(Name = "Direccion Cliente2")]
        public string FDIRCLI2 { get; set; }
        [Display(Name = "Direccion Cliente3")]
        public string FDIRCLI3 { get; set; }
        [Display(Name = "Departameto Cliente")]
        public string FDEPCLI { get; set; }
        [Display(Name = "Pais Cliente")]
        public string FPAICLI { get; set; }
        [Display(Name = "Nit")]
        public string FNIT { get; set; }
        [Display(Name = "Idioma I/ingles")]
        public string FIDIOMA { get; set; }
        [Display(Name = "Mercado")]
        public string FMERCADO { get; set; }
        [Display(Name = "Punto Envio")]
        public string FPTOEN { get; set; }
        [Display(Name = "Nombre Punto Envio")]
        public string FNOMPEN { get; set; }
        [Display(Name = "Direccion PuntoEn1")]
        public string FDIRPEN1 { get; set; }
        [Display(Name = "Direccion PuntoEn2")]
        public string FDIRPEN2 { get; set; }
        [Display(Name = "Direccion PuntoEn3")]
        public string FDIRPEN3 { get; set; }
        [Display(Name = "Departameto PuntEnv")]
        public string FDEPPEN { get; set; }
        [Display(Name = "Pais PuntEnv")]
        public string FPAIPEN { get; set; }
        [Display(Name = "Telefono PtoEnv")]
        public string FTELPEN { get; set; }
        [Display(Name = "Subtotal")]
        public decimal FSUBTOT { get; set; }
        [Display(Name = "Impuestos")]
        public decimal FIMPUES { get; set; }
        [Display(Name = "Descuento")]
        public string FDESCUE { get; set; }
        [Display(Name = "Total Factura")]
        public decimal FTOTFAC { get; set; }
        [Display(Name = "Total Cajas")]
        public string FTOTCAJ { get; set; }
        [Display(Name = "Total Kilos")]
        public string FTOTKIL { get; set; }
        [Display(Name = "Total Unida")]
        public string FTOTUNI { get; set; }
        [Display(Name = "Subtotal Alf")]
        public string FFSUBTOT { get; set; }
        [Display(Name = "Impuestos Alf")]
        public string FFIMPUES { get; set; }
        [Display(Name = "Descuento Alf")]
        public string FFDESCUE { get; set; }
        [Display(Name = "Total Factura Alf")]
        public string FFTOTFAC { get; set; }
        [Display(Name = "Total Cajas Alf")]
        public string FFTOTCAJ { get; set; }
        [Display(Name = "Total Kilos Alf")]
        public string FFTOTKIL { get; set; }
        [Display(Name = "Total Unida Alf")]
        public string FFTOTUNI { get; set; }
        [Display(Name = "Notas Pedidos")]
        public string FNOTPED { get; set; }
        [Display(Name = "Notas Cliente")]
        public string FNOTCLI { get; set; }
        [Display(Name = "Notas Punto Envio")]
        public string FNOTPEN { get; set; }
        [Display(Name = "Factura")]
        public string FAPCHK01 { get; set; }
        [Display(Name = "Copia Factura")]
        public string FAPCHK02 { get; set; }
        [Display(Name = "Fecha")]
        public DateTime FAFECHA { get; set; }
        [Display(Name = "Campo 01")]
        public string FAREF01 { get; set; }
        [Display(Name = "Campo 02")]
        public string FAREF02 { get; set; }
        [Display(Name = "Campo 03")]
        public string FAREF03 { get; set; }
        [Display(Name = "Campo 04")]
        public string FAREF04 { get; set; }
        [Display(Name = "Campo 05")]
        public string FAREF05 { get; set; }
        [Display(Name = "Flag 01")]
        public string FAFLAG01 { get; set; }
        [Display(Name = "Flag 02")]
        public string FAFLAG02 { get; set; }
        [Display(Name = "Aviso Despacho")]
        public string FAFLAG03 { get; set; }
        [Display(Name = "Flag 04")]
        public string FAFLAG04 { get; set; }
        [Display(Name = "Flag 05")]
        public string FAFLAG05 { get; set; }
        [Display(Name = "Creado")]
        public DateTime FACRTDAT { get; set; }
        [Display(Name = "Usuario")]
        public string FACRTUSR { get; set; }
        public string TotLineas { get; set; }
        public string CTAX { get; set; }
        public string SUFD05 { get; set; }
        public string SUFD06 { get; set; }
        public string SUFD13 { get; set; }
        public string SUFD14 { get; set; }
        public string SUFD17 { get; set; }
        public string SUFD19 { get; set; }
        public string CPHON { get; set; }
        public string CMAD6 { get; set; }
        public string CCON { get; set; }
    }

    public interface IRFFACCABRepository
    {
        RFFACCAB Find(string Prefijo, string Factura);
        string UpdFacturaIdNme(string Prefijo, string Factura, string Id, string Nme);
        string UpdFacturaResPDF(string Prefijo, string Factura, string Resultado);
        RFFACCAB GetFlag3(string Prefijo, string Factura);
        RFFACCAB GetCCNUM(string Prefijo, string Factura);
        RFFACCAB GetCCNUMCode(string Prefijo, string Factura);
        string UpdFlag3(string Prefijo, string Factura);

    }
}
