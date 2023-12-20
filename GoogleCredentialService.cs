using Google.Apis.Auth.OAuth2;

namespace BloggerIndexing
{
    public class GoogleCredentialService
    {
        public async Task<string> GetAccessTokenWithJsonPrivateKey()
        {
            var privateKeyStream = File.OpenRead(@"C:\Users\Hamid\Desktop\hamidmosalla-258eabd6f142.json");

            var serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(privateKeyStream);

            var googleCredetial = GoogleCredential.FromServiceAccountCredential(serviceAccountCredential).CreateScoped(new[] { "https://www.googleapis.com/auth/indexing" });

            var result = await googleCredetial.UnderlyingCredential.GetAccessTokenForRequestAsync("https://www.googleapis.com/auth/indexing");

            return result;
        }

        public GoogleCredential GetGoogleCredential()
        {
            Console.WriteLine("Please enter service account file name");
            string file = Console.ReadLine();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "serviceaccount", file);

            GoogleCredential credential;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[] { "https://www.googleapis.com/auth/indexing" });
            }

            return credential;
        }
    }
}
