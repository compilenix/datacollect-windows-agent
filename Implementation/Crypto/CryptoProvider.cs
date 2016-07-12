using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Integration.Crypto;

namespace Implementation.Crypto
{
    /// <summary>
    /// This privides a simplefied interface to sign and verify data using asymectric cryptography.
    /// </summary>
    public class CryptoProvider : ICryptoProvider, IDisposable
    {
        private readonly AsymmetricAlgorithms _asymmetricAlgorithm;
        private readonly X509Certificate2 _certificate;
        private readonly HashAlgorithms _hashAlgorithm;

        /// <param name="certificatePath">path to the certificate file</param>
        /// <param name="asymmetricAlgorithm"><see cref="AsymmetricAlgorithms"/></param>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithms"/></param>
        /// <param name="password"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public CryptoProvider(string certificatePath, string asymmetricAlgorithm, string hashAlgorithm, string password = "")
        {
            if (!File.Exists(certificatePath)) throw new FileNotFoundException(nameof(certificatePath) + " could not be found", certificatePath);

            _certificate = new X509Certificate2(certificatePath, password, X509KeyStorageFlags.PersistKeySet);
            Enum.TryParse(hashAlgorithm, true, out _hashAlgorithm);
            Enum.TryParse(asymmetricAlgorithm, true, out _asymmetricAlgorithm);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // ReSharper disable once UseNullPropagation
            if (disposing && _certificate != null)
            {
                _certificate.Dispose();
            }
        }

        /// <summary>
        /// Sign stuff, using a given <see cref="Stream"/> as input.
        /// </summary>
        /// <param name="dataToBeSigned"></param>
        /// <returns>signature</returns>
        public byte[] Sign(Stream dataToBeSigned)
        {
            if (dataToBeSigned?.Length < 1) throw new IndexOutOfRangeException(nameof(dataToBeSigned) + " length must be greater than zero");

            byte[] signature;

            switch (_asymmetricAlgorithm)
            {
                case AsymmetricAlgorithms.DSA:
                    signature = _certificate.GetECDsaPrivateKey().SignData(dataToBeSigned, new HashAlgorithmName(_hashAlgorithm.ToString()));
                    break;
                case AsymmetricAlgorithms.ECDSA:
                    signature = _certificate.GetECDsaPrivateKey().SignData(dataToBeSigned, new HashAlgorithmName(_hashAlgorithm.ToString()));
                    break;
                case AsymmetricAlgorithms.RSA:
                    signature = ((RSACryptoServiceProvider)_certificate.PrivateKey).SignData(dataToBeSigned, GetHashAlgorithm(_hashAlgorithm));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return signature;
        }

        /// <summary>
        /// Sign stuff.
        /// </summary>
        /// <param name="dataToBeSigned"></param>
        /// <returns>signature</returns>
        public byte[] Sign(byte[] dataToBeSigned)
        {
            return Sign(new MemoryStream(dataToBeSigned, false));
        }

        /// <summary>
        /// Verify a signature, using a given <see cref="Stream"/> as input.
        /// </summary>
        /// <param name="dataToBeVerifyed"></param>
        /// <param name="signature"></param>
        /// <returns>if the data can be considered genuine</returns>
        public bool Verify(Stream dataToBeVerifyed, byte[] signature)
        {
            if (dataToBeVerifyed?.Length < 1) throw new IndexOutOfRangeException(nameof(dataToBeVerifyed) + " length must be greater than zero");

            bool isValid;

            switch (_asymmetricAlgorithm)
            {
                case AsymmetricAlgorithms.DSA:
                    isValid = _certificate.GetECDsaPublicKey().VerifyData(dataToBeVerifyed, signature, new HashAlgorithmName(_hashAlgorithm.ToString()));
                    break;
                case AsymmetricAlgorithms.ECDSA:
                    isValid = _certificate.GetECDsaPublicKey().VerifyData(dataToBeVerifyed, signature, new HashAlgorithmName(_hashAlgorithm.ToString()));
                    break;
                case AsymmetricAlgorithms.RSA:
                    try
                    {
                        isValid = _certificate.GetRSAPublicKey().VerifyData(dataToBeVerifyed, signature, new HashAlgorithmName(_hashAlgorithm.ToString()), RSASignaturePadding.Pkcs1);
                    }
                    catch
                    {
                        isValid = _certificate.GetRSAPublicKey().VerifyData(dataToBeVerifyed, signature, new HashAlgorithmName(_hashAlgorithm.ToString()), RSASignaturePadding.Pss);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return isValid;
        }

        /// <summary>
        /// Verify a signature.
        /// </summary>
        /// <param name="dataToBeVerifyed"></param>
        /// <param name="signature"></param>
        /// <returns>if the data can be considered genuine</returns>
        public bool Verify(byte[] dataToBeVerifyed, byte[] signature)
        {
            return Verify(new MemoryStream(dataToBeVerifyed, false), signature);
        }

        /// <summary>
        /// Initializes and returns a new instance of the corresponding CryptoServiceProvider class (for example: <see cref="RSACryptoServiceProvider" />).
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns>a instance of the corresponding CryptoServiceProvider class</returns>
        public object GetHashAlgorithm(HashAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case HashAlgorithms.MD5:
                    return new MD5CryptoServiceProvider();
                case HashAlgorithms.SHA:
                case HashAlgorithms.SHA1:
                    return new SHA1CryptoServiceProvider();
                case HashAlgorithms.SHA256:
                    return new SHA256CryptoServiceProvider();
                case HashAlgorithms.SHA384:
                    return new SHA384CryptoServiceProvider();
                case HashAlgorithms.SHA512:
                    return new SHA256CryptoServiceProvider();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}