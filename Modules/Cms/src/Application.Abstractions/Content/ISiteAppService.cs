using FluentCMS.Membership;
using FluentCMS.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface ISiteAppService : ICrudAppService<Site, Guid, GetSiteRequest, SiteCreateRequest, SiteUpdateRequest>
{
}

public interface ISiteService : IApplicationService
{
    Task<IEnumerable<Site>> GetAll(CancellationToken cancellationToken = default);
    Task<Site> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Site> GetByUrl(string url, CancellationToken cancellationToken = default);
    Task<Site> Create(SiteTemplate siteTemplate, CancellationToken cancellationToken = default);
    Task<Site> Update(Site site, CancellationToken cancellationToken = default);
    Task<Site> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<string> GetSitemap(Site site, Uri uri, CancellationToken cancellationToken = default);
}

public interface ISetupService : IApplicationService
{
    Task<IEnumerable<string>> GetTemplates(CancellationToken cancellationToken = default);
    Task<bool> Start(SetupTemplate setupTemplate, CancellationToken cancellationToken = default);
    Task<bool> IsInitialized(CancellationToken cancellationToken = default);
}

public class Site : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Urls { get; set; } = [];
    public Guid LayoutId { get; set; }
    public Guid DetailLayoutId { get; set; }
    public Guid EditLayoutId { get; set; }
}

public class SiteDetailResponse : Site
{
    public IEnumerable<Guid> AdminRoleIds { get; set; } = [];
    public IEnumerable<Guid> ContributorRoleIds { get; set; } = [];
    public List<RoleDetailResponse> AllRoles { get; set; } = [];
    public Dictionary<string, string> Settings { get; set; } = [];
}
public class GetSiteRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class SiteCreateRequest : ExtensibleEntityDto
{
    [Required]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    public string Template { get; set; } = default!;

    [Required]
    public string Url { get; set; } = default!;
}

public class SiteUpdateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    public List<string> Urls { get; set; } = [];

    [Required]
    public Guid LayoutId { get; set; } = default!;

    [Required]
    public Guid DetailLayoutId { get; set; } = default!;

    [Required]
    public Guid EditLayoutId { get; set; } = default!;

    [Required]
    public List<Guid> ContributorRoleIds { get; set; } = [];

    [Required]
    public List<Guid> AdminRoleIds { get; set; } = [];
}

public class SiteTemplate
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public string Template { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Layout> Layouts { get; set; } = [];
    public List<Block> Blocks { get; set; } = [];
    public List<ContentTypeTemplate> ContentTypes { get; set; } = [];
    public string Layout { get; set; } = default!;
    public string EditLayout { get; set; } = default!;
    public string DetailLayout { get; set; } = default!;
    public List<PageTemplate> Pages { get; set; } = [];
    public List<FileTemplate> Files { get; set; } = [];
    public List<FolderTemplate> Folders { get; set; } = [];
    public List<string> AdminRoles { get; set; } = [];
    public List<string> ContributorRoles { get; set; } = [];
    public List<Role> Roles { get; set; } = [];
    public Dictionary<string, string> Settings { get; set; } = [];


    public void Merge(SiteTemplate siteTemplate)
    {
        throw new NotImplementedException();
    }
}

public class SetupTemplate
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Template { get; set; } = default!;
    public SiteTemplate Site { get; set; } = default!;
    public List<PluginDefinition> PluginDefinitions { get; set; } = [];
}

public class SetupRequest:EntityDto
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    public string Template { get; set; } = default!;

    [Required]
    public string Url { get; set; } = default!;
}
