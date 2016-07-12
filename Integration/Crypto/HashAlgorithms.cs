using System.Diagnostics.CodeAnalysis;
#pragma warning disable 1591

namespace Integration.Crypto
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum HashAlgorithms
    {
        SHA,
        SHA1,
        MD5,
        SHA256,
        SHA384,
        SHA512
    }
}