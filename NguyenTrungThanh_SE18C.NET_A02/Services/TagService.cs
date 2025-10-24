using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public List<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Tag GetOrCreateTagByName(string tagName)
        {
            return _tagRepository.GetOrCreateTagByName(tagName);
        }
    }
}