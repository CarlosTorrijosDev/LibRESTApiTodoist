using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Adjunto de un comentario de Todoist.
    /// </summary>
    public class AttachmentModel
    {
        /// <summary>
        /// Nombre del fichero.
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// Tamaño del fichero.
        /// </summary>
        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        /// <summary>
        /// Tipo MIME del fichero.
        /// </summary>
        [JsonProperty("file_type")]
        public string FileType { get; set; }

        /// <summary>
        /// URL del fichero.
        /// </summary>
        [JsonProperty("file_url")]
        public string FileUrl { get; set; }

        /// <summary>
        /// Estado de subida del fichero.
        /// </summary>
        [JsonProperty("upload_state")]
        public string UploadState { get; set; }
    }
}