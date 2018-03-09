using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Todoist task.
    /// </summary>
    public class TaskModel
    {
        /// <summary>
        /// Priority type.
        /// </summary>
        public enum PriorityType
        {
            p4 = 1,
            p3,
            p2,
            p1
        }

        /// <summary>
        /// Task identifier.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Project identifier.
        /// </summary>
        [JsonProperty("project_id")]
        public long ProjectID { get; set; }

        /// <summary>
        /// Content of the task.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Task indicator completed.
        /// </summary>
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        /// <summary>
        /// List of labels associated with the task.
        /// </summary>
        [JsonProperty("label_ids")]
        public List<long> LabelIDs { get; set; }

        /// <summary>
        /// Position in the project.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Indentation level of the task from 1 to 5.
        /// </summary>
        [JsonProperty("indent")]
        public int Indent { get; set; }

        /// <summary>
        /// Priority of the task from 1 (normal, default value) to 4 (urgent).
        /// </summary>
        [JsonProperty("priority")]
        public PriorityType Priority { get; set; }

        /// <summary>
        /// Expiration of the task.
        /// </summary>
        [JsonProperty("due")]
        public DueModel Due { get; set; }

        /// <summary>
        /// URL to access the task in the Todoist web interface.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Number of comments of the task.
        /// </summary>
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
}
