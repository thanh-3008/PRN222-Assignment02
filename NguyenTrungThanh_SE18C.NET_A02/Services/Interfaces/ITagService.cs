using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ITagService
    {
        Tag GetOrCreateTagByName(string tagName);
        List<Tag> GetAllTags();
    }
}