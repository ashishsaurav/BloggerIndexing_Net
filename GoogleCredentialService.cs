using Google.Apis.Auth.OAuth2;

namespace BloggerIndexing
{
    public class GoogleCredentialService
    {
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
