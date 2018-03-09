using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Todoist project.
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// Project identifier.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Project's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Project position in the project list / Project order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Value from 1 to 4 indicating the indentation level of the project.
        /// </summary>
        [JsonProperty("indent")]
        public int Indent { get; set; }

        /// <summary>
        /// Number of comments on the project.
        /// </summary>
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
}