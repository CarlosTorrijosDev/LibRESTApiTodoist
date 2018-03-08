using Newtonsoft.Json;
using System;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Comentario de Todoist.
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// Identificador de comentario.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Identificador de tarea (para los comentarios de tarea).
        /// </summary>
        [JsonProperty("task_id")]
        public long? TaskID { get; set; }

        /// <summary>
        /// Identificador de proyecto (para los comentarios de proyecto).
        /// </summary>
        [JsonProperty("project_id")]
        public long? ProjectID { get; set; }

        /// <summary>
        /// Fecha y hora de inserción del comentario, en formato RFC3339 en UTC.
        /// </summary>
        [JsonProperty("posted")]
        public string Posted { get; set; }

        /// <summary>
        /// Contenido del comentario.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Fichero adjunto.
        /// </summary>
        [JsonProperty("attachment")]
        public AttachmentModel Attachment { get; set; }
    }
}