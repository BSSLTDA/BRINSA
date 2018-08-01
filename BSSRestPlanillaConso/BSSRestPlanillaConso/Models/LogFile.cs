using System;
using System.IO;
using System.Text;

namespace BSSRestPlanillaConso.Models
{
    /// <summary>
    /// Guarda Logs
    /// </summary>
    public class LogFile
    {
        private StreamWriter sw;
        private StringBuilder sb;

        /// <summary>
        /// directory exists
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="create">if set to <c>true</c> [create].</param>
        /// <returns>
        /// System.Boolean
        /// </returns>
        public static bool DirectoryExists(string name, bool create = false)
        {
            if (!Directory.Exists(name))
            {
                if (create)
                    Directory.CreateDirectory(name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Guarda archivo en carpeta especificada
        /// </summary>
        /// <param name="Contenido">Contenido del archivo</param>
        /// <param name="NmeArchivo">Nombre con el que se guardará el archivo</param>
        /// <param name="LogPath">Ruta donde se guardará el archivo</param>
        /// <param name="Extension">Extensión del archivo</param>
        /// <remarks>Autor: JASC Fecha: 2018-06-06</remarks>
        public void CapturaLog(string Contenido, string NmeArchivo, string LogPath, string Extension)
        {
            DirectoryExists(LogPath, true);
            sb = new StringBuilder();
            sw = new StreamWriter(LogPath + "\\" + NmeArchivo + "." + Extension);            
            sb.AppendLine(Contenido);            
            sw.WriteLine(sb.ToString());
            sw.Close();
        }
    }
}