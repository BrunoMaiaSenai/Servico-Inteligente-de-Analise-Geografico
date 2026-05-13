using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;

namespace ServicoInteligenteGeografico.Data
{
    /// <summary>
    /// Classe responsável por criar e manter a conexão com o Firebase.
    /// Toda a aplicação usa esse único ponto de acesso ao banco.
    /// </summary>
    public class Database
    {
        
        private const string FirebaseUrl = "https://servicegeo-b5eb5-default-rtdb.firebaseio.com/";

        // Instância única do cliente (padrão Singleton)
        private static FirebaseClient? _client;

        /// <summary>
        /// Retorna o cliente do Firebase pronto para uso.
        /// Cria a conexão apenas na primeira chamada.
        /// </summary>
        public static FirebaseClient GetClient()
        {
            if (_client == null)
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromFile("serviceAccountKey.json")
                    });
                }

                _client = new FirebaseClient(FirebaseUrl, new FirebaseOptions
                {
                    // If the library supports it, passing the credential directly is cleaner.
                    // Otherwise, using the Admin SDK to get the Access Token is more standard than a Custom Token:
                    AuthTokenAsyncFactory = async () =>
                    {
                        var credential = GoogleCredential.FromFile("serviceAccountKey.json")
                                            .CreateScoped("https://www.googleapis.com/auth/userinfo.email",
                                                          "https://www.googleapis.com/auth/firebase.database");
                        return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                    }
                });
            }
            return _client;
        }
    }
}