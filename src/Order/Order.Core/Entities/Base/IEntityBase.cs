using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Core.Entities.Base
{
    public interface IEntityBase<Type>
    {
        Type Id { get; }
    }
}
