namespace CHUSHKA.Services.Contracts
{
    using System.Collections.Generic;

    public interface IProductService
    {
        string Create(string name, decimal price, string description, string type);

        T Details<T>(string id);

        ICollection<T> GetAll<T>();

        bool Order(string id, string username);

        bool Edit(string id, string name, decimal price, string description, string type);

        void Delete(string id);

    }
}
