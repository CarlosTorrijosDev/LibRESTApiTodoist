using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Todoist label.
    /// </summary>
    public class LabelModel
    {
        /// <summary>
        /// Label identifier.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Name of the label.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Number used by customers to order the list of labels.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}