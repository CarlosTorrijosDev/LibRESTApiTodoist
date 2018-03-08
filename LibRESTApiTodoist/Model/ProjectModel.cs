using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Proyecto de Todoist.
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Nombre del proyecto.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Posición del proyecto en la lista de proyecto/Orden del proyecto.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Valor de 1 a 4 indicando el nivel de indentación del proyecto.
        /// </summary>
        [JsonProperty("indent")]
        public int Indent { get; set; }

        /// <summary>
        /// Número de comentarios del proyecto.
        /// </summary>
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
}