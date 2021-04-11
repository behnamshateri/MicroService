using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Order.Core.Entities.Base
{
    public abstract class EntityBase<TType> : IEntityBase<TType>
    {
        [Key]
        public virtual TType Id { get; protected set; }

        private int? _requestedHashCode;
        
        // Transient means that object can be either saved in DB (this state is called persistent) or not saved (this state is called transient)
        public bool IsTransient()
        {
            return Id.Equals(default(TType));
        }
        
        // Dar entity 2 no equality check mishe, Reference equality & Identifier equality
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (EntityBase<TType>)obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }

            return item == this;
        }
        
        // Override "==" operator
        public static bool operator == (EntityBase<TType> a, EntityBase<TType> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        // Override "!=" operator
        public static bool operator != (EntityBase<TType> a, EntityBase<TType> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
                }

                return _requestedHashCode.Value;
            }
            
            return base.GetHashCode();
        }

    }
}
