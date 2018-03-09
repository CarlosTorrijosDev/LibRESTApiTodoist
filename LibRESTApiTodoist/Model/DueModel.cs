using Newtonsoft.Json;

namespace LibRESTApiTodoIst.Model
{
    /// <summary>
    /// Expiration of a Todoist task.
    /// </summary>
    public class DueModel
    {
        /// <summary>
        /// Date defined in a human way in arbitrary format.
        /// </summary>
        [JsonProperty("string")]
        public string String { get; set; }

        /// <summary>
        /// Date in YYYY-MM-DD format of the user's time zone.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Date and time in RFC3339 format of the UTC area.
        /// It is only returned if an expiration time has been set (eg if it is not a task that takes up the entire day).
        /// </summary>
        [JsonProperty("datetime")]
        public string Datetime { get; set; }

        /// <summary>
        /// User's time zone, either in tzdata compatible format ("Europe / Berlin"), or a string specifying East or UTC difference as "UTC ± HH: MM" (eg "UTC-01: 00").
        /// It is only returned if an expiration time has been set.
        /// </summary>
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}
