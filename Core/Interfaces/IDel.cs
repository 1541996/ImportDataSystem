using System;

namespace Core.Interfaces
{
    public interface IDel<T>
    {
        Nullable<bool> IsDeleted { get; set; }

    }
}
