using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CLCommon
{
    public class RFFACDET
    {
        [Display(Name = "Prefijo")]        
        public string DPREFIJ { get; set; }
        [Display(Name = "Factura")]
        public string DFACTUR { get; set; }
        [Display(Name = "Linea")]
        public string DLINEA { get; set; }
        [Display(Name = "Pedido")]
        public string DPEDIDO { get; set; }
        [Display(Name = "Linea Pedido")]
        public string DLINPED { get; set; }
        [Display(Name = "Ordenamiento")]
        public string DORDENL { get; set; }
        [Display(Name = "Consolidado")]
        public string DCONSOL { get; set; }
        [Display(Name = "Codigo Producto")]
        public string DCODPRO { get; set; }
        [Display(Name = "Descripcion Producto")]
        public string DDESPRO { get; set; }
        [Display(Name = "Unidad Medida")]
        public string DUNIMED { get; set; }
        [Display(Name = "Porc de Impue")]
        public string DPORIMP { get; set; }
        [Display(Name = "Cantidad")]
        public decimal DCANTID { get; set; }
        [Display(Name = "Unidades")]
        public string DUNIDAD { get; set; }
        [Display(Name = "Valor Unitario")]
        public decimal DVALUNI { get; set; }
        [Display(Name = "Valor Total")]
        public decimal DVALTOT { get; set; }
        [Display(Name = "Porc de Impue")]
        public string DFPORIMP { get; set; }
        [Display(Name = "Cantidad")]
        public string DFCANTID { get; set; }
        [Display(Name = "Cantidad")]
        public string DFUNIDAD { get; set; }
        [Display(Name = "Valor Unitario")]
        public string DFVALUNI { get; set; }
        [Display(Name = "Valor Total")]
        public decimal DFVALTOT { get; set; }
        [Display(Name = "Notas Linea Pedido")]
        public string DNOTLIN { get; set; }
        [Display(Name = "IALPDF")]
        public string DFIALPDF { get; set; }
        [Display(Name = "IAL")]
        public string DFIAL { get; set; }
        [Display(Name = "Flag 01")]
        public string DFLAG01 { get; set; }
        [Display(Name = "Flag 02")]
        public string DFLAG02 { get; set; }
        [Display(Name = "Flag 03")]
        public string DFLAG03 { get; set; }
        [Display(Name = "Flag 04")]
        public string DFLAG04 { get; set; }
        [Display(Name = "Flag 05")]
        public string DFLAG05 { get; set; }
        
    }

    public interface IRFFACDETRepository
    {
        List<RFFACDET> Find(string Prefijo, string Factura);
    }
}
