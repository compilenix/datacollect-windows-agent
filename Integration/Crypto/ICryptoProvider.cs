using System.IO;

namespace Integration.Crypto
{
    /// <summary>
    /// This privides a simplefied interface to sign and verify data using asymectric cryptography.
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Initializes and returns a new instance of the corresponding CryptoServiceProvider class (for example: <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" />).
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns>a instance of the corresponding CryptoServiceProvider class</returns>
        object GetHashAlgorithm(HashAlgorithms algorithm);

        /// <summary>
        /// Sign stuff, using a given <see cref="Stream"/> as input.
        /// </summary>
        /// <param name="dataToBeSigned"></param>
        /// <returns>signature</returns>
        byte[] Sign(Stream dataToBeSigned);

        /// <summary>
        /// Sign stuff.
        /// </summary>
        /// <param name="dataToBeSigned"></param>
        /// <returns>signature</returns>
        byte[] Sign(byte[] dataToBeSigned);

        /// <summary>
        /// Verify a signature, using a given <see cref="Stream"/> as input.
        /// </summary>
        /// <param name="dataToBeVerifyed"></param>
        /// <param name="signature"></param>
        /// <returns>if the data can be considered genuine</returns>
        bool Verify(Stream dataToBeVerifyed, byte[] signature);

        /// <summary>
        /// Verify a signature.
        /// </summary>
        /// <param name="dataToBeVerifyed"></param>
        /// <param name="signature"></param>
        /// <returns>if the data can be considered genuine</returns>
        bool Verify(byte[] dataToBeVerifyed, byte[] signature);
    }
}