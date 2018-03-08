using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Etiqueta de Todoist.
    /// </summary>
    public class LabelModel
    {
        /// <summary>
        /// Identificador de la etiqueta.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Nombre de la etiqueta.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Número usado por los clientes para ordenar la lista de etiquetas.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}