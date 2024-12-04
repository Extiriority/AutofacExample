using System.Collections.Generic;

namespace AutofacDiagnostics
{
    public class Registrations
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public Lifetime Lifetime { get; private set; }
        public string Scope { get; private set; }
        public string ParentScope { get; private set; }
        public List<string> Implementations { get; private set; }
        
        public Registrations(string name, string type, Lifetime lifetime, string scope, string parentScope, List<string> implementations)
        {
            Name = name;
            Type = type;
            Lifetime = lifetime;
            Scope = scope;
            ParentScope = parentScope;
            Implementations = implementations;
        } 
    }
}