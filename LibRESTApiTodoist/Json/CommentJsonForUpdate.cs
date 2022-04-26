using System.Collections.Generic;

namespace LibRESTApiTodoIst.Service
{
    internal class CommentJsonForUpdate : BaseJson
    {
        public string Content { get; }

        public CommentJsonForUpdate(string content)
        {
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return obj is CommentJsonForUpdate other &&
                   Content == other.Content;
        }

        public override int GetHashCode()
        {
            return 1997410482 + EqualityComparer<string>.Default.GetHashCode(Content);
        }
    }
}
