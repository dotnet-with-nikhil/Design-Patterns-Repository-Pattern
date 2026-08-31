namespace DotNet8_Enterprise_CRUD.Services;

public interface ITransientService { Guid Id { get; } }
public interface IScopedService { Guid Id { get; } }
public interface ISingletonService { Guid Id { get; } }
public class TransientService : ITransientService { public Guid Id { get; } = Guid.NewGuid(); }
public class ScopedService : IScopedService { public Guid Id { get; } = Guid.NewGuid(); }
public class SingletonService : ISingletonService { public Guid Id { get; } = Guid.NewGuid(); }
