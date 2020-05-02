using Microsoft.Azure.Cosmos;

namespace App.Data
{
    public interface ICosmosService
    {
        Database GetDatabase();
    }
}