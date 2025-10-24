using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface ITagRepository
    {
        Tag GetOrCreateTagByName(string tagName);
        List<Tag> GetAllTags();
    }
}