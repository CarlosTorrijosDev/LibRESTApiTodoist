using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Tarea de Todoist.
    /// </summary>
    public class TaskModel
    {
        /// <summary>
        /// Tipo de prioridad.
        /// </summary>
        public enum PriorityType
        {
            p4 = 1,
            p3,
            p2,
            p1
        }

        /// <summary>
        /// Identificador de la tarea.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        [JsonProperty("project_id")]
        public long ProjectID { get; set; }

        /// <summary>
        /// Contenido de la tarea.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Indicador de tarea completada.
        /// </summary>
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        /// <summary>
        /// Lista de etiquetas asociadas a la tarea.
        /// </summary>
        [JsonProperty("label_ids")]
        public List<long> LabelIDs { get; set; }

        /// <summary>
        /// Posición en el proyecto.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Nivel de indentación de la tarea de 1 a 5.
        /// </summary>
        [JsonProperty("indent")]
        public int Indent { get; set; }

        /// <summary>
        /// Prioridad de la tarea de 1 (normal, valor por defecto) a 4 (urgente).
        /// </summary>
        [JsonProperty("priority")]
        public PriorityType Priority { get; set; }

        /// <summary>
        /// Vencimiento de la tarea.
        /// </summary>
        [JsonProperty("due")]
        public DueModel Due { get; set; }

        /// <summary>
        /// URL para acceder a la tarea en la interfaz web de Todoist.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Número de comentarios de la tarea.
        /// </summary>
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
}
