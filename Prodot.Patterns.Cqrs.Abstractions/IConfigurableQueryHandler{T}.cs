namespace Prodot.Patterns.Cqrs;

public interface IConfigurableQueryHandler<TConfiguration>
{
    TConfiguration Configuration { set; }
}
