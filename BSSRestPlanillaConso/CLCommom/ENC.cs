
using System;
using System.Xml.Serialization;

namespace CLCommon
{
    
    /// <summary>
    /// ENCABEZADO
    /// </summary>    
   
    public class ENC
    {
        /// <summary>
        /// Identificador del tipo de documento.
        /// Se debe enviar según corresponda el tipo de documento:
        /// INVOIC  ->Factura
        /// ND      ->Nota Débito
        /// NC      ->Nota Crédito
        /// </summary>        
        public string ENC_1 { get; set; }
        /// <summary>
        /// Identificación del obligado a facturar electrónico - NIT.(NIT BRINSA)
        /// </summary>
        public string ENC_2 { get; set; }
        /// <summary>
        /// Identificación del adquiriente - NIT. (RFFACCAB-FNIT)
        /// </summary>
        public string ENC_3 { get; set; }
        /// <summary>
        /// Versión del esquema UBL (UBL 2.0)
        /// </summary>
        public string ENC_4 { get; set; }
        /// <summary>
        /// Versión del formato del documento (DIAN 1.0)
        /// </summary>
        public string ENC_5 { get; set; }
        /// <summary>
        /// Número de documento.
        /// Número de factura o factura cambiaria. Incluye prefijo + consecutivo de factura. No se permiten caracteres adicionales como espacios o guiones.
        /// (RFFACCAB-FPREFIJ + RFFACCAB-FFACTUR)
        /// </summary>
        public string ENC_6 { get; set; }
        /// <summary>
        /// Fecha de emisión de la factura. Formato AAAA-MM-DD (RFFACCAB-FFECFAC)
        /// </summary>
        public string ENC_7 { get; set; }
        /// <summary>
        /// Hora de emisión de la factura. Formato: HH:MM:SS hora militar
        /// </summary>
        public string ENC_8 { get; set; }
        /// <summary>
        /// Tipo de factura. Obligatorio solo para Factura, no aplica para Notas
        /// 1-FACTURA DE VENTA
        /// 2-FACTURA DE EXPORTACIÓN
        /// 9-NOTA DEBITO o NOTA CREDITO
        /// </summary>
        public string ENC_9 { get; set; }
        /// <summary>
        /// Divisa consolidada aplicable a toda la factura.
        /// Obligatorio para Factura y Nota Débito.
        /// Opcional para Nota Crédito. (RFFACCAB-FMONEDA)
        /// </summary>
        public string ENC_10 { get; set; }
        public string ENC_11 { get; set; }
        public string ENC_12 { get; set; }
        /// <summary>
        /// Código de contabilidad del comprador aplicado a toda la factura.
        /// Código del cliente en LX (RFFACCAB-FCLIENT)
        /// </summary>
        public string ENC_13 { get; set; }
        public string ENC_14 { get; set; }
        /// <summary>
        /// Número total de ítems o líneas en el documento.
        /// </summary>
        public string ENC_15 { get; set; }
        /// <summary>
        /// Fecha de vencimiento. Formato AAAA-MM-DD(RFFACCAB-FFECVEN)
        /// </summary>
        public string ENC_16 { get; set; }
        public string ENC_17 { get; set; }
        public string ENC_18 { get; set; }
    }
}
