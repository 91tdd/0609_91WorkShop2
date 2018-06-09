using System.Collections.Generic;

namespace WorkShop2.Tests
{
    public interface IRepository<T>
    {
        List<Budget> GetBudgets();
    }
}