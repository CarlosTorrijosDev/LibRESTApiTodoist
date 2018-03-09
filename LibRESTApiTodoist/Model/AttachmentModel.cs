using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Attached to a comment by Todoist.
    /// </summary>
    public class AttachmentModel
    {
        /// <summary>
        /// File name.
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        /// <summary>
        /// MIME type of the file.
        /// </summary>
        [JsonProperty("file_type")]
        public string FileType { get; set; }

        /// <summary>
        /// URL of the file.
        /// </summary>
        [JsonProperty("file_url")]
        public string FileUrl { get; set; }

        /// <summary>
        /// Upload status of the file.
        /// </summary>
        [JsonProperty("upload_state")]
        public string UploadState { get; set; }
    }
}