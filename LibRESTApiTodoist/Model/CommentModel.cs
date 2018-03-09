using Newtonsoft.Json;
using System;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Comment from Todoist.
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// Comment identifier.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Task identifier (for task comments).
        /// </summary>
        [JsonProperty("task_id")]
        public long? TaskID { get; set; }

        /// <summary>
        /// Project identifier (for project comments).
        /// </summary>
        [JsonProperty("project_id")]
        public long? ProjectID { get; set; }

        /// <summary>
        /// Date and time of insertion of the comment, in RFC3339 format in UTC.
        /// </summary>
        [JsonProperty("posted")]
        public string Posted { get; set; }

        /// <summary>
        /// Content of the comment.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Attached document.
        /// </summary>
        [JsonProperty("attachment")]
        public AttachmentModel Attachment { get; set; }
    }
}