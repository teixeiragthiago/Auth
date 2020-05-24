using Auth.SharedKernel;
using Auth.SharedKernel.Secret;

namespace Auth.Infra
{
    public class DbContext
    {
        public readonly MongoDb mongoDb;

        public DbContext()
        {
            mongoDb = Config();
        }

        private MongoDb Config()
        {
            return new MongoDb(Encryption.Decrypt(Secret.connectionString), "auth_project");       
        }
    }
}