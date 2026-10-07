using System.Collections.Generic;

namespace FluentCMS.Content;

public class ContentTypeTemplate : ContentType
{
    public List<Dictionary<string, object?>> Contents { get; set; } = [];
}
