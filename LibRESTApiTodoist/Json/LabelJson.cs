using System.Collections.Generic;

namespace LibRESTApiTodoIst.Service
{
    internal class LabelJson : BaseJson
    {
        public string Name { get; }

        public LabelJson(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is LabelJson other &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
