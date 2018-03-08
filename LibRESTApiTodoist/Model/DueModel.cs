using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Vencimiento de una tarea de Todoist.
    /// </summary>
    public class DueModel
    {
        /// <summary>
        /// Fecha definida de manera humana en formato arbitrario.
        /// </summary>
        [JsonProperty("string")]
        public string String { get; set; }

        /// <summary>
        /// Fecha en formato YYYY-MM-DD de la zona horaria del usuario.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Fecha y hora en formato RFC3339 de la zona UTC.
        /// Sólo se devuelve si se ha establecido una hora de vencimiento (p.ej. si no se trata de una tarea que ocupa todo el día).
        /// </summary>
        [JsonProperty("datetime")]
        public string Datetime { get; set; }

        /// <summary>
        /// Zona horaria del usuario, ya sea en formato compatible con tzdata ("Europe/Berlin"), o una cadena especificando Este o diferencia UTC como “UTC±HH:MM” (p.ej. “UTC-01:00”).
        /// Sólo se devuelve si se ha establecido una hora de vencimiento.
        /// </summary>
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}
