using System;

namespace Sequel.Infrastructure.Component
{
    public interface IComponent
    {
        Type[] Dependencies { get; }

        bool Load();
    }
}
