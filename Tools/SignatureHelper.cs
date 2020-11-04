using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApplication1.Tools {
    public class SignatureHelper {
		public static string APIKEY_HEADER = "apikey";
	public static string TIMESTAMP_HEADER = "timestamp";
	public static string SIGNATURE_HEADER = "signature";
	public static List<string> SIGNATURE_KEYWORDS = new List<string> { APIKEY_HEADER, TIMESTAMP_HEADER };

		/** 用私钥进行进行签名 */
		public static string createSignature(WebHeaderCollection headers, string url, string privateKey) {
			Dictionary<string, string> sortedHeaders = new Dictionary<string, string>();
			foreach (var key in headers.Keys) {
				if (SIGNATURE_KEYWORDS.Contains(key.ToString())) {
					sortedHeaders.Add(key.ToString(), headers.GetValues(key.ToString())[0]);
				}
			}
			string sortedUrl = createSortedUrl(url, sortedHeaders);
			byte[] privateKeyBytes = Base64.Decode(privateKey);
			var source = UTF8Encoding.UTF8.GetBytes(sortedUrl);
			var dsa = new DSACryptoServiceProvider();
			dsa.FromXmlString(privateKey);
			dsa.CreateSignature(privateKeyBytes);
			var pKey = dsa.ExportParameters(true); //私钥
			var sig = BitConverter.ToString(dsa.SignData(source));

			return sig;
		}


		public static string createSortedUrl(string url, Dictionary<string, string> headersAndParams) {
			string par = "";
			foreach (var key in headersAndParams.Keys) {
				if (par.Length > 0) {
					par += "@";
				}
				par += key + "=" + headersAndParams[key];
			}
			if (!url.EndsWith("?"))
				url += "?";
			//System.out.println(url + params);
			return url + par;
		}
    }
}
