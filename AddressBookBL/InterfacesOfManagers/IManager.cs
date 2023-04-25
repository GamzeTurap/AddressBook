using AddressBookEL.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.InterfacesOfManagers
{
    public interface IManager<T, Id>
    {
        IDataResult<T> Add(T model); //Ekleme icin IREsult degil IDataResult kullandik.Cünkü eklenen verinin Id'sine ihtiyac duyarsak geriye donen datadan ID'yi alabiliriz.
        IResult Update(T model);
        IResult Delete(T model);
        IDataResult<ICollection<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        IDataResult<T> GetByConditions(Expression<Func<T, bool>>? filter = null);
        IDataResult<T> GetById(Id id);
    }
}
