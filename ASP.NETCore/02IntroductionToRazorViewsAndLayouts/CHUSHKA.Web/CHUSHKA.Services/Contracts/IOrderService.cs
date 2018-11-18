namespace CHUSHKA.Services.Contracts
{
    using System.Collections.Generic;

    public interface IOrderService
    {
        ICollection<T> All<T>();
    }
}
