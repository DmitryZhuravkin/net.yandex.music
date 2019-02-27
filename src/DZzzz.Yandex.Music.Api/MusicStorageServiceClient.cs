using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Configuration;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Yandex.Music.Api.Model;

namespace DZzzz.Yandex.Music.Api
{
    public class MusicStorageServiceClient : XmlHttpServiceClient
    {
        public MusicStorageServiceClient(
            HttpServiceClientConfiguration configuration,
            IHttpClientFactory httpClientFactory)
            : base(configuration, httpClientFactory)
        {
        }

        public async Task<string> GetTrackDonwloadUriAsync(string trackStorageDir)
        {
            string url = $"/download-info/{trackStorageDir}/2.mp3";

            DownloadInfo info = await SendRequestWithResultAsync<DownloadInfo>(url, HttpMethod.Get).ConfigureAwait(false);

            return BuildFileUri(info);
        }

        private string BuildFileUri(DownloadInfo info)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                string temp = $"XGRlBW9FXlekgbPrRHuSiA{info.Path.Substring(1)}{info.S}";

                byte[] keyBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(temp));

                StringBuilder sBuilder = new StringBuilder();
                foreach (var t in keyBytes)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                string key = sBuilder.ToString();

                return $"http://{info.Host}/get-mp3/{key}/{info.Ts}{info.Path}";
            }
        }
    }
}