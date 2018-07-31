using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CLCommon
{
    /// <summary>
    /// INFORMACIÓN DEL EMISOR ELECTRONICO  DEL DOCUMENTO
    /// </summary>
    public class EMI
    {
        /// <summary>
        /// Tipo de identificación - Tipos de Persona
        /// 1-Persona Jurídica
        /// 2-Persona Natural
        /// 3-Gran Contribuyente
        /// </summary>
        public string EMI_1 { get; set; }
        /// <summary>
        /// Número de documento de identificación. 
        /// Identificador fiscal: Número completo de Identificación Tributaria o similar.En Colombia se debe utilizar el NIT.
        /// </summary>
        public string EMI_2 { get; set; }
        /// <summary>
        /// Tipo de documento de identificación. En Colombia se debe utilizar el NIT
        /// </summary>
        public string EMI_3 { get; set; }
        /// <summary>
        /// Régimen: Régimen al que pertenece. 
        /// 0  ->Simplificado 
        /// 2  ->Común
        /// Importante: Esta campo se usa solo para la representación gráfica.Los tipos de responsabilidades del emisor se deben enviar en el registro TAC(INFORMACIÓN TRIBUTARIA, ADUANERA Y CAMBIARIA - EMISOR )
        /// </summary>
        public string EMI_4 { get; set; }
        public string EMI_5 { get; set; }
        /// <summary>
        /// Razón social de la empresa.  
        /// Obligatorio en caso de ser una persona jurídica.
        /// Nombre del comerciante en la Cámara de Comercio o equivalente.
        /// Si ""Tipo de identificación - Tipos de Persona"" es ""1"" ""Persona Jurídica"" o 3 ""Gran Contribuyente"" (EMI_1); entonces este campo es obligatorio."
        /// </summary>
        public string EMI_6 { get; set; }
        public string EMI_7 { get; set; }
        public string EMI_8 { get; set; }
        public string EMI_9 { get; set; }
        /// <summary>
        /// Dirección en texto libre.
        /// Texto Libre para establecer una dirección.
        /// Ubicación del punto de venta || localización || guía para llegar.
        /// </summary>
        public string EMI_10 { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string EMI_11 { get; set; }
        /// <summary>
        /// Municipio (distrito administrativo)
        /// Barrio || Localidad || Sección || Zona de la ciudad
        /// </summary>
        public string EMI_12 { get; set; }
        /// <summary>
        /// Ciudad || Municipio
        /// </summary>
        public string EMI_13 { get; set; }
        public string EMI_14 { get; set; }
        /// <summary>
        /// Código País
        /// </summary>
        public string EMI_15 { get; set; }
        public string EMI_16 { get; set; }
        public string EMI_17 { get; set; }
        /// <summary>
        /// Dirección postal en la Ciudad | Distrito
        /// </summary>
        public string EMI_18 { get; set; }
        public string EMI_19 { get; set; }
        public string EMI_20 { get; set; }
        public string EMI_21 { get; set; }
        [XmlElement("TAC", typeof(TAC))]
        public List<TAC> LMdlTAC { get; set; }
        [XmlElement("ICC", typeof(ICC))]
        public List<ICC> LMdlICC { get; set; }
        public List<MdlCDE> LMdlCDE { get; set; }
        public List<MdlBFE> LMdlBFE { get; set; }
    }
}
