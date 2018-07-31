using System.Collections.Generic;
using System.Xml.Serialization;

namespace CLCommon
{
    /// <summary>
    /// INFORMACIÓN DEL ADQUIRIENTE
    /// </summary>
    public class ADQ
    {
        /// <summary>
        /// Tipo de identificación - Tipos de Persona 
        /// </summary>
        public string ADQ_1 { get; set; }
        /// <summary>
        /// Número de documento de identificación. 
        /// Identificador fiscal: Número completo de Identificación Tributaria o similar.En Colombia se debe utilizar el NIT.
        /// </summary>
        public string ADQ_2 { get; set; }
        /// <summary>
        /// Tipo de documento de identificación. 
        /// </summary>
        public string ADQ_3 { get; set; }
        /// <summary>
        /// Régimen: Régimen al que pertenece. 
        /// 0  ->Simplificado 
        /// 2  ->Común
        /// Importante: Esta campo se usa solo para la representación gráfica.
        /// Los tipos de responsabilidades del emisor se deben enviar en el registro TCR(INFORMACIÓN TRIBUTARIA, ADUANERA Y CAMBIARIA - ADQUIRIENTE )
        /// </summary>
        public string ADQ_4 { get; set; }
        public string ADQ_5 { get; set; }
        /// <summary>
        /// Razón social de la empresa.  
        /// Obligatorio en caso de ser una persona jurídica.
        /// Nombre del comerciante en la Cámara de Comercio o equivalente.
        /// Si ""Tipo de identificación - Tipos de Persona"" es ""1"" ""Persona Jurídica"" o 3 ""Gran Contribuyente"" (ADQ+1); entonces este campo es obligatorio.
        /// </summary>
        public string ADQ_6 { get; set; }
        public string ADQ_7 { get; set; }
        /// <summary>
        /// Nombre del adquiriente. 
        /// Obligatorio si es una persona natural.Nombre completo del participante.
        /// Si ""Tipo de identificación - Tipos de Persona"" es ""2"" ""Persona Natural"" (ADQ+1); entonces este campo es obligatorio.
        /// </summary>
        public string ADQ_8 { get; set; }
        /// <summary>
        /// Primer y segundo Apellido. 
        /// Obligatorio si es una persona natural.
        /// Si ""Tipo de identificación - Tipos de Persona"" es ""2"" ""Persona Natural"" (ADQ+1); entonces este campo es obligatorio.
        /// </summary>
        public string ADQ_9 { get; set; }
        /// <summary>
        /// Dirección en texto libre.
        /// Texto Libre para establecer una dirección.
        /// Ubicación del punto de venta || localización || guía para llegar.
        /// </summary>
        public string ADQ_10 { get; set; }
        public string ADQ_11 { get; set; }
        public string ADQ_12 { get; set; }
        public string ADQ_13 { get; set; }
        public string ADQ_14 { get; set; }
        /// <summary>
        /// Código País
        /// </summary>
        public string ADQ_15 { get; set; }
        public string ADQ_16 { get; set; }
        public string ADQ_17 { get; set; }
        /// <summary>
        /// Dirección postal en la Ciudad | Distrito
        /// </summary>
        public string ADQ_18 { get; set; }
        public string ADQ_19 { get; set; }
        public string ADQ_20 { get; set; }
        public string ADQ_21 { get; set; }
        [XmlElement("TCR", typeof(TCR))]
        public List<TCR> LMdlTCR { get; set; }
        [XmlElement("ICR", typeof(ICR))]
        public List<ICR> LMdlICR { get; set; }
        [XmlElement("CDA", typeof(CDA))]
        public List<CDA> LMdlCDA { get; set; }

    }
}
