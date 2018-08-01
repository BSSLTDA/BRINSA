
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CLCommon
{
    /// <summary>
    /// ITEMS DEL DOCUMENTO
    /// </summary>
    public class ITE
    {
        /// <summary>
        /// Para Factura: Número consecutivo del Ítem. Ver Nota 14
        /// * Para Notas crédito y débito: Es importante el número de consecutivo inicie en 1 y continue en orden, 
        /// porque este orden consecutivo es requerido para calcular el UUID de la línea como lo especifica la DIAN.
        /// </summary>
        public string ITE_1 { get; set; }
        /// <summary>
        /// true-Indica si el ítem/servicio a facturar es gratis.
        /// Si es una bonificación o son artículos gratis, se debe enviar = true
        /// false-Si es una venta, porse debe enviar = false.
        /// Indicando que no es gratis.
        /// </summary>
        public string ITE_2 { get; set; }
        /// <summary>
        /// Cantidad del artículo solicitado.
        /// Número de unidades servidas/prestadas.
        /// </summary>
        public decimal ITE_3 { get; set; }
        /// <summary>
        /// Unidad de medida de los bienes/servicios vendidos por ítem.
        /// </summary>
        public string ITE_4 { get; set; }
        /// <summary>
        /// Costo Total:  
        /// Cantidad x Precio Unidad.
        /// </summary>
        public decimal ITE_5 { get; set; }
        /// <summary>
        /// Moneda del costo total
        /// </summary>
        public string ITE_6 { get; set; }
        /// <summary>
        /// Precio Unitario. 
        /// Precio unitario de la unidad de bien o servicio servido/prestado, en la moneda indicada en la Cabecera de la Factura.Siempre sin Impuestos
        /// </summary>
        public decimal ITE_7 { get; set; }
        /// <summary>
        /// Moneda del precio unitario.
        /// </summary>
        public string ITE_8 { get; set; }
        /// <summary>
        /// Identificador de artículo.
        /// Código identificador del artículo, por ejemplo el GTIN(Global Trade Ítem Number)., EAN, UPC, CUM, CUP.
        /// </summary>
        public string ITE_9 { get; set; }
        public string ITE_10 { get; set; }
        /// <summary>
        /// Descripción
        /// Descripción del artículo que pertenece a la línea de la facturas
        /// </summary>
        public string ITE_11 { get; set; }
        public string ITE_12 { get; set; }
        /// <summary>
        /// Cantidad de unidades de empaque.
        /// </summary>
        public string ITE_13 { get; set; }
        /// <summary>
        /// Unidad de medida de los empaques.
        /// Se debe enviar si envía la cantidad de unidades de empaque
        /// </summary>
        public string ITE_14 { get; set; }
        /// <summary>
        /// Número de elementos en un empaque
        /// </summary>
        public string ITE_15 { get; set; }
        public string ITE_16 { get; set; }
        public string ITE_17 { get; set; }
        public string ITE_18 { get; set; }
        /// <summary>
        /// Total del ítem (incluyendo Descuentos y cargos).
        /// </summary>
        public decimal ITE_19 { get; set; }
        /// <summary>
        /// Moneda del Total del ítem (incluyendo Descuentos y cargos)
        /// Obligatorio si se envía el valor total del ítem
        /// </summary>
        public string ITE_20 { get; set; }
        public string ITE_21 { get; set; }
        public string ITE_22 { get; set; }
        public string ITE_23 { get; set; }
        public string ITE_24 { get; set; }
        public string ITE_25 { get; set; }
        public string ITE_26 { get; set; }
        public List<MdlLOT> LMdlLOT { get; set; }
        public List<MdlIRF> LMdlIRF { get; set; }
        public List<MdlMYM> LMdlMYM { get; set; }
        public List<MdlIPA> LMdlIPA { get; set; }
        public List<MdlDFI> LMdlDFI { get; set; }
        public List<MdlIDE> LMdlIDE { get; set; }
        [XmlElement("TII", typeof(TII))]
        public List<TII> LMdlTII { get; set; }
        public List<MdlILE> LMdlILE { get; set; }
        public List<MdlPSI> LMdlPSI { get; set; }
        public List<MdlDIT> LMdlDIT { get; set; }
    }
}
