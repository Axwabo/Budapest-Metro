// ReSharper disable once CheckNamespace

namespace System.Diagnostics.CodeAnalysis
{

    public sealed class MemberNotNullWhenAttribute : Attribute
    {

        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            ReturnValue = returnValue;
            Members = members;
        }

        public MemberNotNullWhenAttribute(bool returnValue, string member)
        {
            ReturnValue = returnValue;
            Members = new[] {member};
        }

        public bool ReturnValue { get; }

        public string[] Members { get; }

    }

}
