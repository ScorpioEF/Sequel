namespace Sequel.Infrastructure.Component
{
    public enum ComponentState
    {
        Error,
        MissingDependency,
        MissingAssembly,
        Loadable,
        AlreadyExist,
        Loaded,
    }
}
