using LibRESTApiTodoIst.Service;
using System.Collections.Generic;

namespace LibRESTApiTodoist.Json
{
    internal class CommentJsonForProject : BaseJson
    {
        public long Project_id { get; }
        public string Content { get; }

        public CommentJsonForProject(long project_id, string content)
        {
            Project_id = project_id;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return obj is CommentJsonForProject other &&
                   Project_id == other.Project_id &&
                   Content == other.Content;
        }

        public override int GetHashCode()
        {
            int hashCode = -184039824;
            hashCode = hashCode * -1521134295 + Project_id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            return hashCode;
        }
    }
}
