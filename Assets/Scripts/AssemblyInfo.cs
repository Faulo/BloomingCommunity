using System.Runtime.CompilerServices;
using BloomingCommunity.Runtime;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_EDITMODE)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_PLAYMODE)]

namespace BloomingCommunity.Runtime {
    static class AssemblyInfo {
        public const string NAMESPACE_RUNTIME = "BloomingCommunity.Runtime";
        public const string NAMESPACE_EDITOR = "BloomingCommunity.Editor";

        public const string NAMESPACE_TESTS_PLAYMODE = "BloomingCommunity.Tests.PlayMode";
        public const string NAMESPACE_TESTS_EDITMODE = "BloomingCommunity.Tests.EditMode";

        public const string NAMESPACE_PROXYGEN = "DynamicProxyGenAssembly2";
    }
}