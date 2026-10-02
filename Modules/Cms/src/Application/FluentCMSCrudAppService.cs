using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Services;
using ByteLabs.Foundations.Domain.Entities;
using ByteLabs.Foundations.Domain.Repositories;
using FluentCMS.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services;

public abstract class FluentCMSCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput> : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput> where TEntity : class, IEntity<TKey>
{

    protected FluentCMSCrudAppService(IRepository<TEntity, TKey> repository)
        : base(repository)
    {
        LocalizationResource = typeof(FluentCMSResource);
        ObjectMapperContext = typeof(FluentCMSApplicationModule);
    }


    public override async Task<TEntityDto> CreateAsync(TCreateInput input)
    {
        await OnBeforeCreateAsync(input);
        var model = await base.CreateAsync(input);
        return await OnAfterCreateAsync(model);
    }

    public virtual Task OnBeforeCreateAsync(TCreateInput input, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task<TEntityDto> OnAfterCreateAsync(TEntityDto input, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(input);
    }

    public override async Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input)
    {
        await OnBeforeUpdateAsync(id, input);
        var model = await base.UpdateAsync(id, input);
        return await OnAfterUpdateAsync(model);
    }

    public virtual Task OnBeforeUpdateAsync(TKey id, TUpdateInput input, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task<TEntityDto> OnAfterUpdateAsync(TEntityDto input, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(input);
    }

    public override async Task DeleteAsync(TKey id)
    {
        await OnBeforeDeleteAsync(id);
        await base.DeleteAsync(id);

        using (DataFilter.Disable<ISoftDelete>())
        {
            var model = await GetAsync(id);
            await OnAfterDeleteAsync(model);
        }
    }

    public virtual Task OnBeforeDeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnAfterDeleteAsync(TEntityDto model, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

}
