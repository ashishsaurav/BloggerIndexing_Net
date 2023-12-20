using Google.Apis.Auth.OAuth2;
using Google.Apis.Indexing.v3;
using Google.Apis.Indexing.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;

namespace BloggerIndexing
{
    public class GoogleBatchIndexingService
    {
        private GoogleCredentialService _googleCredentialService;
        private GoogleCredential _googleCredential;

        public GoogleBatchIndexingService()
        {
            _googleCredentialService = new GoogleCredentialService();
            _googleCredential = _googleCredentialService.GetGoogleCredential();
        }

        public async Task<List<PublishUrlNotificationResponse>> BatchAddOrUpdateGoogleIndex(IEnumerable<string> jobUrls)
        {
            return await BatchAddUpdateIndex(jobUrls, "URL_UPDATED");
        }

        private async Task<List<PublishUrlNotificationResponse>> BatchAddUpdateIndex(IEnumerable<string> jobUrls, string action)
        {
            var credential = _googleCredential.UnderlyingCredential;

            var googleIndexingApiClientService = new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            var request = new BatchRequest(googleIndexingApiClientService);

            var notificationResponses = new List<PublishUrlNotificationResponse>();

            foreach (var url in jobUrls)
            {
                var urlNotification = new UrlNotification
                {
                    Url = url,
                    Type = action
                };

                request.Queue<PublishUrlNotificationResponse>(
                    new UrlNotificationsResource.PublishRequest(googleIndexingApiClientService, urlNotification), (response, error, i, message) =>
                    {
                        notificationResponses.Add(response);
                    });
            }

            await request.ExecuteAsync();

            return await Task.FromResult(notificationResponses);
        }
    }
}
