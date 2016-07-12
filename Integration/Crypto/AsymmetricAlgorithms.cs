using System.Diagnostics.CodeAnalysis;
#pragma warning disable 1591

namespace Integration.Crypto
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum AsymmetricAlgorithms
    {
        RSA,
        DSA,
        ECDSA
    }
}