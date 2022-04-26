using System.Collections.Generic;

namespace LibRESTApiTodoIst.Service
{
    internal class CommentJsonForTask : BaseJson
    {
        public long Task_id { get; }
        public string Content { get; }

        public CommentJsonForTask(long task_id, string content)
        {
            Task_id = task_id;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return obj is CommentJsonForTask other &&
                   Task_id == other.Task_id &&
                   Content == other.Content;
        }

        public override int GetHashCode()
        {
            int hashCode = 97428334;
            hashCode = hashCode * -1521134295 + Task_id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            return hashCode;
        }
    }
}
