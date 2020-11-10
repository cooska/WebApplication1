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
			SortedDictionary<string, string> sortedHeaders = new SortedDictionary<string, string>();
			foreach (string key in headers.Keys) {
				if (SIGNATURE_KEYWORDS.Contains(key)) {
					sortedHeaders[key] = headers[key];
				}
			}
			string sortedUrl = createSortedUrl(url, sortedHeaders);
			byte[] privateKeyBytes = Convert.FromBase64String(privateKey);
			var source = UTF8Encoding.UTF8.GetBytes(sortedUrl);
			var dsa = new DSACryptoServiceProvider();
			dsa.FromXmlString(privateKey);
			dsa.CreateSignature(privateKeyBytes);
			var pKey = dsa.ExportParameters(true); //私钥
			var sig = BitConverter.ToString(dsa.SignData(source));

			return sig;
		}


		public static string createSortedUrl(string url, SortedDictionary<string, string> headersAndParams) {
			string @params = "";
			foreach (string key in headersAndParams.Keys) {
				if (@params.Length > 0) {
					@params += "@";
				}
				@params += key + "=" + headersAndParams[key].ToString();
			}
			if (!url.EndsWith("?", StringComparison.Ordinal)) {
				url += "?";
			}
			Console.WriteLine(url + @params);
			return url + @params;

		}
	}
}
