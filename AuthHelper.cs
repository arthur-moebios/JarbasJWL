using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace JarbasJWL
{
	internal class CodeChallengeHelper
	{
        private static object ContosoHandler;

        static internal async Task<string> ReadResponseTextForResponseAsync(WebResponse authorizationResponse)
		{
			string authorizationResponseText;
			using (Stream authorizationResponseStream = authorizationResponse.GetResponseStream())
			{
				UTF8Encoding textEncoderWithoutBOM = new UTF8Encoding(false);
				long responseLengthLong = authorizationResponse.ContentLength;
				if (0 <= responseLengthLong)
				{
					int responseRemainder = (int)responseLengthLong;
					byte[] responseBytes = new byte[responseRemainder];
					int responseIndex = 0;
					int responseBytesRead;
					do
					{
						responseBytesRead = await authorizationResponseStream.ReadAsync(responseBytes, responseIndex, responseRemainder).ConfigureAwait(false);
						responseIndex += responseBytesRead;
						responseRemainder -= responseBytesRead;
					} while (responseBytesRead != 0);
					/* UNDONE:  observe response charset. */
					authorizationResponseText = textEncoderWithoutBOM.GetString(responseBytes);
				}
				else
				{
					int responseRemainder = 4096;
					byte[] responseBytes = new byte[responseRemainder];
					int responseIndex = 0;
					int responseBytesRead;
					do
					{
						responseBytesRead = await authorizationResponseStream.ReadAsync(responseBytes, responseIndex, responseRemainder).ConfigureAwait(false);
						responseIndex += responseBytesRead;
						responseRemainder -= responseBytesRead;
						if (responseIndex == responseBytes.Length)
						{
							Array.Resize<byte>(ref responseBytes, responseIndex + 4096);
							responseRemainder += 4096;
						}
						if (responseRemainder < 1)
						{
							break;
						}
					} while (responseBytesRead != 0);
					Array.Resize<byte>(ref responseBytes, responseIndex);
					/* UNDONE:  observe response charset. */
					authorizationResponseText = textEncoderWithoutBOM.GetString(responseBytes);
				}
			}
			return authorizationResponseText;
		}	

		static internal string GetAccessTokenFromAuthorizationResponse(string authorizationResponseText, out int accessTokenLifetimeSeconds)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			Dictionary<string, object> authorizationResponseObject = serializer.Deserialize<Dictionary<string, object>>(authorizationResponseText);
			string accessToken = (string)authorizationResponseObject["access_token"];
			accessTokenLifetimeSeconds = (int)authorizationResponseObject["expires_in"];
			return accessToken;
		}
	}
}
